import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { tap, switchMap, takeUntil, finalize, switchMapTo } from 'rxjs/operators';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { TaskService } from 'src/app/services/task.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { combineLatest, Subject, forkJoin, of } from 'rxjs';
import { RuleService } from 'src/app/services/rule.service';
import { DictionaryService } from 'src/app/services/dictionary.service';
import { saveAs } from 'file-saver';
import { BatchModalComponent } from '../batch-modal/batch-modal.component';

declare const window: any;

@Component({
  selector: 'km-annotator-classification',
  templateUrl: './annotator-classification.component.html',
  styleUrls: ['./annotator-classification.component.less']
})
export class AnnotatorClassificationComponent implements OnInit {
  private unsubscribe = new Subject<void>();
  userInfo: any;
  workspaceId: string;
  taskId: string;
  docId: string;
  taskName: string;
  workspaceType: string;
  taskDocumentStatus: string;
  thList = [];
  itemList = [];
  isLoading = false;
  actionLoading = false;
  previousDocInfo: any;

  docInfoVisible = false;
  activeDocInfo: any;

  showSetting = false;
  confidenceValue: number = 0;
  confidenceSyndromeValue: number = 0;

  pageSize = 10;

  pageIndex = 1;

  partNameList = [];
  reasonList = [];

  contentClone: any;

  dataSource: string;

  dataClone = [];

  tableXY = {
    x: '1100px',
    y: '0px'
  };

  indeterminate = false;

  checked = false;

  setOfCheckedId = new Set<string>();

  constructor(
    private docService: PreMarkService,
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private localStorage: LocalStorageService,
    private msg: NzMessageService,
    private modalService: NzModalService,
    private dictionaryService: DictionaryService
  ) { }

