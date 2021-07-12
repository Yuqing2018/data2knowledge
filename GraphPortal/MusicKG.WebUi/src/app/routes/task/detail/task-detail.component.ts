import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest, forkJoin, of, Subject } from 'rxjs';
import { switchMap, tap, takeUntil } from 'rxjs/operators';
import { TaskService } from 'src/app/services/task.service';
import { TaskInfo, DocumentResult, TaskDocument } from 'src/app/interfaces/task.interface';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { saveAs } from 'file-saver';
import { DocumentData } from 'src/app/interfaces/pre-mark.interface';
import { WorkspaceService } from 'src/app/services/workspace.service';
import { WOKSPACR_TYPE_LIST, WORKSPCE_LIST_CSV, TASK_TYPE } from 'src/app/core/common';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { NzMessageService } from 'ng-zorro-antd';
import { OntologyDialogueService } from 'src/app/services/ontology-dialogue.service';

@Component({
  selector: 'km-task-detail',
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.less']
})
export class TaskDetailComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject<void>();
  taskInfo: TaskInfo;
  workspaceType: string;
  docList: TaskDocument[];
  workspaceId: string;
  workspaceInfo: any
  isLoading: boolean = false

  allChecked: boolean = false
  isIndeterminate: boolean = false
  total: number = 0
  selectedItems: number = 0
  isGraph: boolean = false

  get exportBtnDisabled() {
    return this.docList && this.docList.filter(val => val.checked == true).length == 0
  }

  constructor(
    private route: ActivatedRoute,
    private api: TaskService,
    private router: Router,
    private workspaceService: WorkspaceService,
    private dialogService: OntologyDialogueService,
    private preService: PreMarkService,
    private msg: NzMessageService
  ) { }

  getAnnotators(annotators: UserInfo[]) {
    return annotators && annotators.filter(a => a.roles && a.roles.indexOf('Annotator') !== -1).map(u => u.name).join(',');
  }

  ngOnInit() {
    this.getWorkspaceType();
    this.getWorkspaceIdAndTaskId().pipe(
      takeUntil(this.unsubscribe),
      switchMap(([w, t]) => this.getTaskInfo(w.workspace, t.task))
    ).subscribe(([taskInfo, docList]) => {
      this.taskInfo = taskInfo;
      this.docList = docList.items;
      this.total = this.docList.length
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  getTaskInfo(workspaceId: string, taskId: string) {
    return forkJoin(
      this.api.get(workspaceId, taskId),
      this.api.getTaskDoc(workspaceId, taskId)
    );
  }

  getWorkspaceIdAndTaskId() {
    return combineLatest(
      this.route.pathFromRoot[1].params,
      this.route.params
    );
  }

  getWorkspaceType() {
    this.route.pathFromRoot[1].params.pipe(
      takeUntil(this.unsubscribe),
      tap(w => { this.workspaceId = w.workspace }),
      switchMap(w => this.workspaceService.info(w.workspace))
    ).subscribe(res => {
      this.workspaceType = res.type.id;
      this.workspaceInfo = WORKSPCE_LIST_CSV.filter(val => val.value === this.workspaceType)
      this.isGraph = this.workspaceType === WOKSPACR_TYPE_LIST[7].value
    });
  }

  goTo(doc: DocumentData) {
    let type = WOKSPACR_TYPE_LIST.find(w => w.value == (this.taskInfo.taskType || this.workspaceType));
    this.router.navigate([`doc/${doc.id}/${type.annotation}`], { relativeTo: this.route });
  }

  download(doc: DocumentData, type: string = 'json') {
    this.isLoading = true
    if (this.workspaceType === WOKSPACR_TYPE_LIST[7].value) {
      this.downloadGraph(doc)
      return
    }
    if (this.taskInfo.taskType == TASK_TYPE.INTENT || this.taskInfo.taskType == TASK_TYPE.ENTITY) {
      this.downloadDialog(doc);
      return;
    }
    this.api.getAnnotationResult(this.workspaceId, this.taskInfo.id, doc.id).pipe(
      switchMap(res => this.getResultList(res))
    ).subscribe((res: any) => {
      if (res && res.length > 0) {
        if (type === 'csv') {
          let content = this.exportCsv(res)
          var blob = new Blob([content], { type: "text/csv" });
          let name = doc.name.split('.')[0] + '(Output).csv'
          saveAs(blob, `${name}`);
        } else {
          this.exportJson(res, doc)
        }
      } else {
        this.msg.warning('暂无无标注结果');
      }
      this.isLoading = false
    })
  }

  downloadGraph(doc) {
    this.api.getAnnotationResult(this.workspaceId, this.taskInfo.id, doc.id).pipe(
      switchMap((res: any) => {
        let manager = res.find((r: any) => r.annotatedBy.roles.indexOf('Manager') !== -1);
        if (manager) {
          return this.getResultList([manager])
        }
        return of(false);
      })
    ).subscribe((res: any) => {
      if (res && res.length > 0) {
        let result = res.map(val => {
          return {
            id: val.id,
            name: val.name,
            tags: val.tags.map(el => el.name)
          }
        })
        var blob = new Blob([JSON.stringify(result[0])], { type: "text/plain;charset=utf-8" });
        let name = doc.name.split('.')[0] + '(Output).json'
        saveAs(blob, `${name}`);
      } else {
        this.msg.warning('暂无无标注结果');
      }
      this.isLoading = false
    })
  }

  downloadDialog(doc: DocumentData) {
    this.api.getAnnotationResult(this.workspaceId, this.taskInfo.id, doc.id).pipe(
      switchMap((res: any) => {
        let manager = res.find((r: any) => r.annotatedBy.roles.indexOf('Manager') !== -1);
        if (manager) {
          return this.getResultList([manager])
        }
        return of(false);
      })
    ).subscribe((res: any) => {
      if (res && res.length > 0) {
        var blob = new Blob([JSON.stringify(res[0])], { type: "text/plain;charset=utf-8" });
        let name = doc.name.split('.')[0] + '(Output).json'
        if (this.taskInfo.taskType == TASK_TYPE.INTENT) {
          this.downloadIntentEntity(res[0]);
        }
        saveAs(blob, `${name}`);

      } else {
        this.msg.warning('暂无无标注结果');
      }
      this.isLoading = false
    })
  }

  downloadIntentEntity(intent: any) {
    let parts = [];
    for (let intentItem of intent) {
      for (let tra of intentItem.TrainingPhrases) {
        if (tra.TrainingPhraseType == 'Example') {
          for (let part of tra.Parts) {
            parts.push(part);
          }
        }
      }
    }
    this.dialogService.getOntology(this.workspaceId).pipe(
      switchMap((res: any) => {
        if (res.entityDocumentId) {
          return this.preService.downloadDocument(this.workspaceId, res.entityDocumentId);
        }
        return of(false);
      })
    ).subscribe(res => {
      let entity = res.map(entity => {
        let entityPart = parts.filter(p => p.EntityType == entity.EntityType).map(d => d.Text);
        let unique = (textArr: any) => {
          return Array.from(new Set(textArr));
        };
        entityPart = unique(entityPart);
        if (entityPart && entityPart.length > 0) {
          entity.Entries = {
            Value: [entityPart[0]],
            Synonyms: entityPart.splice(1, entityPart.length)
          };
        } else {
          entity.Entries = [];
        }
        return entity;
      })
      var blob = new Blob([JSON.stringify(entity)], { type: "text/plain;charset=utf-8" });
      let name = 'entityFile(Output).json'
      saveAs(blob, `${name}`);
      console.log(entity);
    });
  }

  exportCsv(res: any) {
    let result: string
    switch (this.workspaceType) {
      case '5c414bd75a01cf00010f9660':
        result = this.exportTextSimilaritySCV(res)
        break;
      case '5c414c0a5a01cf00010f9661':
        let downContent = {
          Results: res[0].Results.map((item: any, index: number) => {
            item.LabelerResults = res.map(r => {
              return r.Results[index].LabelerResults[0];
            });
            return item;
          })
        };
        result = this.handleNERCSV(downContent)
        break;

      default:
        break;
    }
    return result
  }

  exportTextSimilaritySCV(data: any) {
    const header = this.getTextSimilarityHeader(data)
    const content = this.getTextSimilarityContent(data)
    content.unshift(header)
    let dataType = '\uFEFF'
    let i = content.map(val => {
      return dataType + val.join(',')
    })
    return i.join('\r\n')
  }

  getTextSimilarityContent(data: any) {
    let textArr = data[0].Results.map(val => val.Item.TextSource)
    let targetArr = []
    textArr.map((val, index) => {
      targetArr[index] = this.getTextSimilarityTargets(val, index, data)
    })
    return this.newGetTextSimilarityContent(textArr, targetArr, data)
  }

  newGetTextSimilarityContent(text: any, target: any, data: any) {
    let contet = []
    text.map((val, index) => {
      target[index].map(item => {
        let result = [val, item, ...this.getTextSimilarityItem(val, item, data)]
        contet.push(result)
      })
    })
    return contet
  }

  getTextSimilarityItem(source: any, target: any, data: any) {
    let result = []
    data.map(val => {
      let i = val.Results.filter(item => item.Item.TextSource === source && item.Item.TextTargets.indexOf(target) > -1)
      if (i.length === 0) {
        result.push('')
      } else {
        result.push(i[0].LabelerResults[0].LabelResults[i[0].Item.TextTargets.indexOf(target)])
      }
    })
    return result
  }

  getTextSimilarityTargets(val: any, index: any, data: any) {
    let result = []
    data.map(val => {
      result.push(...val.Results[index].Item.TextTargets)
    })
    return Array.from(new Set(result))
  }

  getTextSimilarityHeader(data: any) {
    let labeler = data.map(val => {
      let labelerInfo = val.Results[0].LabelerResults[0]
      return labelerInfo.LabelerRole === 'Annotator' ? `${labelerInfo.LabelerId}(A)` : `${labelerInfo.LabelerId}(M)`
    })
    return ['TextSource', 'TextTargets', ...labeler]
  }

  handleNERCSV(data: any) {
    const header = ['TextSource', 'EntityType', 'EntityText', 'Index-start', 'Index-end', 'LabelerName', 'LabelerRole']
    const content = this.getContentNERCSV(data)
    content.unshift("\uFEFF" + header.join(','))
    return content.join('\r\n')
  }

  getContentNERCSV(data: any) {
    let result = []
    data.Results.map(val => {
      let content = []
      val.LabelerResults.map(item => {
        let lebelerResult = []
        for (let i = 0; i < item.LabelResults.length; i++) {
          let entitytext = val.Item.Text.slice(item.LabelResults[i].Start, item.LabelResults[i].End)
          let itemContent = [val.Item.Text, item.LabelResults[i].EntityType, entitytext, item.LabelResults[i].Start, item.LabelResults[i].End, item.LabelerId, item.LabelerRole]
          lebelerResult.push(itemContent)
        }
        content.push(lebelerResult)
      })
      result.push(content)
    })
    return this.getNERStr(result)
  }

  getNERStr(data: any[]): any {
    let resultArr = []
    data.map(val => {
      val.map(item => {
        item.map(el => {
          resultArr.push(el)
        })
      })
    })

    var dataType = "\uFEFF"
    return resultArr.map(val => dataType + val.join(','))
  }

  exportJson(res: any, doc: any) {
    let downContent: any;
    if (this.workspaceType == WOKSPACR_TYPE_LIST[0].value) {
      downContent = this.handleTextSimilarity(res);
    } else {
      downContent = {
        Results: res[0].Results.map((item: any, index: number) => {
          item.LabelerResults = res.map(r => {
            return r.Results[index].LabelerResults[0];
          });
          return item;
        })
      };
    }
    var blob = new Blob([JSON.stringify(downContent)], { type: "text/plain;charset=utf-8" });
    let name = doc.name.split('.')[0] + '(Output).json'
    saveAs(blob, `${name}`);
  }

  handleTextSimilarity(res: any[]) {
    let result = { Results: [] };
    res[0].Results.map((data: any, index: number) => {
      let textTargetSet = new Set();
      let labelerResults = [];
      res.forEach(r => {
        r.Results[index].Item.TextTargets.forEach(t => {
          textTargetSet.add(t);
        })
      });
      res.forEach(r => {
        labelerResults.push({
          ...r.Results[index].LabelerResults[0],
          LabelResults: Array.from(textTargetSet).map(l => {
            let indexLabel = r.Results[index].Item.TextTargets.indexOf(l);
            return indexLabel !== -1 ? (r.Results[index].LabelerResults[0].LabelResults[indexLabel]) : 0;
          })
        });
      });
      result.Results.push({
        Item: { ...data.Item, TextTargets: Array.from(textTargetSet) },
        LabelerResults: labelerResults
      });
    });
    return result;
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

  checkAll(isAll) {
    if (isAll) {
      this.isIndeterminate = false
      this.allChecked = true
      this.docList.map(val => val.checked = true)
    } else {
      this.isIndeterminate = false
      this.allChecked = false
      this.docList.map(val => val.checked = false)
    }
    this.selectedItems = this.docList.filter(val => val.checked == true).length
  }

  checkOne(data) {
    let selected = this.docList.filter(val => val.checked == true)
    if (selected.length > 0) {
      this.isIndeterminate = true
    } else {
      this.isIndeterminate = false
    }
    this.allChecked = false
    if (selected.length === this.docList.length) {
      this.isIndeterminate = false
      this.allChecked = true
    }
    this.selectedItems = this.docList.filter(val => val.checked == true).length
  }

  batchExport() {
    this.isLoading = true
    let selected = this.docList.filter(val => val.checked == true)
    let list = selected.map(val => {
      return this.api.getAnnotationResult(this.workspaceId, this.taskInfo.id, val.id).pipe(
        switchMap((res: any) => {
          let manager = res.find((r: any) => r.annotatedBy.roles.indexOf('Manager') !== -1);
          if (manager) {
            return this.getResultList([manager])
          }
          return of(false);
        })
      )
    })
    forkJoin(...list).subscribe(res => {
      if (res.filter(val => val).length === 0) {
        this.msg.warning('暂无无标注结果');
        this.isLoading = false
        return
      }
      let result = res.filter(val => val).map(val => val[0]).map(val => {
        return {
          id: val.id,
          name: val.name,
          tags: val.tags.map(el => el.name)
        }
      })
      let blob = new Blob([JSON.stringify(result)], { type: "text/plain;charset=utf-8" });
      let name = this.taskInfo.name + '(Output).json'
      saveAs(blob, `${name}`);
      this.isLoading = false
    })
  }

}
