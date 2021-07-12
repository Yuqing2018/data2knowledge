import { Injectable } from '@angular/core';
import { getYear, subYears } from 'date-fns';

const INIT_YEARS = Array.from({ length: 5}).map((v,i) => getYear(subYears(new Date(), 4 - i)) + '');

@Injectable({
  providedIn: 'root'
})
export class CircularStoreService {
  taskSearch = {
    pageIndex: 1,
    taskIds: null,
    createdBy: null,
    warningUnit: null,
    carType: null,
    carModel: null,
    yearModels: INIT_YEARS,
    partNames: null,
    syndromes: null,
    focusType: null
  };

  constructor() { }

  resetTaskSearch() {
    this.taskSearch = {
      pageIndex: 1,
      taskIds: null,
      createdBy: null,
      warningUnit: null,
      carType: null,
      carModel: null,
      yearModels: INIT_YEARS,
      partNames: null,
      syndromes: null,
      focusType: null
    };
  }

}