  ngOnInit() {
    this.userInfo = this.localStorage.get('user');
    this.isLoading = true;
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
      switchMap(() => this.getResult(this.docId)),
      switchMap(result => {
        if (result) {
          return of(result);
        } else {
          return this.docService.downloadDocument(this.workspaceId, this.docId)
        }
      }),
      takeUntil(this.unsubscribe)
    ).subscribe(docInfo => {
      this.dataSource = docInfo.DataSource;
      this.setData(docInfo);
      this.isLoading = false;
      // this.getOptions(docInfo);
    });
  }

  sort(event: any) {
    this.pageIndex = 1;
    if (event.value) {
      this.itemList = this.itemList.sort((a, b) => {
        if (a.result[event.key].Value && b.result[event.key].Value) {
          const judgment = a.result[event.key].Value.localeCompare(b.result[event.key].Value, 'zh', { sensitivity: 'accent' });
          if (event.value == 'descend') {
            return judgment * -1;
          }
          if (event.value == 'ascend') {
            return judgment;
          }
        } else {
          if (event.value == 'descend') {
            return a.result[event.key].Value ? 1 : -1;
          }
          if (event.value == 'ascend') {
            return a.result[event.key].Value ? -1 : 1;
          }
        }
        return 0;
      });
    } else {
      this.itemList = [].concat(this.dataClone)
    }
    this.itemList = [].concat(this.itemList)
  }

  setData(docInfo: any) {
    this.previousDocInfo = JSON.stringify(docInfo);
    this.thList = Object.keys(docInfo.Items[0].Features).map((key, index) => {
      return {
        name: key,
        isShow: docInfo.Items[0].Features[key].KeyFeature
      }
    }).filter(item => item.isShow);
    this.itemList = docInfo.Items.map(item => {
      if (!item.ModelResult) {
        item.ModelResult = {
          PartName: { Value: null, Probability: null },
          Syndrome: { Value: null, Probability: null }
        };
      }
      if (!item.Result) {
        item.Result = {
          PartName: { Value: null, Probability: null },
          Syndrome: { Value: null, Probability: null }
        };
      }
      let data = {
        id: item.Id,
        values: Object.keys(item.Features).map(key => ({
          value: item.Features[key].Value,
          isShow: item.Features[key].KeyFeature
        })).filter(item => item.isShow),
        originalResult: {
          partName: item.OriginalResult.PartName,
          syndrome: item.OriginalResult.Syndrome,
        },
        result: {
          partName: (item.Result && item.Result.PartName) || item.ModelResult.PartName || null,
          syndrome: (item.Result && item.Result.Syndrome) || item.ModelResult.Syndrome || null,
          isAddForTraining: item.Result && !!item.Result.IsAddForTraining
        },
        modelResult: {
          partName: { ...item.ModelResult.PartName },
          syndrome: {
            ...item.ModelResult.Syndrome,
            Value: item.ModelResult.Syndrome.Value
          },
        }
      }
      if (docInfo.DataSource == 'MQI') {
        data.result.partName = {
          ...item.OriginalResult.PartName
        }
      }
      return data;
    });
    this.sortList();
  }

  sortList() {
    this.itemList = this.itemList.sort((a, b) => {
      if (!a.result.partName.Value) {
        return -1;
      }
      if (!b.result.partName.Value) {
        return 1
      }
      if (!a.originalResult.partName || !a.originalResult.syndrome) {
        return 1;
      }
      const judgment = a.originalResult.partName.Value.localeCompare(b.originalResult.partName, 'zh-Hans-CN', { sensitivity: 'accent' });
      if (judgment === 0) {
        return a.originalResult.syndrome.Value.localeCompare(b.originalResult.syndrome, 'zh-Hans-CN', { sensitivity: 'accent' });
      }
      return judgment;
    });
    this.dataClone = [].concat(this.itemList);
  }

  getOptions(docInfo: any) {
    forkJoin(
      this.dictionaryService.getPartName(this.workspaceId),
      this.dictionaryService.getReasonAll(this.workspaceId)
    ).subscribe(([partName, reason]) => {
      this.partNameList = partName.entries.items;
      this.reasonList = reason || [];
      this.setData(docInfo);
      this.isLoading = false;
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  getResult(docId: string) {
    return this.taskService.getAnnotationResult(this.workspaceId, this.taskId, docId).pipe(
      switchMap((res: any) => {
        if (res && res.length > 0) {
          let doc = res.find(r => r.annotatedBy.id == this.userInfo.id && r.resultType === 'ForGraphMerging');
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

  onExport() {
    this.actionLoading = true;
    this.getResult(this.docId).pipe(
      switchMap(result => {
        if (result) {
          return of(result);
        } else {
          return this.docService.downloadDocument(this.workspaceId, this.docId)
        }
      })
    ).subscribe(result => {
      let blob = new Blob([JSON.stringify(result)], { type: "text/plain;charset=utf-8" });
      let name = this.taskName + '(Output).json'
      saveAs(blob, `${name}`);
      this.actionLoading = false;
    })
  }

  onSave(status: string = 'Assigned') {
    if (status == 'Annotated') {
      this.onSubmit(status);
      return;
    }
    const allResult = this.parseResult();
    this.actionLoading = true;
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(allResult),
      resultType: "ForGraphMerging",
      status: status
    }).subscribe(res => {
      this.msg.success('操作成功');
      this.previousDocInfo = JSON.stringify(allResult);
      this.taskDocumentStatus = status;
      this.actionLoading = false;
      this.setOfCheckedId.clear();
      this.refreshCheckedStatus();
    });
  }

  alertError(index: number) {
    const tIndex = Math.ceil((index + 1) / this.pageSize);
    this.msg.warning(`第${tIndex}页序号${index + 1}存在未标注项`);
  }

  onSubmit(status: string) {
    const allResult = this.parseResult();
    const tranResult = JSON.parse(JSON.stringify(allResult));
    tranResult.Items = tranResult.Items.filter(d => d.Result.IsAddForTraining);
    for (let index = 0; index < allResult.Items.length; index++) {
      if (allResult.DataSource == 'MQI') {
        if (!allResult.Items[index].Result.Syndrome.Value) {
          this.alertError(index)
          return;
        }
      } else {
        if (!allResult.Items[index].Result.PartName.Value || !allResult.Items[index].Result.Syndrome.Value) {
          this.alertError(index)
          return;
        }
      }
    }
    this.actionLoading = true;
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(tranResult),
      resultType: "ForModelTraining",
      status: 'Assigned'
    }).pipe(
      switchMap(res => {
        return this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
          result: JSON.stringify(allResult),
          resultType: "ForGraphMerging",
          status: status
        });
      })
    ).subscribe(res => {
      this.msg.success('操作成功');
      this.previousDocInfo = JSON.stringify(allResult);
      this.taskDocumentStatus = status;
      this.actionLoading = false;
      this.goTo();
    });
  }

  parseResult() {
    const saveResult = JSON.parse(this.previousDocInfo);
    for (let index = 0; index < saveResult.Items.length; index++) {
      const labelItem = this.itemList.find(item => saveResult.Items[index].Id == item.id).result;
      saveResult.Items[index].Result = {
        PartName: {
          Value: labelItem.partName.Value,
          Probability: labelItem.partName.Probability
        },
        Syndrome: {
          Value: labelItem.syndrome.Value,
          Probability: labelItem.syndrome.Probability
        },
        IsAddForTraining: !!labelItem.isAddForTraining
      }
    }
    return saveResult;
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || !this.previousDocInfo || this.previousDocInfo == JSON.stringify(this.parseResult());
  }

  goTo() {
    this.router.navigateByUrl('/annotator/task?snapshot=true');
  }

  openInfo(item: any) {
    this.docInfoVisible = true;
    this.activeDocInfo = JSON.parse(this.previousDocInfo).Items.find(i => i.Id == item.id).Features;
  }

  close() {
    this.docInfoVisible = false;
    this.showSetting = false;
  }

  openSetting() {
    this.showSetting = true;
  }

  settingValue() {
    this.itemList = this.itemList.map(item => {
      const partNameValue = {
        ...item.modelResult.partName
      };
      const syndromeValue = {
        ...item.modelResult.syndrome
      };
      if (Math.round(item.modelResult.partName.Probability * 100) / 100 < this.confidenceValue) {
        partNameValue.Value = null;
      }
      if (Math.round(item.modelResult.syndrome.Probability * 100) / 100 < this.confidenceSyndromeValue) {
        syndromeValue.Value = null;
        syndromeValue.Id = null;
      }
      item.result = {
        partName: partNameValue,
        syndrome: syndromeValue,
        isAddForTraining: item.result.isAddForTraining
      }
      return item;
    });
    // this.sortList();
    this.showSetting = false;
  }

  labelChange(item: any, newValue: string) {
    if (this.dataSource == 'MQI') {
      item.result.isAddForTraining = (item.result.syndrome.Value !== item.modelResult.syndrome.Value);
    } else {
      item.result.isAddForTraining = (item.result.syndrome.Value !== item.modelResult.syndrome.Value) || (newValue !== item.modelResult.partName.Value)
    }
  }

  labelSyndromeChange(item: any, newValue: string) {
    if (this.dataSource == 'MQI') {
      item.result.isAddForTraining = (newValue !== item.modelResult.syndrome.Value);
    } else {
      item.result.isAddForTraining = (item.result.syndrome.Value !== item.modelResult.syndrome.Value) || (item.result.partName.Value !== item.modelResult.partName.Value)
    }
  }

  batchUpdate() {
    if (this.setOfCheckedId && this.setOfCheckedId.size > 0) {
      this.modalService.create({
        nzTitle: '批量标注',
        nzComponentParams: {
          dataSource: this.dataSource
        },
        nzContent: BatchModalComponent,
        nzOnOk: (component: BatchModalComponent) => {
          this.itemList.filter(item => this.setOfCheckedId.has(item.id)).forEach(item => {
            if (this.dataSource !== 'MQI') {
              this.labelChange(item, component.partName)
              item.result.partName.Value = component.partName;
            }
            this.labelSyndromeChange(item, component.syndrome);
            item.result.syndrome.Value = component.syndrome;
          });
          this.setOfCheckedId.clear();
          this.refreshCheckedStatus();
        }
      });
    }
  }

  onItemChecked(id: string, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.refreshCheckedStatus();
  }

  onAllChecked(value: boolean): void {
    const filterData = this.itemList.slice((this.pageIndex - 1) * this.pageSize, this.pageIndex * this.pageSize).forEach(item => this.updateCheckedSet(item.id, value));
    this.refreshCheckedStatus();
  }

  refreshCheckedStatus(): void {
    const filterData = this.itemList.slice((this.pageIndex - 1) * this.pageSize, this.pageIndex * this.pageSize);
    this.checked = this.setOfCheckedId.size > 0 && filterData.length > 0 && filterData.every(item => this.setOfCheckedId.has(item.id));
    this.indeterminate = filterData.some(item => this.setOfCheckedId.has(item.id)) && !this.checked;
  }

  updateCheckedSet(id: string, checked: boolean): void {
    if (checked) {
      this.setOfCheckedId.add(id);
    } else {
      this.setOfCheckedId.delete(id);
    }
  }

}
