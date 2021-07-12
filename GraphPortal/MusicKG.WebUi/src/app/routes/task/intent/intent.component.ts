import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { IntentAnnotationToolComponent } from 'src/app/shared/intent-annotation-tool/intent-annotation-tool.component';
import { Router, ActivatedRoute } from '@angular/router';
import { OntologyDialogueService } from 'src/app/services/ontology-dialogue.service';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { TaskService } from 'src/app/services/task.service';
import { AuthService } from 'src/app/core/auth.service';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { combineLatest, forkJoin, of } from 'rxjs';
import { tap, switchMap, map } from 'rxjs/operators';
import { unflatten, getRandomColor } from 'src/app/core/utils';
import { DEFAULT_OLORS } from 'src/app/core/common';

@Component({
  selector: 'km-intent',
  templateUrl: './intent.component.html',
  styleUrls: ['./intent.component.less']
})
export class IntentComponent implements OnInit {
  ontology: any = [];
  intent: any;
  currentIntentItem: any;
  workspaceId: string;
  taskId: string;
  docId: string;
  taskName: string;
  workspaceType: string;
  userInfo: UserInfo;
  taskDocumentStatus: any;
  docContentSource: any;
  docList: any;
  isChange = false;
  annotatorList: any;
  taskInfo: any;
  hasReviewPermission = false;
  annotationReuslt: any;

  index = 0;

  @ViewChild('intentComponent') intentComponent: IntentAnnotationToolComponent;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private docService: PreMarkService,
    private dialogService: OntologyDialogueService,
    private taskService: TaskService,
    private authService: AuthService,
    private modalService: NzModalService,
    private msg: NzMessageService
  ) { }

  get notHasPagePrevious() {
    return this.index == 0;
  }

  get notHasPageNext() {
    return this.intent && this.index == (this.intent.length - 1);
  }

  jumperItem(itemIndex: number) {
    if (itemIndex >= 0 && itemIndex < this.intent.length) {
      this.index = itemIndex;
      this.currentIntentItem = this.intent[this.index];
    }
  }

  pagePrevious() {
    if (this.index > 0) {
      this.index--;
      this.currentIntentItem = this.intent[this.index]
    }
  }

  pageNext() {
    if (this.index < (this.intent.length - 1)) {
      this.index++;
      this.currentIntentItem = this.intent[this.index]
    }
  }

  goTo() {
    this.router.navigate(['../../../../'], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      }
    })
  }

  ngOnInit() {
    this.userInfo = this.authService.getUser();
    this.getWorkSpaceAndTaskDocIds().pipe(
      tap(([w, t]) => {
        this.workspaceId = w.workspace;
        this.taskId = t.task;
        this.docId = t.doc;
      }),
      switchMap(() => this.getDocList())
    ).subscribe(() => {
      this.getEntityData();
    });
  }

  getWorkSpaceAndTaskDocIds() {
    return combineLatest(
      this.route.pathFromRoot[1].params,
      this.route.pathFromRoot[3].params
    );
  }

  onSave(status: string = 'Assigned') {
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
    let result = this.parseResult(this.intent);
    this.taskService.saveAnnotationResult(this.workspaceId, this.taskId, this.docId, {
      result: JSON.stringify(result),
      resultType: "ForModelTraining",
      status: status
    }).subscribe(res => {
      this.msg.success('操作成功');
      this.taskDocumentStatus = status;
      // this.jumperItem(this.index + 1);   
      this.isChange = false;
      this.getDocData();
    });
  }

  getDocList() {
    return this.taskService.getTaskDoc(this.workspaceId, this.taskId).pipe(
      tap(res => {
        this.docList = res.items;
      })
    );
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
      this.docService.downloadDocument(this.workspaceId, this.docId),
      this.taskService.get(this.workspaceId, this.taskId)
    ).subscribe(([res, doc, task]) => {
      this.taskInfo = task;
      this.annotatorList = res;
      this.docContentSource = JSON.parse(JSON.stringify(doc));
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

  selectedAnnotator() {
    let intentData = [];
    for (let index = 0; index < this.docContentSource.length; index++) {
      let intent = this.docContentSource[index];
      let trainingPhrases = [];
      if(this.annotatorList) {
        this.annotatorList.filter(a => a.checked).map(a => a.labelData).forEach(r => {
          for (let t of r[index].TrainingPhrases) {
            let isExit = trainingPhrases.find(s => {
              return s.Parts.map(ds => ds.Text).join(',') == t.Parts.map(ds => ds.Text).join(',') && s.TrainingPhraseType == t.TrainingPhraseType;
            });
            if (!isExit) {
              trainingPhrases.push(t);
            }
          }
        });
      }else {
        trainingPhrases = intent.TrainingPhrases || [];
      }
      intentData.push({
        ...intent,
        TrainingPhrases: JSON.parse(JSON.stringify(trainingPhrases))
      });
    }
    this.intent = this.setIntentColor(intentData);
    this.jumperItem(this.index);
  }

  getEntityData() {
    this.dialogService.getOntology(this.workspaceId).pipe(
      switchMap((d: any) => {
        if (d.entityDocumentId) {
          return this.docService.downloadDocument(this.workspaceId, d.entityDocumentId)
        } else {
          of(false);
        }
      })
    ).subscribe(res => {
      if (res) {
        let ontology = unflatten(res);
        this.setOntologyColor(ontology);
        this.getDocData();
      }
    });
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

  selectEntityChange(entity: any) {
    this.intentComponent.handleEntity(entity);
  }

  setOntologyColor(data: any) {
    let colorIndex = 0;
    this.ontology = data.map((ontology: any) => {
      ontology.Color = colorIndex > DEFAULT_OLORS.length - 1 ? getRandomColor() : DEFAULT_OLORS[colorIndex];
      colorIndex++;
      return ontology;
    })
  }

  setIntentColor(data: any) {
    return data.map((intent: any) => {
      intent.TrainingPhrases = intent.TrainingPhrases.map(t => {
        t.Parts = t.Parts.map(p => {
          let ontology = this.ontology.find(o => o.EntityType == p.EntityType);
          if (ontology) {
            p.Color = ontology.Color;
          } else {
            p.EntityType = null;
            p.Alias = null;
            p.Color = null;
          }
          return p;
        });
        return t;
      })
      return intent;
    });
  }

  parseResult(result: any) {
    result = JSON.parse(JSON.stringify(result));
    return result.map((item) => {
      item.TrainingPhrases = item.TrainingPhrases.map(d => {
        d.Parts = d.Parts.map(p => {
          delete p.Color;
          return p;
        })
        return d;
      });
      return item;
    });
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || !this.isChange;
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }

}
