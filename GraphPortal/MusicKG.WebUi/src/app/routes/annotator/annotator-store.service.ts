import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AnnotatorStoreService {
  taskSearch = {
    pageIndex: 1,
    selectedStatus: [],
    searchKey: ''
  };

  constructor() { }

  resetTaskSearch() {
    this.taskSearch = {
      pageIndex: 1,
      selectedStatus: [],
      searchKey: ''
    };
  }

}
