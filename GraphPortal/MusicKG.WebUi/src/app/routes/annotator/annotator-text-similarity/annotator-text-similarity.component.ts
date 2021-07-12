import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard';
import { TaskService } from 'src/app/services/task.service';
import { Router, ActivatedRoute } from '@angular/router';
import { combineLatest, forkJoin, of, Subject } from 'rxjs';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { TestSimilarityItem, TestSimilarityDocContent } from 'src/app/interfaces/task.interface';
import { switchMap, takeUntil, tap, filter } from 'rxjs/operators';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { ajv } from 'src/app/app.component';
import { RuleService } from 'src/app/services/rule.service';

@Component({
  selector: 'km-annotator-text-similarity',
  templateUrl: './annotator-text-similarity.component.html',
  styleUrls: ['./annotator-text-similarity.component.less']
})
export class AnnotatorTextSimilarityComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {
  private unsubscribe = new Subject<void>();
  docContent: TestSimilarityDocContent;
  itemTotal = 0;
  itemPage = 0;
  currentItemContent: TestSimilarityItem;
  workspaceId: string;
  taskId: string;
  docId: string;
  taskName: string;
  docList: any[];
  index: number;
  docName: string;
  userInfo: UserInfo;
  oldResult: string;
  taskDocumentStatus: string;
  workspaceType: string;
  contentValidate = true;
  dictionaryItemList = [];
  addItem = {
    Result: 1,
    Text: null
  };

  constructor(
    private docService: PreMarkService,
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private localStorage: LocalStorageService,
    private msg: NzMessageService,
    private modalService: NzModalService,
    private ruleService: RuleService
  ) { }

  get notHasPagePrevious() {
    return this.index == 0;
  }

  get notHasPageNext() {
    return this.docList && this.index == (this.docList.length - 1);
  }

  get notHasItemPrevious() {
    return this.itemPage == 0;
  }

  get notHasItemNext() {
    return this.itemPage == (this.itemTotal - 1);
  }

  ngOnInit() {
    this.userInfo = this.localStorage.get('user');
    combineLatest(
      this.route.pathFromRoot[3].params,
      this.route.queryParams
    ).pipe(
      tap(([t, q]) => {
        this.workspaceId = q.workspaceId;
        this.taskId = t.task;
        this.docId = t.doc;
        this.taskName = q.name;
        this.workspaceType = q.type;
      }),
      switchMap(() => this.getDocList()),
      takeUntil(this.unsubscribe)
    ).subscribe(() => {
      this.getDictionaryContent();
      this.initContent(this.docId);
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  initContent(docId: string) {
    this.currentItemContent = null;
    this.getDocContent(docId);
    this.itemPage = 0;
    if (this.docList && this.docList.length > 0) {
      this.docName = this.docList[this.index].name;
    }
  }

  jumperItem(itemIndex: number) {
    if (itemIndex >= 0 && itemIndex < this.itemTotal) {
      this.itemPage = itemIndex;
      this.currentItemContent = this.docContent.Items[this.itemPage];
    }
  }

  pagePrevious() {
    if (this.index > 0) {
      this.index--;
      this.docId = this.docList[this.index].id;
      this.goToDocPage();
    }
  }

  pageNext() {
    if (this.index < (this.docList.length - 1)) {
      this.index++;
      this.docId = this.docList[this.index].id;
      this.goToDocPage();
    }
  }

  goToDocPage() {
    this.router.navigate([`../../${this.docId}/textSimilarity`], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      },
    });
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

  getDocList() {
    return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
      tap(res => {
        this.docList = res.items;
        this.index = this.docList.map((d: any) => d.id).indexOf(this.docId);
        this.docName = this.docList[this.index].name;
      })
    );
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

  canDeactivate() {
    return !this.contentValidate || this.taskDocumentStatus == 'Annotated' || !this.oldResult || (this.oldResult && this.oldResult == JSON.stringify(this.parseResult()));
  }

  getDocContent(docId: string) {
    forkJoin(
      this.getResult(docId),
      this.docService.downloadDocument(this.workspaceId, docId)
    ).subscribe(([result, content]) => {
      if (ajv.validate('textSimilarity', content)) {
        this.contentValidate = true;
        this.oldResult = result && JSON.stringify(result);
        content.Items = content.Items.map((item: TestSimilarityItem, index: number) => {
          let userResult: number[];
          let textTargets = [...item.TextTargets];
          if (result) {
            userResult = result.Results[index].LabelerResults.find(r => r.LabelerId == this.userInfo.name).LabelResults;
            item.TextTargets = result.Results[index].Item.TextTargets;
          }
          item.Labels = item.TextTargets.map((text: string, index: number) => {
            return { Text: text, Result: userResult ? userResult[index] : 0, IsAdded: !textTargets.find(t => t == text) };
          });
          return item;
        });
        this.docContent = content;
        this.itemTotal = this.docContent.Items.length;
        if (this.docContent.Items && this.docContent.Items.length > 0) {
          this.currentItemContent = this.docContent.Items[0];
        }
        this.taskDocumentStatus = this.docList[this.index].status;
      } else {
        this.msg.warning('不支持该文档格式');
        this.itemTotal = 1;
        this.contentValidate = false;
        this.taskDocumentStatus = 'Annotated';
      }
    });
  }

  getDictionaryContent() {
    if (this.dictionaryItemList && this.dictionaryItemList.length > 0) { return; }
    this.taskService.get(this.workspaceId, this.taskId).pipe(
      filter(res => res.dictionaryIds && res.dictionaryIds.length > 0),
      switchMap(res => this.ruleService.getContent(this.workspaceId, res.dictionaryIds[0]))
    ).subscribe((res: any) => {
      this.dictionaryItemList = res.items;
    });
  }

  get isAnnotation() {
    return this.docContent.Items.every(item => {
      return item.Labels.every(l => l.Result == 0 || l.Result == 1);
    });
  }

  save(status: string = 'Assigned') {
    if (!this.isAnnotation) {
      this.msg.warning('存在未标注数据');
    } else {
      if (status == 'Assigned') {
        this.jumperItem(this.itemPage + 1);
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
      Results: this.docContent.Items.map(item => {
        return {
          Item: { Id: item.Id, TextSource: item.TextSource, TextTargets: item.TextTargets },
          LabelerResults: [
            { LabelerId: this.userInfo.name, LabelResults: item.Labels.map(l => l.Result), LabelerRole: "Annotator" }
          ]
        }
      })
    };
  }

  itemPrevious() {
    if (this.itemPage > 0) {
      this.itemPage--;
      this.currentItemContent = this.docContent.Items[this.itemPage];
    }
  }

  itemNext() {
    if (this.itemPage < (this.itemTotal - 1)) {
      this.itemPage++;
      this.currentItemContent = this.docContent.Items[this.itemPage];
    }
  }

  onAddItem() {
    if (this.addItem.Text && !this.currentItemContent.TextTargets.find(l => l == this.addItem.Text)) {
      this.currentItemContent.Labels.push({ ...this.addItem, IsAdded: true });
      this.currentItemContent.TextTargets.push(this.addItem.Text);
      this.addItem = {
        Result: 1,
        Text: null
      }
    } else {
      this.msg.warning('相似文本不能为空且不能重复');
    }
  }

  onRemoveItem(item: any) {
    this.currentItemContent.Labels = this.currentItemContent.Labels.filter(l => l !== item);
    this.currentItemContent.TextTargets = this.currentItemContent.TextTargets.filter(t => t !== item.Text);
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }

}
