import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CicrularService {

  constructor(
    private http: HttpClient
  ) { }

  warnTypeList(warningType: string) {
    return this.http.get(`/honda/v1/vehiclefault/warningindex?warningType=${warningType}`);
  }

  addTask(data: any) {
    return this.http.post(`/honda/v1/vehiclefault/warningtask`, data);
  }

  updateTask(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/warningtask/${id}`, data);
  }

  taskInfo(id: string) {
    return this.http.get(`/honda/v1/vehiclefault/warningtask/${id}`);
  }

  delTask(id: string) {
    return this.http.delete(`/honda/v1/vehiclefault/warningtask/${id}`);
  }

  stopTask(id: string) {
    return this.http.put(`/honda/v1/vehiclefault/warningtask/${id}/termination`, null);
  }

  getTask(warningType: string, warningStatus: string, from: number, createdBy: string, warningUnit: string,carModels:string) {
    return this.http.get(`/honda/v1/vehiclefault/warningtask`, {
      params: this._handleParams({
        warningType,
        warningStatus,
        from,
        size: 10,
        createdBy,
        warningUnit,
        carModels
      })
    });
  }

  private _handleParams(params: any) {
    const paramsClone = JSON.parse(JSON.stringify(params));
    Object.keys(paramsClone).forEach(key => {
      if (paramsClone[key] === null || paramsClone[key] === '') {
        delete paramsClone[key];
      }
    });
    return paramsClone;
  }

  getSyndrome(keyword: string, from: number = 0, size: number = 10) {
    const url = `/honda/v1/vehiclefault/syndrome/list?from=${from}&size=${size}&keyword=${keyword}`
    return this.http.get<any>(url)
  }

  addSyndrome(data: any) {
    return this.http.post(`/honda/v1/vehiclefault/syndrome`, data);
  }

  updateSyndrome(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/syndrome/${id}`, data);
  }

  warningTrack(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/warningtrack`, {
      params: this._handleParams(data)
    })
  }

  exportWarningTrack(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/warningtrack/export`, {
      params: this._handleParams(data),
      responseType: 'arraybuffer'
    })
  }

  exportRiskReport(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/warningtrack/riskreport`, {
      params: this._handleParams(data),
      responseType: 'arraybuffer'
    })
  }

  info(id: string, status: string, from: number, size: number) {
    return this.http.get(`/honda/v1/vehiclefault/warningrecord`, {
      params: this._handleParams({
        detailId: id,
        status,
        from,
        size
      })
    })
  }  

  confirm(id: string, data: any) {
    return this.http.post(`/honda/v1/vehiclefault/warningrecord/${id}/confirm`, data);
  }

  warningRecordInfo(id: string) {
    return this.http.get(`/honda/v1/vehiclefault/warningtrack/${id}`);
  }

  editFreqencyAndFocused(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/warningtrack/${id}/frequency`, data);
  }

  editRishLevel(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/warningrecord/${id}/aiRiskLevel`, data);
  }

  saveSetting(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/warningtrack/${id}/settings`, data);
  }

  warningCount() {
    return this.http.get(`/honda/v1/vehiclefault/warningtrack/warningCount`);
  }

  getCarType(dataSource?: string) {
    return this.http.get(`/honda/v1/vehiclefault/data/carmodels`, {
      params: { dataSource: dataSource || '' }
    }).pipe(
      map((d: any) => {
        return d && d.sort((a,b) => a.substr(0, 1).toLowerCase().charCodeAt(0) - b.substr(0, 1).toLowerCase().charCodeAt(0))
      })
    )
  }

}
