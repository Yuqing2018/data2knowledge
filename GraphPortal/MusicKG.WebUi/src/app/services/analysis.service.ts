import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AnalysisService {

  constructor(
    private http: HttpClient
  ) { }

  getList(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/data`, {
      params: this._handleParams(data)
    })
  }

  listExport(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/data/report`, {
      params: this._handleParams(data),
      responseType: 'arraybuffer'
    });
  }

  info(id: string) {
    return this.http.get(`/honda/v1/vehiclefault/data/${id}`)
  }

  update(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/data/${id}`,data)
  }

  updateQis(id: string, data: any) {
    return this.http.post(`/honda/v1/vehiclefault/data/qis/${id}/link`,data)
  }
  
  deleteqis(id:string){
    return this.http.delete(`/honda/v1/vehiclefault/data/qis/${id}`)
  }

  manualadd(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/data/qis/${id}`,data)
  }
  
  updateCntrTime(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/data/qis/${id}/cntrTime`,data)
  }

  updateQisParts(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/data/relatedPartName/${id}`,data)
  }

  updateQisDataFrom(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/data/dataFrom/${id}`,data)
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

  getCarModel(carType: string[], dataSource?: string) {
    return this.http.get(`/honda/v1/vehiclefault/data/carmodels/cartypes`, {
      params: {carModel: carType || '', dataSource: dataSource || '' }
    }).pipe(
      map((d: any) => {
        return d && d.sort((a,b) => a.substr(0, 1).toLowerCase().charCodeAt(0) - b.substr(0, 1).toLowerCase().charCodeAt(0))
      })
    );
  }

  getYearModel(carType: string[], carModel: string[], dataSource?: string) {
    return this.http.get(`/honda/v1/vehiclefault/data/carmodels/modelyears`,{
      params: { carModel: carType || '', carType: carModel || '', dataSource: dataSource || '' }
    }).pipe(
      map((d: any) => {
        return d && d.sort((a,b) => b - a)
      })
    );
  }

  getPartName(carType: string[], carModel: string[], yearModels: any, dataSource?: string) {
    return this.http.get(`/honda/v1/vehiclefault/data/carmodels/partnames`, { 
      params: this._handleParams({
        carModel: carType || '',
        carType: carModel || '',
        modelYears: yearModels == '全部' ? '' : yearModels,
        dataSource: dataSource || ''
      })
    });
  }

  getSyndromeByPartName(carType: string, carModel: string, yearModels: any, partName: any, dataSource?: string) {
    return this.http.get(`/honda/v1/vehiclefault/data/carmodels/syndromes`, {
      params: this._handleParams({
        carModel: carType || '',
        carType: carModel || '',
        modelYears: yearModels == '全部' ? '' : yearModels,
        dataSource: dataSource || '',
        partName: partName
      })
    });
  }

  getQisList(carType: string, carModel: string, yearModels: any, partName: any, dataSource: string ,frameNo: string ,from: number = 0, size: number = 10) {
    return this.http.get(`/honda/v1/vehiclefault/data/qis/list?from=${from}&size=${size}`, {
      params: this._handleParams({
        carModel: carType || '',
        carType: carModel || '',
        modelYears: yearModels == '全部' ? '' : yearModels,
        dataSource: dataSource || '',
        partName: partName || '',
        frameNo: frameNo || ''
      })
    });
  }

  getSyndrome(carType: string, carModel: string, yearModels: any) {
    return this.http.get(`/honda/v1/vehiclefault/data/carmodels/syndromes`, {
      params: this._handleParams({
        carModel: carType || '',
        carType: carModel || '',
        modelYears: yearModels
      })
    }); 
  }

  syndrome(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/statistics/${data.selectType == 'partName' ? 'syndrome' : 'partname'}`, {
      params: this._handleParams({
        carModel: data.carType,
        carType: data.carModel,
        yearModels: data.yearModels,
        partName: data.partName,
        syndrome: data.syndrome,
        datasource:data.dataSource
      })
    });
  }

  statistic(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/statistics`, {
      params: this._handleParams({
        carModel: data.carType,
        carType: data.carModel,
        yearModels: data.yearModels,
        partName: data.partName,
        syndrome: data.syndrome,
        datasource:data.dataSource
      })
    });
  }

  days(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/statistics/days`, {
      params: this._handleParams({
        carModel: data.carType,
        carType: data.carModel,
        yearModels: data.yearModels,
        partName: data.partName,
        syndrome: data.syndrome,
        month: data.month,
        chartType: data.chartType,
        datasource:data.dataSource
      })
    });
  }

  linedays(data: any, type: string) {
    return this.http.get(`/honda/v1/vehiclefault/statistics/${type}/days`, {
      params: this._handleParams({
        carModel: data.carType,
        carType: data.carModel,
        yearModels: data.yearModels,
        partName: data.partName,
        syndrome: data.syndrome,
        month: data.month,
        chartType: data.chartType,
        datasource:data.dataSource
      })
    });
  }

  private _handleParams(params: any) {
    const paramsClone = JSON.parse(JSON.stringify(params));
    Object.keys(paramsClone).forEach(key => {
      if (paramsClone[key] === undefined || paramsClone[key] === null || paramsClone[key] === '') {
        delete paramsClone[key];
      }
    });
    return paramsClone;
  }

  export(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/statistics/report`, {
      params: this._handleParams(data),
      responseType: 'arraybuffer'
    });
  }

  exportChart(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/statistics/report/chart`, {
      params: this._handleParams({
        carModel: data.carType,
        carType: data.carModel,
        yearModels: data.yearModels,
        partName: data.partName || '',
        syndrome: data.syndrome,
        datasource:data.dataSource
      }),
      responseType: 'arraybuffer'
    });
  }
  
  riskLevel(data: any) {
    return this.http.get(`/honda/v1/vehiclefault/warningrecord/riskLevel`, {
      params: this._handleParams({
        carModels: data.carType,
        carTypes: data.carModel,
        yearModels: data.yearModels,
        partName: data.partName,
        syndrome: data.syndrome,
        warningUnit: data.warningUnit
      }),
      responseType: 'text'
    });
  }

  getTaskIds() {
    return this.http.get(`/honda/v1/vehiclefault/warningtask/options`).pipe(
      map((d: any) => {
        return d && d.sort((a,b) => b - a )
      })
    );
  }
//首页部分
  getwarningData() {
    return this.http.get(`/honda/v1/vehiclefault/overall/overall/warning/metrics`);
  }

  getwarningTypesData(warningType:string) {
    return this.http.get(`/honda/v1/vehiclefault/overall/overall/warning/bywarningtype`,{
      params: { warningType: warningType}
    });
  }

  getdatasourcesData(datasource:string,frequency:string,start?:string,end?:string) {
    return this.http.get(`/honda/v1/vehiclefault/overall/overall/vehiclefault/lastweek`,{
      params: { datasource: datasource,frequency:frequency,start:start,end:end}
    });
  }

  getdatasourceTopData(start?:string,end?:string) {
    return this.http.get(`/honda/v1/vehiclefault/overall/overall/vehiclefault/top`,{
      params:{start:start,end:end}
    });
  }

  getkeyriskData(){
    return this.http.get(`/honda/v1/vehiclefault/overall/overall/warning/focus`);
  }
}


