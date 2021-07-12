import { Component, OnInit, OnDestroy } from '@angular/core';
import { TaskService } from 'src/app/services/task.service';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest, Subject, forkJoin } from 'rxjs';
import { DocumentResult, TaskDocument, TaskInfo } from 'src/app/interfaces/task.interface';
import { LocalStorageService } from 'src/app/core/local-storage.service';
import { tap, switchMap, takeUntil, finalize } from 'rxjs/operators';
import { WOKSPACR_TYPE_LIST } from 'src/app/core/common';

@Component({
  selector: 'km-annotator-document',
  templateUrl: './annotator-document.component.html',
  styleUrls: ['./annotator-document.component.less']
})
export class AnnotatorDocumentComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject<void>(); 
  taskName: string;
  workspaceId: string;
  taskId: string;
  docList: TaskDocument[] = [];
  workspaceType: string;
  taskInfo: TaskInfo;

  isLoading = false;

  constructor(
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.isLoading = true;
    combineLatest(
      this.route.params,
      this.route.queryParams
    ).pipe(
      takeUntil(this.unsubscribe),
      tap(([t, q]) => {
        this.taskName = q.name;
        this.workspaceId = q.workspaceId;
        this.taskId = t.task;
        this.workspaceType = q.type
      }),
      switchMap(q => forkJoin(
        this.taskService.getTaskDoc(this.workspaceId, this.taskId),
        // this.taskService.get(this.workspaceId, this.taskId)
      ).pipe(
        finalize(() => this.isLoading = false)
      ))
    ).subscribe(([res]) => {
      this.docList = res.items;
      // this.taskInfo = taskInfo;
      if (this.docList && this.docList.length > 0) {
        this.goTo(this.docList[0]);
      }
    });
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

  goTo(doc: TaskDocument) {
    this.router.navigate([`doc/${doc.id}/classification`], {
      queryParams: { workspaceId: this.workspaceId, name: this.taskName, type: this.workspaceType },
      relativeTo: this.route
    });
  }

}
