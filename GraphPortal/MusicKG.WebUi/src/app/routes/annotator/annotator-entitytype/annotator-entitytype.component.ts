import { Component, OnInit, OnDestroy, HostListener, ViewChild } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { Subject, combineLatest, of, forkJoin } from 'rxjs'
import { tap, switchMap } from 'rxjs/operators'
import { UserInfo } from 'src/app/interfaces/user.interface'
import { EntitytypeData } from '../../../interfaces/entitytype'
import { AnnotationCanDeactivate } from '../../../core/can-deactivate.guard'
import { NzModalService, NzMessageService } from 'ng-zorro-antd'
import { OntologyDialogueService } from 'src/app/services/ontology-dialogue.service'
import { PreMarkService } from 'src/app/services/pre-mark.service'
import { TaskService } from 'src/app/services/task.service'
import { AuthService } from 'src/app/core/auth.service'
import { EntitytypeAnnotationToolComponent } from '../../../shared/entitytype-annotation-tool/entitytype-annotation-tool.component'

@Component({
  selector: 'km-annotator-entitytype',
  templateUrl: './annotator-entitytype.component.html',
  styleUrls: ['./annotator-entitytype.component.less']
})
export class AnnotatorEntitytypeComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {

  taskDocumentStatus: string
  nodes: EntitytypeData[]
  oldResult: EntitytypeData[]
  documentName: string = ''
  taskName: string
  workspaceId: string
  workspaceType: string
  userInfo: UserInfo
  taskId: string
  docId: string
  docList: any
  isLoading: true

  private unsubscribe = new Subject<void>()
  @ViewChild(EntitytypeAnnotationToolComponent) tool: EntitytypeAnnotationToolComponent


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private modalService: NzModalService,
    private message: NzMessageService,
    private authService: AuthService,
    private taskService: TaskService,
    private dialogService: OntologyDialogueService,
    private docService: PreMarkService,
  ) { }

  ngOnInit() {
    this.userInfo = this.authService.getUser()
    combineLatest(
      this.route.params,
      this.route.queryParams
    ).pipe(
      tap(([t, q]) => {
        this.workspaceId = q.workspaceId
        this.taskId = t.task
        this.docId = t.doc
        this.workspaceType = q.type
      }),
      switchMap(() => this.getDocList())
    ).subscribe(() => {
      this.getEntityData()
    })
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
        this.getDocData()
      }
    })
  }

  getDocData() {
    forkJoin(
      this.getResult(this.docId),
      this.docService.downloadDocument(this.workspaceId, this.docId)
    ).subscribe(([result, content]) => {
      this.taskDocumentStatus = this.docList[0].status
      this.nodes = result || content
      this.oldResult = [].concat(JSON.parse(JSON.stringify(this.nodes)))
      this.documentName = this.nodes[0].EntityType
    })
  }

  getResult(docId: string) {
    return this.taskService.getAnnotationResult(this.workspaceId, this.taskId, docId).pipe(
      switchMap((res: any) => {
        if (res && res.length > 0) {
          let doc = res.find(r => r.annotatedBy.id == this.userInfo.id)
          if (doc) {
            this.taskDocumentStatus = doc.taskDocumentStatus
            return this.docService.downloadDocument(this.workspaceId, doc.resultDocumentId)
          } else {
            return of(false)
          }
        } else {
          return of(false)
        }
      })
    )
  }

  getDocList() {
    return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
      tap(res => {
        this.docList = res.items
      })
    )
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
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(this.tool.nodes),
      resultType: 'ForModelTraining',
      status: status
    }).subscribe(res => {
      this.message.success('保存成功')
      this.taskDocumentStatus = status
      this.oldResult = [].concat(JSON.parse(JSON.stringify(this.tool.nodes)))
    })
  }

  ngOnDestroy() {
    this.unsubscribe.next()
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || JSON.stringify(this.tool.nodes) == JSON.stringify(this.oldResult)
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true
    }
  }

  currentItem(idx: number) {
    this.documentName = this.nodes[idx].EntityType
  }

}
