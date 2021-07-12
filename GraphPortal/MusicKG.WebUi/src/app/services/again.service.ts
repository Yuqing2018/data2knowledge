import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AgainService {

  constructor(
    private http: HttpClient
  ) { }

  delAgain(id: string) {
    return this.http.delete(`/honda/v1/vehiclefault/reannotationtask/${id}`);
  }

  getAgainList(from: number = 0, size: number = 10,status:string,showAll:boolean) {
    const url = `/honda/v1/vehiclefault/reannotationtask?from=${from}&size=${size}&status=${status}&showAll=${showAll}`
    return this.http.get<any>(url)
  }

  addAgainData(data: any) {
    return this.http.post(`/honda/v1/vehiclefault/reannotationtask`, data);
  }

  updateAgainData(id: string, data: any) {
    return this.http.put(`/honda/v1/vehiclefault/reannotationtask/${id}`, data);
  }

}
