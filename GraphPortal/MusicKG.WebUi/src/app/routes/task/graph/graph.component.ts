import { Component, OnInit, ViewChild, HostListener, OnDestroy } from '@angular/core'
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard'
import { Router, ActivatedRoute } from '@angular/router'
import { UserInfo } from 'src/app/interfaces/user.interface'
import { AuthService } from 'src/app/core/auth.service'
import { combineLatest, forkJoin, of, Subject } from 'rxjs'
import { tap, switchMap, map } from 'rxjs/operators'
import { TaskService } from 'src/app/services/task.service'
import { OntologyService } from 'src/app/services/ontology.service'
import { PreMarkService } from 'src/app/services/pre-mark.service'
import { NzModalService, NzMessageService } from 'ng-zorro-antd'

@Component({
  selector: 'km-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.less']
})
export class GraphComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {

  taskName: string
  workspaceId: string
  workspaceType: string
  userInfo: UserInfo;
  taskId: string
  docId: string
  docList: any = []
  tagList: any
  taskDocumentStatus: string
  docName: string
  index: number = 1
  annotatorList: any;
  taskInfo: any
  docContentSource: any
  hasReviewPermission: any
  src: string
  oldResult: any
  isLoading: boolean = false
  isJumper: boolean = false
  item: boolean = false

  private unsubscribe = new Subject<void>()

  @ViewChild('graphTool') tool

  get notHasPagePrevious() {
    return this.index <= 1
  }

  get notHasPageNext() {
    return this.docList && this.index && this.index >= this.docList.length
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private taskService: TaskService,
    private entityService: OntologyService,
    private docService: PreMarkService,
    private message: NzMessageService,
    private modalService: NzModalService
  ) { }

  ngOnInit() {
    this.userInfo = this.authService.getUser();
    this.getWorkSpaceAndTaskDocIds().pipe(
      tap(([w, t]) => {
        this.isLoading = true
        this.workspaceId = w.workspace
        this.taskId = t.task
        this.docId = t.doc
      }),
      switchMap(() => this.getDocList()),
      switchMap(() => this.getTagList()),
    ).subscribe(() => {
      this.getDocData()
    })
  }

  getWorkSpaceAndTaskDocIds() {
    return combineLatest(
      this.route.pathFromRoot[1].params,
      this.route.pathFromRoot[3].params
    )
  }

