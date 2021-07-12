import { Component, OnInit, ViewChild, HostListener, OnDestroy } from '@angular/core'
import { NzModalService, NzMessageService } from 'ng-zorro-antd'
import { EntitytypeAnnotationToolComponent } from '../../../shared/entitytype-annotation-tool/entitytype-annotation-tool.component'
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard'
import { combineLatest, forkJoin, of, Subject } from 'rxjs'
import { tap, switchMap, map } from 'rxjs/operators'
import { EntitytypeData } from '../../../interfaces/entitytype'
import { Router, ActivatedRoute } from '@angular/router'
import { TaskService } from 'src/app/services/task.service'
import { OntologyDialogueService } from 'src/app/services/ontology-dialogue.service'
import { PreMarkService } from 'src/app/services/pre-mark.service'
import { UserInfo } from 'src/app/interfaces/user.interface'
import { AuthService } from 'src/app/core/auth.service'


@Component({
  selector: 'km-entitytype',
  templateUrl: './entitytype.component.html',
  styleUrls: ['./entitytype.component.less']
})
export class EntitytypeComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {

  annotatorList: any
  taskDocumentStatus: string
  nodes: EntitytypeData[]
  oldResult: EntitytypeData[]
  documentName: string = ''
  taskName: string
  workspaceType: string
  workspaceId: string
  taskId: string
  docId: string
  docList: any
  userInfo: UserInfo
  taskInfo: any
  docContentSource: any
  isLoading: boolean = true


  private unsubscribe = new Subject<void>()
  @ViewChild(EntitytypeAnnotationToolComponent) tool: EntitytypeAnnotationToolComponent

  constructor(
    private modalService: NzModalService,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private taskService: TaskService,
    private dialogService: OntologyDialogueService,
    private docService: PreMarkService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.userInfo = this.authService.getUser();
    this.getWorkSpaceAndTaskDocIds().pipe(
      tap(([w, t]) => {
        this.workspaceId = w.workspace;
        this.taskId = t.task;
        this.docId = t.doc;
      }),
      switchMap(() => this.getDocList())
    ).subscribe(() => {
      this.getEntityData()
    })
  }

  getWorkSpaceAndTaskDocIds() {
    return combineLatest(
      this.route.pathFromRoot[1].params,
      this.route.pathFromRoot[3].params
    )
  }

