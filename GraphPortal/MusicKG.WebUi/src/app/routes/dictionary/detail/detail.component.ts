import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest } from 'rxjs';
import { DictionaryService } from 'src/app/services/dictionary.service';
import { NzMessageService } from 'ng-zorro-antd';
import { DictDetail } from 'src/app/interfaces/dict.interface';

@Component({
  selector: 'km-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.less']
})
export class DetailComponent implements OnInit {

  workspaceId: string
  dictionaryId: string
  dataSet: DictDetail
  items: Array<string>
  value: string
  total: number = 1
  searchLoading: boolean = false
  from = 0
  searchFrom: number = 0
  isSearchModal: boolean = false
  pageIndex: number = 1


  constructor(
    private route: ActivatedRoute,
    private dictService: DictionaryService,
    private message: NzMessageService
  ) { }

  ngOnInit() {
    combineLatest(
      this.route.pathFromRoot[1].params,
      this.route.params
    ).subscribe(res => {
      this.workspaceId = res[1].workspace
      this.dictionaryId = res[1].dictionary
      this.getDetail()
    })
  }

  getDetail() {
    this.dictService.get(this.workspaceId, this.dictionaryId, this.from).subscribe(res => {
      this.dataSet = res
      this.items = res.entries.items
      this.total = res.entries.totalCount
      this.searchLoading = false
    })
  }

  search() {
    if (!this.value || !this.value.replace(/\s+/g, '') && this.value) {
      this.message.warning('空白字符不是合法词条')
      return
    }
    this.pageIndex = 1
    this.searchFrom = 0
    this.searchValue()
  }

  searchValue() {
    this.searchLoading = true
    this.isSearchModal = true
    this.dictService.getEntries(this.workspaceId, this.dictionaryId, this.value, this.searchFrom).subscribe(res => {
      this.items = res.items
      this.total = res.totalCount
      this.searchLoading = false
    }, err => {
      this.message.error(err.msg)
    })
  }

  pageIndexChange(index: number) {
    this.searchLoading = true
    if (this.isSearchModal) {
      this.searchFrom = (index - 1) * 10
      this.searchValue()
    } else {
      this.from = (index - 1) * 10
      this.getDetail()
    }

  }

  reset() {
    this.pageIndex = 1
    this.from = 0;
    this.searchFrom = 0
    this.value = null
    this.getDetail()
    this.searchLoading = true
    this.isSearchModal = false
  }

}
