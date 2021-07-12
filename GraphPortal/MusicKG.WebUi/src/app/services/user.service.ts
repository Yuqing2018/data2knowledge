import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { UserLoginInfo, UserList, UserInfo } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient
  ) { }

  login(name: string, password: string): Observable<UserLoginInfo> {
    return this.http.post<UserLoginInfo>(`/User/Login`, { name, password });
  }

  getUserList(): Observable<UserList> {
    return this.http.get<UserList>(`/User`)
  }

  getAnnotatorList(): Observable<UserList> {
    return this.http.get<UserList>(`/User?role=Annotator`)
  }

  createUser(data: object): Observable<UserInfo> {
    return this.http.post<UserInfo>(`/User`, data)
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete<any>(`/User/${id}`)
  }

  updateUser(id: string, data: object): Observable<UserInfo> {
    return this.http.put<UserInfo>(`/User/${id}`, data)
  }

}