  getDocList(): any {
    if (this.docList && this.docList.length > 0) {
      return of(false);
    } else {
      return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
        tap(res => {
          this.docList = res.items
        })
      )
    }
  }

  getTagList(): any {
    if (this.tagList && this.tagList.length > 0) {
      return of(false);
    } else {
      return this.entityService.getOntologyEntity(this.workspaceId).pipe(
        tap(res => {
          this.tagList = res.items
        })
      )
    }
  }

  getDocData() {
    this.taskService.getAnnotationResult(this.workspaceId, this.taskId, this.docId).subscribe((res: any) => {
      this.annotatorList = res.map(r => {
        let name = r.annotatedBy.roles.indexOf('Manager') !== -1 ? `审批人(${r.annotatedBy.name})` : r.annotatedBy.name;
        return { label: name, value: r.annotatedBy.name, checked: false, role: r.annotatedBy.roles[0], resultDocumentId: r.resultDocumentId };
      });
      let doc = res.find(r => r.annotatedBy.id == this.userInfo.id);
      if (doc) {
        this.taskDocumentStatus = doc.taskDocumentStatus;
      } else {
        this.taskDocumentStatus = 'Assigned';
      }
      this.initAnnotationContent(this.annotatorList);
    });
  }

  initAnnotationContent(res: any) {
    forkJoin(
      this.getResultList(res),
      this.docService.getGraph(this.workspaceId, this.docId),
      this.taskService.get(this.workspaceId, this.taskId)
    ).subscribe(([res, doc, task]) => {
      this.docName = this.docList.find(val => val.id === this.docId).name
      this.index = this.docList.findIndex(val => val.id === this.docId) + 1
      this.index = this.index
      this.taskInfo = task;
      this.annotatorList = res;
      if (!res) this.tool.tags = []
      this.src = this.formateImg(doc)
      this.hasReviewPermission = this.docList[0].annotators.find(user => user.id == this.userInfo.id);
      if (res && this.taskInfo.status !== 'Created') {
        this.annotatorList = this.annotatorList.map(d => {
          let hasManager = this.annotatorList.find(d => d.role == 'Manager');
          d.checked = hasManager ? (d.role == 'Manager' ? true : false) : (d.role !== 'Manager' ? true : false);
          return d;
        });
      }
      this.selectedAnnotator();
    });
  }

  getResultList(result: any): any {
    if (result && result.length > 0) {
      return forkJoin(
        result.map(r => this.docService.downloadDocument(this.workspaceId, r.resultDocumentId).pipe(
          map(d => {
            return { ...r, labelData: d };
          })
        ))
      );
    } else {
      return of(false);
    }
  }

  formateImg(data: any) {
    let src = ''
    let bytes = new Uint8Array(data)
    for (let len = bytes.length, i = 0; i < len; i++) {
      src += String.fromCharCode(bytes[i])
    }
    return 'data:image/png;base64,' + window.btoa(src)
  }

  selectedAnnotator() {
    if (!this.annotatorList) {
      this.isLoading = false
      return
    }
    let data
    let selected = this.annotatorList.filter(val => val.checked)
    if (selected.length === 0) {
      data = []
    } else if (selected[0].role === 'Manager') {
      data = selected[0].labelData.tags
    } else {
      data = selected.map(val => val.labelData[0].tags)
    }
    let labelData = this.unique(this.flattenDeep(data))
    this.tool.tags = labelData
    this.oldResult = JSON.parse(JSON.stringify(this.tool.tags))
    this.isLoading = false
  }

  flattenDeep(arr: Array<any>) {
    return arr.reduce((acc, val) => Array.isArray(val) ? acc.concat(this.flattenDeep(val)) : acc.concat(val), []);
  }

  unique(arr: Array<any>) {
    let hash = []
    let result = arr.reduce((item, next) => {
      hash[next.name] ? '' : hash[next.name] = true && item.push(next);
      return item
    }, [])
    return result
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
    this.router.navigate(['../../../'], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      }
    })
  }

  selectedTag(id: string) {
    let currentTag = this.tool.tags
    if (currentTag.some(val => val.id === id)) {
      this.tool.tags = this.tool.tags.filter(t => t.id !== id);
      return
    }
    this.tool.tags.push(this.tagList.find(val => val.id === id))
  }

  pagePrevious() {
    let index = this.index - 1
    index = Math.max(1, index)
    this.getContent(index)
  }

  pageNext() {
    let index = this.index + 1
    index = Math.min(index, this.docList.length)
    this.getContent(index)
  }

  jumperItem(itemIndex: number) {
    if (itemIndex >= 0 && itemIndex < this.docList.length) {
      this.index = itemIndex + 1
      this.isJumper = true
      this.getContent(this.index)
    } else {
      this.message.error('请输入合法数字')
    }
  }

  getContent(index: number) {
    this.docId = this.docList[index - 1].id
    this.router.navigate([`../../${this.docId}/graph`], {
      relativeTo: this.route
    })
  }

  onSave(status: string) {
    if (status === 'Annotated') {
      this.modalService.confirm({
        nzTitle: '确认信息?',
        nzContent: '保存并提交后的信息将不能更改',
        nzOnOk: () => {
          this._save(status)
        }
      })
    } else {
      this._save(status)
    }
  }

  _save(status: string) {
    let result = {
      id: this.docId,
      name: this.docName,
      tags: this.tool.tags
    }
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: 'ForModelTraining',
      status: status
    }).subscribe(res => {
      this.message.success('操作成功')
      this.tool.tags = result.tags
      this.oldResult = JSON.parse(JSON.stringify(this.tool.tags))
      this.taskDocumentStatus = status
      this.getDocData()
    })
  }

  canDeactivate() {
    if (this.isJumper) {
      this.isJumper = false
      return true
    }
    return this.taskDocumentStatus == 'Annotated' || this.taskInfo.status !== 'Accepted' || JSON.stringify(this.tool.tags) === JSON.stringify(this.oldResult)
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }
}