  getDocList() {
    return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
      tap(res => {
        this.docList = res.items
      })
    )
  }

  getEntityData() {
    this.dialogService.getOntology(this.workspaceId).pipe(
      switchMap((d: any) => {
        if (d.entityDocumentId) {
          return this.docService.downloadDocument(this.workspaceId, d.entityDocumentId)
        } else {
          of(false)
        }
      })
    ).subscribe(res => {
      if (res) {
        this.nodes = res
        this.oldResult = JSON.parse(JSON.stringify(this.nodes))
        this.getDocData()
      }
    })
  }

  getDocData() {
    this.taskService.getAnnotationResult(this.workspaceId, this.taskId, this.docId).subscribe((res: any) => {
      this.annotatorList = res.map(r => {
        let name = r.annotatedBy.roles.indexOf('Manager') !== -1 ? `审批人(${r.annotatedBy.name})` : r.annotatedBy.name;
        return { label: name, value: r.annotatedBy.name, checked: false, role: r.annotatedBy.roles[0], resultDocumentId: r.resultDocumentId }
      })
      let doc = res.find(r => r.annotatedBy.id == this.userInfo.id)
      if (doc) {
        this.taskDocumentStatus = doc.taskDocumentStatus
      } else {
        this.taskDocumentStatus = 'Assigned'
      }
      this.initAnnotationContent(this.annotatorList)
    })
  }

  initAnnotationContent(res: any) {
    forkJoin(
      this.getResultList(res),
      this.docService.downloadDocument(this.workspaceId, this.docId),
      this.taskService.get(this.workspaceId, this.taskId)
    ).subscribe(([res, doc, task]) => {
      this.taskInfo = task
      this.annotatorList = res || []
      this.docContentSource = JSON.parse(JSON.stringify(doc))
      this.documentName = this.docContentSource[0].EntityType
      if (res && this.taskInfo.status !== 'Created') {
        this.annotatorList = this.annotatorList.map(d => {
          let hasManager = this.annotatorList.find(d => d.role == 'Manager');
          d.checked = hasManager ? (d.role == 'Manager' ? true : false) : (d.role !== 'Manager' ? true : false)
          return d
        });
        this.selectedAnnotator()
      } else {
        this.isLoading = false
      }
    })
  }

  selectedAnnotator() {
    let selectedNodes = this.annotatorList.filter(val => val.checked).map(val => val.labelData)
    if (selectedNodes.length === 0) {
      this.nodes = JSON.parse(JSON.stringify(this.docContentSource))
    } else if (selectedNodes.length === 1) {
      this.nodes = selectedNodes[0]
    } else {
      this.nodes = this.formateLabelData(selectedNodes)
    }
    this.oldResult = JSON.parse(JSON.stringify(this.nodes))
    this.isLoading = false
  }

  formateLabelData(data) {
    let first = JSON.parse(JSON.stringify(data))[0]
    for (let i = 0; i < first.length; i++) {
      let idArr = data.map(val => val.find(el => el.Id === first[i].Id))
      let result = Array.from(new Set(this.flattenDeep(idArr.map(val => val.Entries.map(el => el.Value))))).map(val => {
        let target = idArr.map(item => {
          let a = item.Entries.find(el => el.Value[0] === val)
          return a ? a.Synonyms : []
        })
        let newTartget = Array.from(new Set(this.flattenDeep(target)))
        return {
          Value: [val],
          Synonyms: newTartget.map(val => [val])
        }
      })
      first[i].Entries = result
    }
    return first
  }

  getResultList(result: any): any {
    if (result && result.length > 0) {
      return forkJoin(
        result.map(r => this.docService.downloadDocument(this.workspaceId, r.resultDocumentId).pipe(
          map(d => {
            return { ...r, labelData: d }
          })
        ))
      );
    } else {
      return of(false)
    }
  }

  save(type: string) {
    let hasEmptyEntity = this.tool.nodes.some(val => val.Entries.some(el => el.Value.length === 0))
    if (hasEmptyEntity) {
      this.message.error('检测到空白实体，请正确填写实体')
      return
    }
    if (type === 'Annotated') {
      this.modalService.confirm({
        nzTitle: '确认信息?',
        nzContent: '保存并提交后的信息将不能更改',
        nzOnOk: () => {
          this.saveResult(type)
        }
      })
    } else {
      this.saveResult(type)
    }
  }

  saveResult(status) {
    let result = this.formateResult(this.tool.nodes)
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: 'ForModelTraining',
      status: status
    }).subscribe(res => {
      this.message.success('保存成功')
      this.taskDocumentStatus = status
      this.getDocData()
    })
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || JSON.stringify(this.tool.nodes) == JSON.stringify(this.oldResult)
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }


  labelChange(label: any) {
    if (label.checked) {
      this.annotatorList = this.annotatorList.map(val => {
        if (val.role !== label.role) val.checked = false
        return val
      })
    }
    this.selectedAnnotator()
  }

  goTo() {
    this.router.navigate(['../../../../'], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      }
    })
  }

  currentItem(idx) {
    this.documentName = this.docContentSource[idx].EntityType
  }

  formateResult(data) {
    return JSON.parse(JSON.stringify(data)).map(val => {
      delete val.children
      delete val.disabled
      delete val.level
      delete val.open
      delete val.selected
      return val
    })
  }

  // Compatible with IE
  flattenDeep(arr) {
    return arr.reduce((acc, val) => Array.isArray(val) ? acc.concat(this.flattenDeep(val)) : acc.concat(val), []);
  }
}
