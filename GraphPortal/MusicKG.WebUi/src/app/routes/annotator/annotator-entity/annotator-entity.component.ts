import { Component, OnInit, ViewChild, HostListener, OnDestroy } from '@angular/core';
import { EntityAnnotationToolComponent } from 'src/app/shared/entity-annotation-tool/entity-annotation-tool.component';
import { OntologyService } from 'src/app/services/ontology.service';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest, forkJoin, of, Subject } from 'rxjs';
import { TaskService } from 'src/app/services/task.service';
import { OntologyEntityInfo } from 'src/app/interfaces/ontology.interface';
import { UserInfo } from 'src/app/interfaces/user.interface';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { PreMarkService } from 'src/app/services/pre-mark.service';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { pipe } from '@angular/core/src/render3';
import { ONTOLOGY_DEFAULT_COLOR } from 'src/app/core/common';
import { ajv } from 'src/app/app.component';

@Component({
  selector: 'km-annotator-entity',
  templateUrl: './annotator-entity.component.html',
  styleUrls: ['./annotator-entity.component.less']
})
export class AnnotatorEntityComponent implements OnInit, OnDestroy, AnnotationCanDeactivate {
  private unsubscribe = new Subject<void>();
  taskName: string;
  workspaceId: string;
  workspaceType: string;
  taskId: string;
  docId: string;
  docList: any[];
  index: number;
  docName: string;
  userInfo: UserInfo;
  itemTotal = 0;
  itemPage = 0;
  taskDocumentStatus: string;
  oldResult: any;
  docContent: any;
  mode = 'entity';
  entityList: OntologyEntityInfo[];
  contentValidate = true;
  docContentSource: any;

  annotationData: any;

  _currentItemContent: any;
  set currentItemContent(value: any) {
    if (value) {
      this.annotationData = {
        id: value.Id,
        text: value.Text,
        entityList: value.SpanItems.filter(s => {
          return this.entityList.find(l => l.name == s.EntityType) && (s.Start >= 0 && s.Start < value.Text.length) && (s.End > 0 && s.End <= value.Text.length);
        }).map((s, index) => {
          let entity = this.entityList.find(l => l.name == s.EntityType);
          return {
            entityType: entity.name,
            start: s.Start,
            end: s.End,
            title: entity.name,
            color: entity.color,
            id: ++index
          }
        })
      };
    }
    this._currentItemContent = value;
  }

  get currentItemContent() {
    return this._currentItemContent
  }

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

  @ViewChild(EntityAnnotationToolComponent) tool: EntityAnnotationToolComponent;

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
    this.router.navigate([`../../${this.docId}/entity`], {
      relativeTo: this.route,
      queryParams: {
        name: this.taskName,
        workspaceId: this.workspaceId,
        type: this.workspaceType
      },
    });
  }

  constructor(
    private entityService: OntologyService,
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private localStorage: LocalStorageService,
    private docService: PreMarkService,
    private msg: NzMessageService,
    private modalService: NzModalService
  ) { }

  ngOnInit() {
    this.userInfo = this.localStorage.get('user');
    combineLatest(
      this.route.pathFromRoot[3].params,
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
      this.getEntityList();
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  canDeactivate() {
    return !this.contentValidate || this.taskDocumentStatus == 'Annotated' || (this.oldResult && this.oldResult == JSON.stringify(this.parseResult())) || (!this.oldResult && this.docContent.Items.every(item => {
      return !item.SpanItems || item.SpanItems.length == 0;
    }));
  }


  initContent(docId: string) {
    this.currentItemContent = null;
    this.getDocContent(docId);
    this.itemPage = 0;
    if (this.docList && this.docList.length > 0) {
      this.docName = this.docList[this.index].name;
    }
  }

  save(status: string = 'Assigned') {
    let validate = this.validateResult();
    if (validate.success) {
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
    } else {
      this.currentItemContent = this.docContent.Items[validate.index];
      this.itemPage = validate.index;
      this.msg.warning(`第${validate.index + 1}项存在问题，标注的实体类型不能重叠`);
      this.viewChange('entity');
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

  validateResult() {
    for (let index = 0; index < this.docContent.Items.length; index++) {
      let spans = [];
      let item = this.docContent.Items[index];
      for (let span of item.SpanItems) {
        let isCover = spans.find(i => {
          return (i.Start >= span.Start && i.Start < span.End) || (i.End > span.Start && i.End <= span.End);
        });
        if (isCover) {
          return { success: false, index: index };
        } else {
          spans.push({ ...span });
        }
      }
    };
    return { success: true }
  }

  parseResult() {
    return {
      Results: this.docContent.Items.map((item, index) => {
        return {
          Item: { Id: item.Id, Text: item.Text, SpanItems: this.docContentSource.Items[index].SpanItems },
          LabelerResults: [
            { LabelerId: this.userInfo.name, LabelResults: item.SpanItems, LabelerRole: "Annotator" }
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
      if (ajv.validate('entity', content)) {
        this.docContentSource = JSON.parse(JSON.stringify(content));
        this.contentValidate = true;
        this.oldResult = result && JSON.stringify(result);
        content.Items = content.Items.map((item: any, index: number) => {
          let userResult: number[];
          if (result) {
            userResult = result.Results[index].LabelerResults.find(r => r.LabelerId == this.userInfo.name).LabelResults;
            item.SpanItems = userResult;
          } else {
            item.SpanItems = item.SpanItems && item.SpanItems.filter(s => {
              return this.entityList.find(l => l.name == s.EntityType) && (s.Start >= 0 && s.Start < item.Text.length) && (s.End > 0 && s.End <= item.Text.length);
            });
          }
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

  getEntityList() {
    this.entityService.getOntologyEntity(this.workspaceId).subscribe(res => {
      this.entityList = res.items;
      this.initContent(this.docId);
    });
  }

  addEntity(entity) {
    let selection = this.tool.selection;
    if (selection && !selection.entity) {
      let isCover = this.annotationData.entityList.find(i => {
        return (i.start >= selection.start && i.start < selection.end) || (i.end > selection.start && i.end <= selection.end);
      });
      if (isCover) {
        this.msg.warning('标注的实体类型不能重叠');
        return;
      }
    }
    this.tool.addEntity(entity);
  }

  viewChange(mode: any) {
    this.mode = mode;
    this.tool.changeMode(mode);
  }

  modelChange(value: any) {
    this._currentItemContent.SpanItems = value.entityList.map(e => {
      return {
        EntityType: e.entityType,
        Start: e.start,
        End: e.end
      }
    });
  }

  jumperItem(itemIndex: number) {
    if (itemIndex >= 0 && itemIndex < this.itemTotal) {
      this.itemPage = itemIndex;
      this.currentItemContent = this.docContent.Items[this.itemPage];
    }
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (!this.canDeactivate()) {
      $event.returnValue = true;
    }
  }
}
