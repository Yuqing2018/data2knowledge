import { Component, OnInit } from '@angular/core';
import { CicrularService } from 'src/app/services/cicrular.service';
import { saveAs } from 'file-saver';
import { Router, ActivatedRoute } from '@angular/router';
import { CircularStoreService } from '../circular-store.service';
import { Location } from '@angular/common';

@Component({
  selector: 'km-circular-track',
  templateUrl: './circular-track.component.html',
  styleUrls: ['./circular-track.component.less']
})
export class CircularTrackComponent implements OnInit {
  dataSet = [];

  searchLoading = false;

  pageIndex: number = 1;

  total = 0;

  filter: any;

  exportLoading  = false;

  exportRiskReportLoading = false;

  searchCache: any;

  constructor(
    private api: CicrularService,
    private router: Router,
    private store: CircularStoreService,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe(queryParams => {
      if (queryParams.snapshot) {
        this.pageIndex = this.store.taskSearch.pageIndex;
        this.searchCache = Object.assign(this.store.taskSearch);
        this.filter = Object.assign(this.store.taskSearch);
      }else {
        const user = JSON.parse(localStorage.getItem('user'));
        this.filter = {
          createdBy: user.name
        };
      }
      this.onSearch();
      this.store.resetTaskSearch();
      this.location.go('/annotator/circular/track');  
    })
  }

  gotoInfo(item: any) {
    this.router.navigateByUrl(`/annotator/circular/track/info/${item.id}?taskId=${item.warningTask.id}`);
  }

  onSearch() {
    this.searchLoading = true;
    this.api.warningTrack({
      ...this.filter,
      from: (this.pageIndex - 1) * 10,
      size: 10
    }).subscribe((res: any) => {
      this.dataSet = res.items;
      this.total = res.totalCount;
      this.searchLoading = false;
      this.store.taskSearch = {
        ...this.filter,
        pageIndex: this.pageIndex
      }
    });
  }

  filterChange(filter: any) {
    this.pageIndex = 1;
    this.filter = filter;
    this.onSearch();
  }

  pageIndexChange(index: any) {
    this.pageIndex = index;
    this.onSearch();
  }

  onExport() {
    this.exportLoading = true;
    this.api.exportWarningTrack({...this.filter}).subscribe((res: any) => {
      var blob = new Blob([res], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
      saveAs(blob, `预警记录导出数据.xlsx`);
      console.log('export')
      this.exportLoading = false;
    }, err => {
      this.exportLoading = false;
    });
  }


  onExportRiskReport() {
    this.exportRiskReportLoading = true;
    this.api.exportRiskReport({...this.filter}).subscribe((res: any) => {
      var blob = new Blob([res], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
      saveAs(blob, `风险台账导出数据.xlsx`);
      console.log('export')
      this.exportRiskReportLoading = false;
    }, err => {
      this.exportRiskReportLoading = false;
    });
  }
}
