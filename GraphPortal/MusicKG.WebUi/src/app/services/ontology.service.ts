import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Page,Option } from '../interfaces/api.interface';
import { OntologyEntityInfo,RelationData } from '../interfaces/ontology.interface';

@Injectable({
  providedIn: 'root'
})
export class OntologyService {

  constructor(
    private http: HttpClient
  ) { }

  /**
   * Get ontology entity list.
   *  api/Workspace/{workspaceId}/OntologyEntity
   */
  getOntologyEntity(workspaceId: string, from: number = 0): Observable<Page<OntologyEntityInfo>> {
    return this.http.get<Page<OntologyEntityInfo>>(`/Workspace/${workspaceId}/OntologyEntity?from=${from}`);
  }
    // 创建entity  /api/Workspace/{workspaceId}/OntologyEntity
  createOntologyEntity(workspaceId: string,params: any): Observable<OntologyEntityInfo> {
    return this.http.post<OntologyEntityInfo>(`/Workspace/${workspaceId}/OntologyEntity`,params);
  }
  // /api/Workspace/{workspaceId}/OntologyEntity/{entityId} Delete ontology entity.
  deleteOntologyEntity(workspaceId: string,entityId: string): Observable<any> {
    return this.http.delete<any>(`/Workspace/${workspaceId}/OntologyEntity/${entityId}`);
  }

  // /api/Workspace/{workspaceId}/OntologyEntity/{entityId}   Get ontology entity.
  getOntologyEntityByid(workspaceId: string,entityId: string): Observable<OntologyEntityInfo> {
    return this.http.get<OntologyEntityInfo>(`/Workspace/${workspaceId}/OntologyEntity/${entityId}`);
  }
  /**
   * PUT  /api/Workspace/{workspaceId}/OntologyEntity/{entityId}    Update ontology entity.
   */
  updateOntologyEntity(workspaceId: string,entityId: string ,params:any): Observable<OntologyEntityInfo> {
    return this.http.put<OntologyEntityInfo>(`/Workspace/${workspaceId}/OntologyEntity/${entityId}`,params);
  }
  
   /**
   * Get Entity Property Type option list
   *
   * @returns {Observable<Option[]>}
   * @memberof OptionService
   */
  getEntityPropertyType(): Observable<Option[]> {
    return this.http.get<Option[]>(`/Option?type=EntityPropertyType`);
  }

  /**
   * Get OntologyR elation ship list.
   *  /api/Workspace/{workspaceId}/OntologyRelationship   Get ontology entity list.
   */
  getOntologyRelationship(workspaceId: string, from: number = 0,size: number = 10): Observable<Page<RelationData>> {
    return this.http.get<Page<RelationData>>(`/Workspace/${workspaceId}/OntologyRelation?from=${from}&size=${size}`);
  }

  //   /api/Workspace/{workspaceId}/OntologyRelation // Create ontology relation.
  createOntologyRelation(workspaceId: string,params: any): Observable<RelationData> {
    return this.http.post<RelationData>(`/Workspace/${workspaceId}/OntologyRelation`,params);
  }

  //   /api/Workspace/{workspaceId}/OntologyRelation/{relationId}  // Update ontology relation.
  updateOntologyRelation(workspaceId: string,relationId:string, params: any): Observable<RelationData> {
    return this.http.put<RelationData>(`/Workspace/${workspaceId}/OntologyRelation/${relationId}`,params);
  }

  //  /api/Workspace/{workspaceId}/OntologyRelation/{relationId}   Delete ontology relation.
  deleteOntologyRelation(workspaceId: string,relationId: string): Observable<any> {
    return this.http.delete<any>(`/Workspace/${workspaceId}/OntologyRelation/${relationId}`);
  }
  
  // /api/Workspace/{workspaceId}/OntologyEntity/Content   // Upload ontology.
  uploadOntology(workspaceId: string,params: any): Observable<any> {
    return this.http.post<any>(`/Workspace/${workspaceId}/OntologyEntity/Content`,params);
  }
  // /api/Workspace/{workspaceId}/OntologyEntity/Content  // Download ontology.
  downloadOntology(workspaceId:string): Observable<any> {
    return this.http.get<any>(`/Workspace/${workspaceId}/OntologyEntity/Content`);
  }

}
