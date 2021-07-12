import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { DEFAULT_OLORS } from 'src/app/core/common';
import { getRandomColor, unflatten } from 'src/app/core/utils';
import { IntentAnnotationToolComponent } from 'src/app/shared/intent-annotation-tool/intent-annotation-tool.component';
import { combineLatest, forkJoin, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { TaskService } from 'src/app/services/task.service';
import { tap, switchMap } from 'rxjs/operators';
import { OntologyDialogueService } from 'src/app/services/ontology-dialogue.service';
import { AuthService } from 'src/app/core/auth.service';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { NzModalService, NzMessageService } from 'ng-zorro-antd';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard';

@Component({
  selector: 'km-annotator-intent',
  templateUrl: './annotator-intent.component.html',
  styleUrls: ['./annotator-intent.component.less']
})
export class AnnotatorIntentComponent implements OnInit, AnnotationCanDeactivate {
  ontology: any = [];
  intent: any;
  currentIntentItem: any;
  workspaceId: string;
  taskId: string;
  docId: string;
  taskName: string;
  workspaceType: string;
  isLoading = false;
  userInfo: UserInfo;
  taskDocumentStatus: any;
  docContentSource: any;
  docList: any;
  isChange = false;

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
    combineLatest(
      this.route.params,
      this.route.queryParams
    ).pipe(
      tap(([t, q]) => {
        this.workspaceId = q.workspaceId;
        this.taskId = t.task;
        this.docId = t.doc;
        this.workspaceType = q.type;
      }),
      switchMap(() => this.getDocList())
    ).subscribe(() => {
      this.isLoading = true;
      this.getEntityData();
    });
  }

  onSave(status: string = 'Assigned') {
    if (status == 'Assigned') {
      // this.jumperItem(this.index + 1);
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
      this.isChange = false;
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
    forkJoin(
      this.getResult(this.docId),
      this.docService.downloadDocument(this.workspaceId, this.docId)
    ).subscribe(([result, content]) => {
      this.taskDocumentStatus = this.docList[0].status;
      this.setIntentColor(result || content);
      this.currentIntentItem = this.intent[0];
      this.isLoading = false;
    });
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
    this.intent = data.map((intent: any) => {
      intent.TrainingPhrases = intent.TrainingPhrases || [];
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
