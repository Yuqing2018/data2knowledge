import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard'
import { Router, ActivatedRoute, NavigationCancel, NavigationEnd } from '@angular/router';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { AuthService } from 'src/app/core/auth.service';
import { combineLatest, forkJoin, of } from 'rxjs';
import { tap, switchMap } from 'rxjs/operators';
import { TaskService } from 'src/app/services/task.service';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { OntologyService } from 'src/app/services/ontology.service';
import { NzMessageService } from 'ng-zorro-antd';
import { NzModalService } from 'ng-zorro-antd';

@Component({
  selector: 'km-annotator-graph',
  templateUrl: './annotator-graph.component.html',
  styleUrls: ['./annotator-graph.component.less']
})

export class AnnotatorGraphComponent implements OnInit, AnnotationCanDeactivate {


  taskName: string
  workspaceId: string
  workspaceType: string
  userInfo: UserInfo;
  taskId: string
  docId: string
  docList: any = []
  tagList: any
  taskDocumentStatus: string
  src: any
  docName: string
  index: number = 1
  currentResult: any
  oldResult: any
  isLoading: boolean
  item: false
  isJumper: boolean = false

  @ViewChild('graphTool') toolComponent
  @ViewChild('graphTag') tagComponent

  get notHasPagePrevious() {
    return this.index <= 1;
  }

  get notHasPageNext() {
    return this.docList && this.index && this.index >= this.docList.length
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private taskService: TaskService,
    private docService: PreMarkService,
    private entityService: OntologyService,
    private message: NzMessageService,
    private modal: NzModalService
  ) { }

  ngOnInit() {
    this.userInfo = this.authService.getUser();
    combineLatest(
      this.route.params,
      this.route.queryParams
    ).pipe(
      tap(([t, q]) => {
        this.isLoading = true
        this.workspaceId = q.workspaceId;
        this.taskId = t.task;
        this.docId = t.doc;
        this.workspaceType = q.type;
      }),
      switchMap(() => this.getDocList()),
      switchMap(() => this.getTagList()),
    ).subscribe(() => {
      this.getDocData()
    });
  }

  getDocList(): any {
    if (this.docList && this.docList.length > 0) {
      return of(false)
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
      return of(false)
    } else {
      return this.entityService.getOntologyEntity(this.workspaceId).pipe(
        tap(res => {
          this.tagList = res.items
        })
      )
    }
  }

  getDocData() {
    forkJoin(
      this.getResult(this.docId),
      this.docService.getGraph(this.workspaceId, this.docId)
    ).subscribe(([result, content]) => {
      this.docName = this.docList.find(val => val.id === this.docId).name
      this.index = this.docList.findIndex(val => val.id === this.docId) + 1
      this.taskDocumentStatus = this.docList[this.index - 1].status
      this.currentResult = result || this.initResult()
      this.oldResult = JSON.parse(JSON.stringify(this.currentResult))
      this.src = this.formateImg(content)
      this.toolComponent.tags = this.currentResult[0].tags
      this.isLoading = false
    });

  }

  getResult(docId: string) {
    return this.taskService.getAnnotationResult(this.workspaceId, this.taskId, docId).pipe(
      switchMap((res: any) => {
        if (res && res.length > 0) {
          let doc = res.find(r => r.annotatedBy.id == this.userInfo.id);
          if (doc) {
            this.taskDocumentStatus = doc.taskDocumentStatus;
            return this.docService.downloadDocument(this.workspaceId, doc.resultDocumentId);
          } else {
            return of(false);
          }
        } else {
          return of(false);
        }
      })
    )
  }

  initResult() {
    return [{
      id: this.docList[this.index - 1].id,
      name: this.docList[this.index - 1].name,
      tags: []
    }]
  }

  formateImg(data: any) {
    let src = ''
    let bytes = new Uint8Array(data)
    for (let len = bytes.length, i = 0; i < len; i++) {
      src += String.fromCharCode(bytes[i])
    }
    return 'data:image/png;base64,' + window.btoa(src)
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

  jumperItem(itemIndex: number) {
    if (itemIndex >= 0 && itemIndex < this.docList.length) {
      this.index = itemIndex + 1
      this.isJumper = true
      this.getContent(this.index)
    } else {
      this.message.error('请输入合法数字')
    }
  }

  pagePrevious() {
    let index = this.index - 1
    index = Math.max(1, index)
    this.getContent(index)
  }

  onSave(status: string) {
    if (status !== 'Assigned') {
      this.modal.confirm({
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
    let result = [{
      id: this.docList[this.index - 1].id,
      name: this.docList[this.index - 1].name,
      tags: this.toolComponent.tags
    }]
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: 'ForModelTraining',
      status: status
    }).subscribe(res => {
      this.message.success('操作成功')
      this.currentResult = result
      this.oldResult = JSON.parse(JSON.stringify(this.currentResult))
      this.taskDocumentStatus = status
    })
  }

  pageNext() {
    let index = this.index + 1
    index = Math.min(index, this.docList.length)
    this.getContent(index)
  }

  getContent(index: number) {
    this.docId = this.docList[index - 1].id
    this.router.navigate([`../../${this.docId}/graph`], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      }
    })
  }


  selectedTag(id: string) {
    if (this.toolComponent.tags.some(val => val.id === id)) {
      this.toolComponent.tags = this.toolComponent.tags.filter(t => t.id !== id);
      return
    }
    this.toolComponent.tags.push(this.tagList.find(val => val.id === id))
  }


  canDeactivate() {
    if (this.isJumper) {
      this.isJumper = false
      return true
    }
    return this.taskDocumentStatus == 'Annotated' || JSON.stringify(this.currentResult) === JSON.stringify(this.oldResult)
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }



}
