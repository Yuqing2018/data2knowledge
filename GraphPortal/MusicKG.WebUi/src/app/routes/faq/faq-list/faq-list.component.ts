import { Component, OnInit } from '@angular/core';
import { FaqModel, FaqSearchQueryModel, FaqService } from 'src/app/services/faq.service';
import { startOfDay, endOfDay, format } from 'date-fns';

@Component({
  selector: 'km-faq-list',
  templateUrl: './faq-list.component.html',
  styleUrls: ['./faq-list.component.less']
})
export class FaqListComponent implements OnInit {
  searchQuery: FaqSearchQueryModel = {
    searchKeyword: null,
    searchType: 1,
    tmplId: null,
    startDate: null,
    endDate: null,
    status: null,
    pageIndex: 1,
    pageSize: 10
  };

  dateRange: Date[];
  faqList: FaqModel[] = [];
  loading = true;
  total: number = 0;

  constructor(
    private formalService: FaqService
  ) { }

  ngOnInit() {
    this.getList();
  }

  onSearch() {
    this.searchQuery.pageIndex = 1;
    this.searchQuery.startDate = (this.dateRange && this.dateRange.length > 0) ? format(startOfDay(this.dateRange[0]), 'YYYY-MM-DD HH:mm:ss') : null;
    this.searchQuery.endDate = (this.dateRange && this.dateRange.length > 0) ? format(endOfDay(this.dateRange[1]), 'YYYY-MM-DD HH:mm:ss') : null;
    this.getList();
  }

  onResetSearch() {
    this.searchQuery = Object.assign(this.searchQuery, {
      searchKeyword: null,
      searchType: 1,
      tmplId: null,
      status: null,
      startDate: null,
      endDate: null,
      pageIndex: 1,
      pageSize: 10
    });
    this.dateRange = null;
    this.getList();
  }

  getList() {
    this.loading = true;
    this.searchQuery = { ...this.searchQuery, question: '', answer: '', faqTmplNm: '' };
    let searchType = ['', 'question', 'answer', 'faqTmplNm'];
    let type = searchType[this.searchQuery.searchType];
    this.searchQuery[type] = this.searchQuery.searchKeyword;
    this.formalService.getList(this.searchQuery).subscribe(res => {
      this.loading = false;
      this.faqList = res.object.items;  
      this.total = res.object.itemCount; 
    });
  }

}
