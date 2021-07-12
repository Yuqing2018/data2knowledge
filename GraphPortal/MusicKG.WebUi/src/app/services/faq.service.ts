import { Injectable } from '@angular/core';
import { HttpClient, HttpBackend } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface FaqSearchQueryModel {
  searchKeyword: string;
  status: number,
  searchType: number;
  tmplId: number;
  question?: string;
  answer?: string;
  faqTmplNm?: string;
  startDate: string;
  endDate: string;
  pageIndex: number;
  pageSize: number;
}

export interface FaqModel {
  id: string;
  faqTmplId: number;
  knwlgTmplNm: string;
  faqTmplNm: string;
  question: string;
  answer: string;
  status: number;
  regnNms: string;
  updateTime: string;
  checked?: boolean;
}

export interface FaqInfoModel {
  faqTmplNm: string;
  knwlgTmplNm: string;
  question: string;
  answer: string;
  channelNms: string;
  regnNms: string;
  coreWord: string;
  keyWords: string;
  optsWord: string;
  optWordNm?: string;
  catalogue1: string;
  catalogue2: string;
  catalogue3: string;
  similarQuestion: string[];
  answerEffTime?: string;
  answerInvldTime?: string;
}

export interface KnowledgeTemplateModel {
  templId: number;
  templNm: string;
}

@Injectable({
  providedIn: 'root'
})
export class FaqService {
  private _http: HttpClient;

  constructor(
    handler: HttpBackend) {
    this._http = new HttpClient(handler);
  }


  getList(searchQuery: FaqSearchQueryModel): Observable<any> {
    return this._http.post(`${environment.faqBaseUrl}/Faq/list_all`, searchQuery);
  }

  getTemplateList(): Observable<any> {
    return this._http.get(`${environment.faqBaseUrl}/KnwlgTmpl`);
  }

  detail(faqId: string): Observable<any> {
    return this._http.get(`${environment.faqBaseUrl}/Faq/list_all/${faqId}`);
  }


}
