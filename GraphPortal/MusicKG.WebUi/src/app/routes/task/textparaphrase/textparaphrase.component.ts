import { Component, OnInit, OnDestroy, HostListener, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, forkJoin, of } from 'rxjs';
import { takeUntil, tap, switchMap } from 'rxjs/operators';
import { TaskService } from 'src/app/services/task.service';
import { AuthService } from 'src/app/core/auth.service';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { ajv } from 'src/app/app.component';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { TextparaphraseAnnotationToolComponent } from 'src/app/shared/textparaphrase-annotation-tool/textparaphrase-annotation-tool.component';

@Component({
  selector: 'km-textparaphrase',
  templateUrl: './textparaphrase.component.html',
  styleUrls: ['./textparaphrase.component.less']
})
export class TextparaphraseComponent implements OnInit, OnDestroy {

  private unsubscribe = new Subject<void>();
  docContentSource: any
  docContent: any
  annotationReuslt: any
  oldResult: any
  showConflict: any
  userInfo: UserInfo
  workspaceId: string
  taskId: string
  docId: string
  docList: any
  pageIndex: number = 0
  docName: string = '文档'
  itemIndex: number = 0
  annotatorList: any
  documentStatus: string
  taskInfo: any
  hasReviewPermission: boolean = false
  guideline: string
  preResult: any = []
  currentResult: any = []
  checked: any = []
  newOldResult: any = []
  @ViewChild('tool') tool: TextparaphraseAnnotationToolComponent
  _currentItemContent: any;

  set currentItemContent(value: any) {
    this._currentItemContent = {
      Id: value.Id,
      Subject: value.Subject,
      Predicate: value.Predicate,
      Object: value.Object,
      ExampleQuestions: value.ExampleQuestions,
      guideline: this.guideline,
      preResult: this.preResult
    };
  }

  get saveBtn() {
    let disabled = false
    return !this.hasReviewPermission || this.documentStatus !== 'Assigned' || (this.taskInfo && this.taskInfo.status !== 'Accepted') || disabled
  }

  get currentItemContent() {
    return this._currentItemContent
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private authService: AuthService,
    private docService: PreMarkService,
    private message: NzMessageService,
    private modal: NzModalService
  ) { }

  ngOnInit() {
    this.userInfo = this.authService.getUser();
    this.route.params.pipe(
      takeUntil(this.unsubscribe),
      tap(t => {
        this.workspaceId = t.workspace;
        this.taskId = t.task;
        this.docId = t.doc;
      }),
      switchMap(() => this.getDocList())
    ).subscribe(() => {
      this.initDoc(this.docId);
    });
  }

