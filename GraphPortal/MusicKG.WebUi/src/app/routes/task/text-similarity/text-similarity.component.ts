import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { TaskService } from 'src/app/services/task.service';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest, forkJoin, of, Subject } from 'rxjs';
import { switchMap, tap, takeUntil, take, filter } from 'rxjs/operators';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { TestSimilarityItem, TestSimilarityDocContent, TaskInfo } from 'src/app/interfaces/task.interface';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard';
import { ajv } from 'src/app/app.component';
import { RuleService } from 'src/app/services/rule.service';
import { Label } from 'src/app/shared/annotation/interface';

@Component({
  selector: 'km-text-similarity',
  templateUrl: './text-similarity.component.html',
  styleUrls: ['./text-similarity.component.less']
})
export class TextSimilarityComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {
  private unsubscribe = new Subject<void>();
  docContent: TestSimilarityDocContent;
  itemTotal = 0;
  itemPage = 0;
  currentItemContent: TestSimilarityItem;
  workspaceId: string;
  taskId: string;
  docId: string
  taskName: string;
  docList: any[];
  index: number;
  docName: string;
  userInfo: UserInfo;
  oldResult: string;
  annotatorList: any[] = [];
  taskDocumentStatus: string;
  hasReviewPermission = false;
  taskInfo: TaskInfo;
  docContentSource: any;
  dictionaryItemList = [];
  addItem = {
    Result: 1,
    Text: null
  };

  annotationReuslt: any;

  constructor(
    private taskService: TaskService,
    private preService: PreMarkService,
    private route: ActivatedRoute,
    private router: Router,
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

  get isAnnotation() {
    return this.docContent.Items.every(item => {
      return item.Labels.every(l => l.Result == 0 || l.Result == 1);
    });
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || (this.oldResult && this.oldResult == JSON.stringify(this.parseResult())) || !this.oldResult;
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
      relativeTo: this.route
    });
  }

  save(status: string = 'Assigned') {
    if (!this.isAnnotation) {
      this.msg.warning('存在未标注数据');
      return;
    }
    if (status == 'Assigned') {
      this._saveResult(status, true);
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

  private _saveResult(status: string, jumper = false) {
    let result = this.parseResult();
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: "ForModelTraining",
      status: status
    }).subscribe(res => {
      this.msg.success('操作成功');
      jumper && this.jumperItem(this.itemPage + 1);     
      this.currentItemContent = null;    
      this.initDoc(this.docId);
      this.taskDocumentStatus = status;
    }, err => {
      this.msg.create(err.type, err.msg);
    });
  }

  parseResult() {
    return {
      Results: this.docContent.Items.map(item => {
        return {
          Item: { Id: item.Id, TextSource: item.TextSource, TextTargets: item.TextTargets },
          LabelerResults: [
            { LabelerId: this.userInfo.name, LabelResults: item.Labels.map(l => l.Result), LabelerRole: "Manager" }
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

  selectedAnnotator() {
    let selectedUser = this.annotatorList.filter(l => l.checked).map(d => d.value);
    this.docContent.Items = this.docContent.Items.map((item: TestSimilarityItem, index: number) => {
      if (selectedUser && selectedUser.length > 0) {
        let textTargetSet = new Set();
        let labels = [];
        let userResult = this.annotationReuslt.filter(d => {
          return selectedUser.find(u => u == d.Results[index].LabelerResults[0].LabelerId);
        }).map(d => {
          return d.Results[index];
        });
        userResult.forEach(u => {
          u.Item.TextTargets.forEach(t => {
            textTargetSet.add(t);
          })
        });
        textTargetSet.forEach(l => {
          let hasLabel = userResult.every(u => {
            return u.Item.TextTargets.find(t => t == l);
          });
          let userSeleteds = userResult.map(d => {
            let indexLabel = d.Item.TextTargets.indexOf(l);
            return indexLabel !== -1 ? (d.LabelerResults[0].LabelResults[indexLabel]) : 0;
          });
          let trueNum = userSeleteds.filter(d => d == 1).length;
          let falseNum = userSeleteds.filter(d => d == 0).length;
          labels.push({
            Text: l,
            Result: trueNum > falseNum ? 1 : 0,
            Conflict: (trueNum > 0) && (falseNum > 0),
            TextConflict: !hasLabel,
            IsAdded: !this.docContentSource.Items[index].TextTargets.find(t => t == l)
          });
        });
        item.Labels = labels;
        item.TextTargets = Array.from(textTargetSet);
      } else {
        let docContent = JSON.parse(JSON.stringify(this.docContentSource));
        item.TextTargets = docContent.Items[index].TextTargets;
        item.Labels = docContent.Items[index].Labels;
      }
      return item;
    });
  }


  ngOnInit() {
    this.userInfo = this.localStorage.get('user');
    this.getWorkSpaceAndTaskDocIds().pipe(
      takeUntil(this.unsubscribe),
      tap(([w, t]) => {
        this.workspaceId = w.workspace;
        this.taskId = t.task;
        this.docId = t.doc;
      }),
      switchMap(() => this.getDocList())
    ).subscribe(() => {
      this.getDictionaryContent();
      this.currentItemContent = null;
      this.itemPage = 0;
      this.initDoc(this.docId);
    });
  }

  getDictionaryContent() {
    if (this.dictionaryItemList && this.dictionaryItemList.length > 0) { return; }
    this.taskService.get(this.workspaceId, this.taskId).pipe(
      filter(res => res.dictionaryIds && res.dictionaryIds.length > 0),
      takeUntil(this.unsubscribe),
      switchMap(res => this.ruleService.getContent(this.workspaceId, res.dictionaryIds[0]))
    ).subscribe((res: any) => {
      this.dictionaryItemList = res.items;
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
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
      this.preService.downloadDocument(this.workspaceId, this.docId),
      this.taskService.get(this.workspaceId, this.taskId)
    ).subscribe(([res, doc, task]) => {
      this.taskInfo = task;
      if (ajv.validate('textSimilarity', doc)) {
        doc.Items = doc.Items.map((item: TestSimilarityItem, index: number) => {
          item.Labels = item.TextTargets.map((text: string, index: number) => {
            return { Text: text, Result: 0 };
          });
          return item;
        });
        this.docContentSource = JSON.parse(JSON.stringify(doc));
        this.docContent = doc;
        this.hasReviewPermission = this.docList[this.index].annotators.find(user => user.id == this.userInfo.id);
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
        this.itemTotal = this.docContent.Items.length;
        if (this.docContent.Items && this.docContent.Items.length > 0) {
          this.jumperItem(this.itemPage);
        }
      } else {
        this.msg.warning('不支持该文档格式');
        this.itemTotal = 1;
        this.hasReviewPermission = false;
      }
    });
  }

  getResultList(result: any): any {
    if (result && result.length > 0) {
      return forkJoin(
        result.map(r => this.preService.downloadDocument(this.workspaceId, r.resultDocumentId))
      );
    } else {
      return of(false);
    }
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

  getWorkSpaceAndTaskDocIds() {
    return combineLatest(
      this.route.pathFromRoot[1].params,
      this.route.pathFromRoot[3].params
    );
  }

  jumperItem(itemIndex: number) {
    if (itemIndex >= 0 && itemIndex < this.itemTotal) {
      this.itemPage = itemIndex;
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
