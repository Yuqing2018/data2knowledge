import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DocumentData } from '../interfaces/pre-mark.interface';
import { HttpClient } from '@angular/common/http';
import { Option,Page } from '../interfaces/api.interface';

@Injectable({
  providedIn: 'root'
})
export class PreMarkService {

  constructor(private http: HttpClient) { }

  // 获取文档管理列表  Workspace/{workspaceId}/Document
  getDocumentList(workspaceId:string,params?:any): Observable<Page<DocumentData>> {
    const url = `/Workspace/${workspaceId}/Document`;
    return this.http.get<Page<DocumentData>>(url,{params:params} );
  }

  //获取可以用于创建任务的文档列表
  getCreateTaskDocumentList(workspaceId: string) {
    return this.http.get<any>(`/Workspace/${workspaceId}/Document?statuses=Preannotated&statuses=Assigned`);
  }

  // 上传文档
  uploadDocument(workspaceId:string,params:any): Observable<any> {
    const url = `/Workspace/${workspaceId}/Document/Content?documentItems=706`;
    return this.http.post<any>(url, params );
  }
  // 修改文档
  updateDocument(workspaceId:string,params:any,documentId:string): Observable<DocumentData> {
    const url = `/Workspace/${workspaceId}/Document/${documentId}`;
    return this.http.put<DocumentData>(url, params);
  }
  // 删除某条文档 /api/Workspace/{workspaceId}/Document/{documentId}
  deleteDocument(workspaceId:string,documentId:string): Observable<void> {
    const url = `/Workspace/${workspaceId}/Document/${documentId}`;
    return this.http.delete<void>(url);
  }
  // 查看文档 /api/Workspace/{workspaceId}/Document/Content/{documentId}
  downloadDocument(workspaceId:string,documentId:string): Observable<any> {
    const url = `/Workspace/${workspaceId}/Document/Content/${documentId}`;
    return this.http.get<any>(url);
  }

  /**
   * Get Document Status option list
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getDocumentStatus(): Observable<Option[]> {
    return this.http.get<Option[]>(`/Option?type=DocumentStatus`);
  }


  getGraph(workspaceId:string,documentId:string): Observable<any> {
    const url = `/Workspace/${workspaceId}/Document/Content/${documentId}`;
    return this.http.get(url, {responseType: 'arraybuffer'})
  }

}