  getDocList() {
    return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
      tap(res => {
        this.docList = res.items;
        this.pageIndex = this.docList.map((d: any) => d.id).indexOf(this.docId);
        this.docName = this.docList[this.pageIndex].name;
      })
    );
  }

  initDoc(docId: string) {
    this.itemIndex = 0;
    this.taskService.getAnnotationResult(this.workspaceId, this.taskId, docId).subscribe((res: any) => {
      this.annotatorList = res.map(r => {
        let name = r.annotatedBy.roles.indexOf('Manager') !== -1 ? `审批人(${r.annotatedBy.name})` : r.annotatedBy.name;
        return { label: name, value: r.annotatedBy.name, checked: false, role: r.annotatedBy.roles[0], id: r.annotatedBy.id };
      });
      let doc = res.find(r => r.annotatedBy.id == this.userInfo.id);
      if (doc) {
        this.documentStatus = doc.taskDocumentStatus;
      } else {
        this.documentStatus = 'Assigned';
      }
      this.initAnnotationContent(res);
    });
  }

  initAnnotationContent(res: any) {
    forkJoin(
      this.getResultList(res),
      this.docService.downloadDocument(this.workspaceId, this.docId),
      this.taskService.get(this.workspaceId, this.taskId)
    ).subscribe(([res, doc, task]) => {
      this.taskInfo = task;
      if (ajv.validate('textparaphrase', doc)) {
        this.docContentSource = JSON.parse(JSON.stringify(doc));
        this.docContent = doc;
        this.hasReviewPermission = this.docList[this.pageIndex].annotators.find(user => user.id == this.userInfo.id);
        if (res && this.taskInfo.status !== 'Created') {
          this.annotationReuslt = res;
          let reviewResult = this.annotationReuslt.find(d => this.userInfo.id == d.Results[0].LabelerResults[0].LabelerId);
          this.oldResult = reviewResult ? JSON.stringify(reviewResult) : null;
          this.annotatorList = this.annotatorList.map(d => {
            let hasManager = this.annotatorList.find(d => d.role == 'Manager');
            d.checked = hasManager ? (d.role == 'Manager' ? true : false) : (d.role !== 'Manager' ? true : false);
            return d;
          });
        }
        this.preResult = this.selectedAnnotator();
        this.guideline = this.docContent.Guideline
        this.currentItemContent = this.docContent.Items[this.itemIndex]
        this.docName = this.docList[this.pageIndex].name;
      } else {
        this.message.warning('不支持该文档格式');
      }
    });
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

  selectedAnnotator(status = 'nomarl') {
    let annotator = this.annotatorList.filter(val => val.checked)
    if (status !== 'checked') {
      this.checked[this.itemIndex] = annotator
      return this.getValue(annotator)
    } else {
      if (this.checked[this.itemIndex] == undefined) {
        this.checked[this.itemIndex] = annotator
        return this.getValue(annotator)
      } else {
        if (JSON.stringify(this.checked[this.itemIndex]) === JSON.stringify(annotator)) {
          return this.currentResult[this.itemIndex]
        } else {
          return this.getValue(annotator)
        }
      }
    }

  }

  getValue(annotator: any) {
    if (this.annotationReuslt == undefined) {
      this.annotationReuslt = []
    }
    let currentItemResult = this.annotationReuslt.map(val => {
      return val.Results[this.itemIndex]
    })
    let result = []
    annotator.map(val => {
      currentItemResult.map(item => {
        if (val.id === item.LabelerResults[0].LabelerId) {
          result = result.concat(item.LabelerResults[0].SimilarQuestions)
        }
      })
    })
    return Array.from(new Set(result))
  }

  createComfirmModal(callback: any) {
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
    this.createComfirmModal(() => {
      this.tool.getCurrentData()
      this.tool.dataSet = null
      this.itemIndex = itemIndex
      this.preResult = this.selectedAnnotator('checked')
      this.currentItemContent = this.docContent.Items[this.itemIndex]
    })

  }

  onPageIndexChange(pageIndex: number) {
    this.itemIndex = 0;
    this.router.navigate([`../../${this.docList[pageIndex].id}/textparaphrase`], {
      relativeTo: this.route
    });
  }

  getCurrentItemResult() {
    if (this.currentResult[this.itemIndex] == undefined) {
      return []
    }
    return this.currentResult[this.itemIndex]
  }

  onFormData(data: any) {
    this.currentResult[this.itemIndex] = data
  }

  labelChange(label: any) {
    if (label.checked && label.role == 'Manager') {
      this.annotatorList = this.annotatorList.map(d => {
        d.checked = d.role !== 'Manager' ? false : d.checked;
        return { ...d };
      });
    }
    if (label.checked && label.role !== 'Manager') {
      this.annotatorList = this.annotatorList.map(d => {
        d.checked = d.role == 'Manager' ? false : d.checked;
        return { ...d };
      });
    }
    let result = this.selectedAnnotator()
    this.preResult = result
    this.currentItemContent = this.docContent.Items[this.itemIndex]
  }

  save(status: string) {
    if (this.tool.checkValue()) {
      if (status == 'Assigned') {
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
          if (status == 'Assigned') {
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

  creatResult(status) {
    let result = { Results: this.parseResult() }
    // console.log(result)
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: 'ForModelTraining',
      status: status
    }).subscribe(res => {
      this.message.success('操作成功')
      this.documentStatus = status
      this.newOldResult = result
      this.initDoc(this.docId)
    }, err => {
      this.message.error(err.msg)
    })
  }

  parseResult() {
    this.tool.getCurrentData()
    let result = this.docContent.Items.map((item, index) => {
      return {
        Item: { ...item },
        LabelerResults: [
          {
            LabelerId: this.userInfo.id,
            LabelerRole: 'Manager',
            SimilarQuestions: []
          }
        ]
      }
    })
    result.map((val, index) => {
      if (this.currentResult[index] == undefined) {
        val.LabelerResults[0].SimilarQuestions = this.checkManager(index)
      } else {
        val.LabelerResults[0].SimilarQuestions = this.currentResult[index]
      }
    })
    return result
  }

  checkManager(index) {
    let manager = this.annotationReuslt.filter(val => val.Results[index].LabelerResults[0].LabelerRole === 'Manager')
    if (manager.length !== 0) {
      return manager[0].Results[index].LabelerResults[0].SimilarQuestions
    } else {
      return this.totalResult(index)
    }
  }

  totalResult(index) {
    let arr = []
    this.annotationReuslt.map(val => {
      arr = arr.concat(val.Results[index].LabelerResults[0].SimilarQuestions)
    })
    return arr
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  canDeactivate() {
    return this.documentStatus == 'Annotated' || (this.oldResult && this.oldResult == JSON.stringify({ Results: this.parseResult() })) || !this.oldResult;
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }

}
