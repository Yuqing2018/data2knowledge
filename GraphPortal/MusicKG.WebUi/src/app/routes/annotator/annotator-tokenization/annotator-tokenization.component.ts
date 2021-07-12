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
  selector: 'km-annotator-tokenization',
  templateUrl: './annotator-tokenization.component.html',
  styleUrls: ['./annotator-tokenization.component.less']
})
export class AnnotatorTokenizationComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {
  private unsubscribe = new Subject<void>();
  workspaceId: string;
  taskId: string;
  docId: string;
  taskName: string;
  workspaceType: string;
  annotationData: TokenizationModel;
  pageIndex: number = 2;
  taskDocumentStatus: string;
  docList: any[];
  docName: string;
  docContent: any;
  oldResult: string;
  docContentSource: any;
  userInfo: UserInfo;
  isLoading = false;

  get saveBtnDisabled() {
    return this.taskDocumentStatus !== 'Assigned';
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
      }),
      switchMap(() => this.getDocList())
    ).subscribe(() => {
      this.isLoading = true;
      this.getDocContent(this.docId);
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || !this.oldResult || (this.oldResult && this.oldResult == JSON.stringify(this.parseResult()));
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
      this.taskDocumentStatus = status;
    });
  }

  parseResult() {
    return {
      Results: this.docContent.Items.map((item, index) => {
        return {
          Item: { Id: item.Id, Text: item.Text, SpanItems: this.docContentSource.Items[index].SpanItems },
          LabelerResults: [
            { LabelerId: this.userInfo.name, LabelResults: item.SpanItems.map(i => {
              return {
                Start: i.Start,
                End: i.End
              }
            }), LabelerRole: "Annotator" }
          ]
        }
      })
    };
  }

  getDocContent(docId: string) {
    forkJoin(
      this.getResult(docId),
      this.docService.downloadDocument(this.workspaceId, docId)
    ).subscribe(([result, content]) => {
      if(ajv.validate('tokenization',content)) {
        this.oldResult = result && JSON.stringify(result);  
        this.docContentSource = JSON.parse(JSON.stringify(content));
        content.Items = content.Items.map((item: any, index: number) => {
          let userResult: any[];
          if (result) {
            userResult = result.Results[index].LabelerResults.find(r => r.LabelerId == this.userInfo.name).LabelResults;
          }
          item.SpanItems = userResult || item.SpanItems;
          return item;
        });
        this.docContent = content;    
        this.taskDocumentStatus = this.docList[this.pageIndex].status;
        this.docName = this.docList[this.pageIndex].name;
      }else { 
        this.msg.warning('不支持该文档格式');
      }
      this.isLoading = false;
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

  getDocList() {
    return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
      tap(res => {
        this.docList = res.items;
        this.pageIndex = this.docList.map((d: any) => d.id).indexOf(this.docId);
        this.docName = this.docList[this.pageIndex].name;             
      })
    );
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

  onPageIndexChange(pageIndex: number) {
    this.router.navigate([`../../${this.docList[pageIndex].id}/tokenization`], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      },
    });
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }
  
}
