import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { takeUntil, tap, switchMap, map } from 'rxjs/operators';
import { combineLatest, Subject } from 'rxjs';
import { Platform } from '@angular/cdk/platform';
import { StatisticService } from 'src/app/services/statistic.service';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'km-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.less']
})
export class DetailComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject<void>();
  count: any = { task: 0, doc: 0, item: 0 }
  time: any = { start: null, end: null }
  queryTime: any = { start: null, end: null }
  userId: string
  workspaceType: string
  dataSet: any

  get isManager() {
    return this.dataSet.user.roles[0] === 'Manager'
  }

  constructor(
    private route: ActivatedRoute,
    private platform: Platform,
    private statisticService: StatisticService,
    private message: NzMessageService
  ) { }

  ngOnInit() {
    combineLatest(
      this.route.params,
      this.route.queryParams
    ).pipe(
      takeUntil(this.unsubscribe),
      tap(([t, q]) => {
        this.userId = t.statistic;
        this.count.task = q.task;
        this.count.doc = q.doc;
        this.count.item = q.item
        this.time.start = q.start.replace(/-/g, '/')
        this.time.end = q.end.replace(/-/g, '/')
        this.workspaceType = q.type
        this.parseTime()
      }),
      switchMap(q => this.statisticService.getDetail(this.userId, this.queryTime.start, this.queryTime.end, this.workspaceType))
    ).subscribe(res => {
      this.dataSet = res
    }, err => {
      this.message.error(err.msg)
    });
  }

  parseTime() {
    let start: string
    let end: string
    if (this.platform.TRIDENT) {
      start = this.time.start.replace(/-/g, '/')
      end = this.time.end.replace(/-/g, '/')
    } else {
      start = this.time.start
      end = this.time.end
    }
    this.queryTime.start = new Date(start + ' 00:00:00').toISOString()
    let endTime = new Date(end + ' 23:59:59')
    endTime.setMilliseconds(999)
    this.queryTime.end = endTime.toISOString()
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

}
