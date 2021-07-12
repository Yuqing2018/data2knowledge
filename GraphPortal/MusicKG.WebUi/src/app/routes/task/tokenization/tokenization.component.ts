import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject, forkJoin, of } from 'rxjs';
import { combineLatest } from 'rxjs';
import { takeUntil, tap, switchMap } from 'rxjs/operators';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { TokenizationModel } from 'src/app/shared/tokenization-annotation-tool/tokenization-annotation-tool.component';
import { TaskService } from 'src/app/services/task.service';
import { AuthService } from 'src/app/core/auth.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { ajv } from 'src/app/app.component';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard';
import { UserInfo } from 'src/app/interfaces/user.interface';

@Component({
  selector: 'km-tokenization',
  templateUrl: './tokenization.component.html',
  styleUrls: ['./tokenization.component.less']
})
export class TokenizationComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {
  private unsubscribe = new Subject<void>();
  workspaceId: string;
  taskId: string;
  docId: string;
  annotationData: TokenizationModel;
  pageIndex: number = 0;
  taskDocumentStatus: string;
  docList: any[];
  docName: string;
  docContent: any;
  oldResult: string;
  docContentSource: any;
  userInfo: UserInfo;
  annotatorList: any;
  taskInfo: any;
  hasReviewPermission: boolean = false;
  annotationReuslt: any;
  showConflict = false;
  isLoading = false;

  get saveBtnDisabled() {
    return this.taskDocumentStatus !== 'Assigned' || !this.hasReviewPermission || (this.taskInfo && this.taskInfo.status !== 'Accepted');
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private docService: PreMarkService,
    private taskService: TaskService,
    private authService: AuthService,
    private msg: NzMessageService,
    private modalService: NzModalService
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
      this.isLoading = true;
      this.initDoc(this.docId);     
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || (this.oldResult && this.oldResult == JSON.stringify(this.parseResult())) || !this.oldResult;
  }

  initDoc(docId: string) {
    this.taskService.getAnnotationResult(this.workspaceId, this.taskId, docId).subscribe((res: any) => {
      this.annotatorList = res.map(r => {
        let name = r.annotatedBy.roles.indexOf('Manager') !== -1 ? `审批人(${r.annotatedBy.name})` : r.annotatedBy.name;
        return { label: name, value: r.annotatedBy.name, checked: false, role: r.annotatedBy.roles[0] };
      });
      let doc = res.find(r => r.annotatedBy.id == this.userInfo.id);
      if (doc) {
        this.taskDocumentStatus = doc.taskDocumentStatus;
      } else {
        this.taskDocumentStatus = 'Assigned';
      }
      this.initAnnotationContent(res);
    });
  }

  initAnnotationContent(res: any) {
    forkJoin(
      this.getResultList(res),
      this.docService.downloadDocument(this.workspaceId, this.docId),
      this.taskService.get(this.workspaceId,this.taskId)
    ).subscribe(([res, doc, task]) => {
      this.taskInfo = task;
      if (ajv.validate('tokenization', doc)) {
        this.docContentSource = JSON.parse(JSON.stringify(doc));
        this.docContent = doc;
        this.hasReviewPermission = this.docList[this.pageIndex].annotators.find(user => user.id == this.userInfo.id);
        if (res && this.taskInfo.status !== 'Created') {
          this.annotationReuslt = res;
          let reviewResult = this.annotationReuslt.find(d => this.userInfo.name == d.Results[0].LabelerResults[0].LabelerId);
          this.oldResult = reviewResult ? JSON.stringify(reviewResult) : null;
          this.annotatorList = this.annotatorList.map(d => {
            let hasManager = this.annotatorList.find(d => d.role == 'Manager');
            d.checked = hasManager ? (d.role == 'Manager' ? true : false) : (d.role !== 'Manager' ? true : false);
            return d;
          });
        }
        this.selectedAnnotator();  
        this.docName = this.docList[this.pageIndex].name;
      } else {
        this.msg.warning('不支持该文档格式');
      }
      this.isLoading = false;
    });
  }

  selectedAnnotator() {
    let selectedUser = this.annotatorList.filter(l => l.checked).map(d => d.value);
    this.docContent.Items = this.docContent.Items.map((item: any, index: number) => {
      if (selectedUser && selectedUser.length > 0) {
        let userResult = this.annotationReuslt.filter(d => {
          return selectedUser.find(u => u == d.Results[index].LabelerResults[0].LabelerId);
        }).map(d => {
          return d.Results[index].LabelerResults[0].LabelResults;
        });
        let spanItemsResult = userResult[0].filter(label => {
              let allExist = true;
              for(let uIndex = 1; uIndex < userResult.length; uIndex++) {
                allExist = allExist && !!userResult[uIndex].find(r => r.Start == label.Start && r.End == label.End);
              }
              return allExist;
        });
        item.SpanItems = spanItemsResult;
      } else {
        item.SpanItems = this.docContentSource.Items[index].SpanItems;
      }
      this.showConflict = selectedUser && selectedUser.length > 1;
      return { ...item };
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


  onSave(status: string) {
    if (status == 'Assigned') {
      this._saveResult(status);
    } else {
      this.modalService.confirm({
        nzTitle: '确认信息?',
        nzContent: '保存并提交后的信息将不能更改',
        nzOnOk: () => {
          this._saveResult(status);
        }
      });
    }
  }

  private _saveResult(status: string) {
    let result = this.parseResult();
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: "ForModelTraining",
      status: status
    }).subscribe(res => {
      this.msg.success('操作成功');
      this.oldResult = JSON.stringify(result);
      this.initDoc(this.docId);
      this.taskDocumentStatus = status;
    });
  }

  parseResult() {
    return {
      Results: this.docContent.Items.map(item => {
        return {
          Item: { Id: item.Id, Text: item.Text },
          LabelerResults: [
            { LabelerId: this.userInfo.name, LabelResults: item.SpanItems.map(i => {
              return {
                Start: i.Start,
                End: i.End
              }
            }), LabelerRole: "Manager" }
          ]
        }
      })
    };
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
    this.selectedAnnotator();  
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

  getDocList() {
    return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
      tap(res => {
        this.docList = res.items;
        this.pageIndex = this.docList.map((d: any) => d.id).indexOf(this.docId);
        this.docName = this.docList[this.pageIndex].name;             
      })
    );
  }

  onPageIndexChange(pageIndex: number) {    
    this.router.navigate([`../../${this.docList[pageIndex].id}/tokenization`], {
      relativeTo: this.route
    });
  }


  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }

}
