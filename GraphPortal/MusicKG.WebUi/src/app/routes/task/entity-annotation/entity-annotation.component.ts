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
import { switchMap, tap, takeUntil } from 'rxjs/operators';
import { AnnotationCanDeactivate } from 'src/app/core/can-deactivate.guard';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { ONTOLOGY_DEFAULT_COLOR } from 'src/app/core/common';
import { ajv } from 'src/app/app.component';
import { TaskInfo } from 'src/app/interfaces/task.interface';

@Component({
  selector: 'km-entity-annotation',
  templateUrl: './entity-annotation.component.html',
  styleUrls: ['./entity-annotation.component.less']
})
export class EntityAnnotationComponent implements OnInit, OnDestroy {
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
  entityList: any;
  annotatorList: any;
  annotationReuslt: any;
  hasReviewPermission = false;
  taskInfo: TaskInfo;
  docItemsRef: any;
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
            user: s.User,
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
      relativeTo: this.route
    });
  }

  constructor(
    private entityService: OntologyService,
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private localStorage: LocalStorageService,
    private msg: NzMessageService,
    private preService: PreMarkService,
    private modalService: NzModalService
  ) { }

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
      this.getEntityList();
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
      this.taskService.get(this.workspaceId,this.taskId)
    ).subscribe(([res, doc, task]) => {
      this.taskInfo = task;
      if (ajv.validate('entity', doc)) {
        this.docContentSource = JSON.parse(JSON.stringify(doc));
        this.docItemsRef = [].concat(doc.Items.map(d => {
          return { ...d };
        }));    
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
      } else {
        this.msg.warning('不支持该文档格式');
        this.itemTotal = 1;
        this.hasReviewPermission = false;
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

  selectedAnnotator() {
    this.viewChange('entity');
    let selectedUser = this.annotatorList.filter(l => l.checked).map(d => d.value);
    this.docContent.Items = this.docContent.Items.map((item: any, index: number) => {
      if (selectedUser && selectedUser.length > 0) {
        let userResult = this.annotationReuslt.filter(d => {
          return selectedUser.find(u => u == d.Results[index].LabelerResults[0].LabelerId);
        }).map(d => {
          return { name: this.annotatorList.find(a => a.value == d.Results[index].LabelerResults[0].LabelerId).label, result: d.Results[index].LabelerResults[0].LabelResults };
        });
        item.SpanItems = [];
        userResult.forEach(r => {
          r.result.forEach(u => {
            let exist = item.SpanItems.find(s => {
              return s.End == u.End && s.Start == u.Start && s.EntityType == u.EntityType;
            });
            if (exist) {
              exist.User = null;
            } else {
              item.SpanItems.push({
                ...u, User: userResult.length > 1 ? r.name : null
              });
            }
          });
        });
      }else {
        item.SpanItems = this.docItemsRef[index].SpanItems;
      }
      return item;
    });    
    this.itemTotal = this.docContent.Items.length;
    if (this.docContent.Items && this.docContent.Items.length > 0) {
       this.jumperItem(this.itemPage);
    }
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

  getWorkSpaceAndTaskDocIds() {
    return combineLatest(
      this.route.pathFromRoot[1].params,
      this.route.pathFromRoot[3].params
    );
  }

  canDeactivate() {
    return this.taskDocumentStatus == 'Annotated' || (this.oldResult && this.oldResult == JSON.stringify(this.parseResult())) || !this.oldResult;
  }

  save(status: string = 'Assigned') {
    let validate = this.validateResult();
    if (validate.success) {
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
    } else {
      this.currentItemContent = this.docContent.Items[validate.index];
      this.itemPage = validate.index;
      this.msg.warning(`第${validate.index + 1}项存在问题，标注的实体类型不能重叠`);
      this.viewChange('entity');
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
            {
              LabelerId: this.userInfo.name, LabelResults: item.SpanItems.map(d => {
                delete d.User;
                return d;
              }), LabelerRole: "Manager"
            }
          ]
        }
      })
    };
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
      this.currentItemContent = null;
      this.itemPage = 0;
      this.initDoc(this.docId);
    });
  }

  addEntity(entity) {
    let selection = this.tool.selection;
    if(selection && !selection.entity) {
      let isCover = this.annotationData.entityList.find(i => {
        return (i.start >= selection.start && i.start < selection.end) || (i.end > selection.start && i.end <= selection.end);
      });
      if(isCover) {
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
    if(this._currentItemContent) {
      this._currentItemContent.SpanItems = value.entityList.map(e => {
        return {
          EntityType: e.entityType,
          Start: e.start,
          End: e.end
        }
      });
    }
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
