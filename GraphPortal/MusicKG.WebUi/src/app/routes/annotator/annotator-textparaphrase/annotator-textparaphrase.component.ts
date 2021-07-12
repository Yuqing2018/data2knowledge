import { Component, OnInit, OnDestroy, HostListener, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject, forkJoin, of } from 'rxjs';
import { combineLatest } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { TaskService } from 'src/app/services/task.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { AuthService } from 'src/app/core/auth.service';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard';
import { TextparaphraseAnnotationToolComponent } from 'src/app/shared/textparaphrase-annotation-tool/textparaphrase-annotation-tool.component';

@Component({
  selector: 'km-annotator-textparaphrase',
  templateUrl: './annotator-textparaphrase.component.html',
  styleUrls: ['./annotator-textparaphrase.component.less']
})
export class AnnotatorTextparaphraseComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {

  private unsubscribe = new Subject<void>();
  workspaceId: string;
  taskId: string;
  docId: string;
  taskName: string;
  workspaceType: string;
  docList: any
  currentDoc: any
  docName: string = '文档'
  pageIndex: number = 2
  pageTotal: number
  currentDocContent: any
  guideline: string
  itemTotal: number = 0
  itemIndex: number = 0
  userInfo: UserInfo
  documentStatus: string
  annotationData: any
  disabled: boolean = false
  oldResult: any
  currentResult: any = []
  _currentItemContent: any;

  @ViewChild('tool') tool: TextparaphraseAnnotationToolComponent


  get SaveBtn() {
    return this.documentStatus !== 'Assigned' || this.disabled
  }

  set currentItemContent(value: any) {
    this._currentItemContent = {
      Id: value.Id,
      Subject: value.Subject,
      Predicate: value.Predicate,
      Object: value.Object,
      ExampleQuestions: value.ExampleQuestions,
      guideline: this.guideline,
      preResult: value.preResult || []
    };
  }

  get currentItemContent() {
    return this._currentItemContent
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private docService: PreMarkService,
    private taskService: TaskService,
    private message: NzMessageService,
    private modal: NzModalService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    combineLatest(
      this.route.params,
      this.route.queryParams
    ).pipe(
      takeUntil(this.unsubscribe),
      tap(([t, q]) => {
        this.workspaceId = q.workspaceId;
        this.taskId = t.task;
        this.docId = t.doc;
        this.taskName = q.name;
        this.workspaceType = q.type;
      })
    ).subscribe(() => {
      this.userInfo = this.authService.getUser()
      this.getTaskDocumentList()
      this.initContent(this.docId);
    });

  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  initContent(docId: string) {
    this.getDocContent(docId, this.itemIndex);
  }

  getDocContent(docId: string, itemIndex?: number) {
    forkJoin(
      this.docService.downloadDocument(this.workspaceId, docId)
    ).subscribe(([content]) => {
      this.currentDocContent = content
      this.guideline = content.Guideline
      this.currentItemContent = content.Items[itemIndex];
      this.itemTotal = content.Items.length
      this.getLaberResult()
    });
  }

  getLaberResult() {
    this.taskService.getAnnotationResult(this.workspaceId, this.taskId, this.docId).subscribe((res: any) => {
      let userResult = res.filter(val => val.annotatedBy.id === this.userInfo.id)
      if (userResult && userResult.length === 0) {
        this.documentStatus = 'Assigned'
      } else {
        this.documentStatus = userResult[0].taskDocumentStatus
      }
      this.getResultList(userResult).subscribe(res => {
        if (!res) {
          this.annotationData = this.initAnnotationData()
          this.oldResult = this.initAnnotationData()
          this.currentResult = []
        } else {
          this.annotationData = res[0].Results
          this.oldResult = res[0].Results
          this.initCurrentResult()
        }
        this.currentItemContent = Object.assign(this.currentItemContent, { preResult: this.getCurrentItemResult() })
      })
    })
  }

  initAnnotationData() {
    return this.currentDocContent.Items.map(item => {
      return {
        Item: { ...item },
        LabelerResults: [
          {
            LabelerId: this.userInfo.id,
            LabelerRole: 'Annotator',
            SimilarQuestions: []
          }
        ]
      }
    })
  }

  initCurrentResult() {
    this.annotationData.map((val, index) => {
      this.currentResult[index] = val.LabelerResults[0].SimilarQuestions
    })
  }

  getCurrentItemResult() {
    if (this.currentResult[this.itemIndex] == undefined) {
      return []
    }
    return this.currentResult[this.itemIndex]
  }

  getResultList(result: any): any {
    if (result && result.length > 0) {
      return forkJoin(
        result.map(r => this.docService.downloadDocument(this.workspaceId, r.resultDocumentId))
      );
    } else {
      return of(false);
    }
  }

  newFormData(data: any) {
    this.currentResult[this.itemIndex] = data
  }

  createComfireModal(callback: any) {
    if (this.tool.checkValue()) {
      callback()
      return
    }
    this.modal.confirm({
      nzTitle: '填写内容存在不合法数据，该操作将丢失不合法数据，是否继续？',
      nzOnOk: () => { callback() }
    })
  }

  onItemIndexChange(itemIndex: number) {
    this.createComfireModal(() => {
      this.tool.getCurrentData()
      this.tool.dataSet = null
      this.tool.focusIndex = null
      this.itemIndex = itemIndex
      this.currentItemContent = Object.assign({ preResult: this.getCurrentItemResult() }, this.currentDocContent.Items[this.itemIndex])
    })
  }

  onPageIndexChange(pageIndex: number) {
    this.itemIndex = 0
    this.tool.dataSet = null
    this.router.navigate([`../../${this.docList[pageIndex].id}/textparaphrase`], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      }
    })
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

  save(status: string) {
    if (this.tool.checkValue()) {
      if (status === 'Assigned') {
        this.creatResult(status)
      } else {
        this.modal.confirm({
          nzTitle: '确认信息?',
          nzContent: '保存并提交后的信息将不能更改',
          nzOnOk: () => {
            this.creatResult(status);
          }
        })
      }
    } else {
      this.modal.confirm({
        nzTitle: '填写内容存在不合法数据，该操作将丢失不合法数据，是否继续？',
        nzOnOk: () => {
          if (status === 'Assigned') {
            this.creatResult(status)
          } else {
            this.modal.confirm({
              nzTitle: '确认信息?',
              nzContent: '保存并提交后的信息将不能更改',
              nzOnOk: () => {
                this.creatResult(status);
              }
            })
          }
        }
      })
    }

  }

  creatResult(status: string) {
    let result = { Results: this.paresResult() }
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: 'ForModelTraining',
      status: status
    }).subscribe(res => {
      this.message.success('操作成功')
      this.documentStatus = status
      this.oldResult = result
    }, err => {
      this.message.error(err.msg)
    })
  }

  getTaskDocumentList() {
    this.taskService.getTaskDoc(this.workspaceId, this.taskId).subscribe(res => {
      this.pageTotal = res.totalCount
      this.docList = res.items
      this.currentDoc = this.docList.find(val => {
        return val.id == this.docId
      })
      this.docName = this.currentDoc.name
      this.pageIndex = this.docList.indexOf(this.currentDoc)
    })
  }

  paresResult() {
    this.tool.getCurrentData()
    let result = this.initAnnotationData()
    result.map((val, index) => {
      if (this.currentResult[index] != undefined) {
        val.LabelerResults[0].SimilarQuestions = this.currentResult[index]
      }
    })
    return result
  }

  canDeactivate() {
    return this.documentStatus == 'Annotated' || JSON.stringify(this.paresResult()) == JSON.stringify(this.oldResult)
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }

}
