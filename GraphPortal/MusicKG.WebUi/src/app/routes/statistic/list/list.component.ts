import { Component, OnInit, OnDestroy } from '@angular/core';
import { Platform } from '@angular/cdk/platform';
import { StatisticService } from 'src/app/services/statistic.service';
import { NzMessageService } from 'ng-zorro-antd';
import { StatisticOverview } from 'src/app/interfaces/statistic.interfaces';
import { Subject } from 'rxjs';

@Component({
  selector: 'km-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.less']
})
export class ListComponent implements OnInit, OnDestroy {

private unsubscribe = new Subject<void>();
  timeBtn: Array<any> = [
    { value: '昨天', type: 'default', days: 1 },
    { value: '过去7天', type: 'default', days: 7 },
    { value: '过去30天', type: 'default', days: 30 },
  ]
  listOfOption: Array<any> = [
    { value: 'total', displayName: '全部' },
    { value: '5c414bd75a01cf00010f9660', displayName: '相似文本' },
    { value: '5c414c0a5a01cf00010f9661', displayName: '实体识别' },
  ]
  initBtnValue: string = '过去7天'
  selectedValue: string = 'total'
  loading: boolean = false
  filterTime: any = { start: null, end: null }
  queryTime: any = { start: null, end: null }
  dataSet: StatisticOverview
  workspaceType: string

  constructor(
    private platform: Platform,
    private message: NzMessageService,
    private statisticService: StatisticService
  ) { }

  ngOnInit() {
    this.getBtn(this.initBtnValue)
    this.search()
  }

  getBtn(value: string) {
    Object.keys(this.filterTime).map(val => this.filterTime[val] = null)
    this.timeBtn.filter(val => val.value === value)[0].type = 'primary'
    this.timeBtn.filter(val => val.value !== value).map(val => val.type = 'default')
  }

  onChange() {
    this.timeBtn.map(val => val.type = 'default')
  }

  reset() {
    this.getBtn(this.initBtnValue)
    this.selectedValue = 'total'
    Object.keys(this.filterTime).map(val => this.filterTime[val] = null)
    this.search()
  }

  search() {
    let time = this.getTime()
    if (time != null) {
      this.getList(time.start, time.end, this.selectedValue)
    }
  }

  getTime() {
    let time: any
    let btn = this.timeBtn.filter(val => val.type === 'primary')
    btn.length !== 0 ? time = this.parseBtnTime(btn[0]) : time = this.filterTime
    Object.keys(time).map(val => time[val] = this.formatTime(time[val]))
    if (!Boolean(time.start) || !Boolean(time.end)) {
      this.message.warning('请选择时间范围')
      return
    }
    if (new Date(time.end) < new Date(time.start)) {
      this.message.warning('请选择正确时间范围')
      return
    }
    if (this.platform.TRIDENT) {
      time.start = time.start.replace(/-/g, '/')
      time.end = time.end.replace(/-/g, '/')
    }
    time.start = new Date(time.start + ' 00:00:00').toISOString()
    let endTime = new Date(time.end + ' 23:59:59')
    endTime.setMilliseconds(999)
    time.end = endTime.toISOString()
    return time

  }

  parseBtnTime(btn: any) {
    let now = new Date().getTime()
    let end = now - 1000 * 60 * 60 * 24
    let start = end - (1000 * 60 * 60 * 24) * (btn.days - 1)
    return { start: new Date(start), end: new Date(end) }
  }

  formatTime(val: Date) {
    this.formatTimeIE()
    if (val) {
      let value = new Date(val)
      let year = value.getFullYear()
      let month = (value.getMonth() + 1).toString().padStart(2, '0')
      let day = value.getDate().toString().padStart(2, '0')
      return year + '-' + month + '-' + day
    } else {
      return ''
    }
  }

  formatTimeIE() {
    if (!String.prototype.padStart) {
      String.prototype.padStart = function padStart(targetLength, padString) {
        targetLength = targetLength >> 0; //truncate if number, or convert non-number to 0;
        padString = String(typeof padString !== 'undefined' ? padString : ' ');
        if (this.length >= targetLength) {
          return String(this);
        } else {
          targetLength = targetLength - this.length;
          if (targetLength > padString.length) {
            padString += padString.repeat(targetLength / padString.length); //append to original to ensure we are longer than needed
          }
          return padString.slice(0, targetLength) + String(this);
        }
      };
    }
  }

  getList(fromDate: string, toDate: string, workspaceTypeId: string) {
    this.loading = true
    this.statisticService.getList(fromDate, toDate, workspaceTypeId).subscribe(res => {
      this.dataSet = res
      this.queryTime.start = this.formatTime(new Date(fromDate))
      this.queryTime.end = this.formatTime(new Date(toDate))
      this.workspaceType = workspaceTypeId
    }, err => {
      this.message.error(err.msg)
    }, () => {
      this.loading = false
    })
  }

  ngOnDestroy() {
    this.unsubscribe.next();
  }

}