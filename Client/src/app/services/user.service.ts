import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { IUser } from '../models/user/user';
import { IPutUserPassword } from '../models/user/putUserPassword';
import { IPutUser } from '../models/user/putUser';
import { IPostUser } from '../models/user/postUser';
import { BehaviorSubject } from 'rxjs'

const apiUrl = 'api/users/';

@Injectable()
export class UserService {

  public currentUser: BehaviorSubject<IUser> = new BehaviorSubject(null);
  
  constructor(private _httpClient: HttpClient) { }

  public getUsers(): Observable<Array<IUser>> {
    return this._httpClient.get<Array<IUser>>(apiUrl);
  }

  public getUser(id: string): Observable<IUser> {
    return this._httpClient.get<IUser>(apiUrl + id);
  }

  public getCurrentUser(): Observable<IUser> {
    return this._httpClient.get<IUser>(apiUrl + 'current');
  }

  public putPassword(model: IPutUserPassword) {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.put(apiUrl + 'password', JSON.stringify(model), { headers: httpHeaders });
  }

  public putUser(user: IPutUser) {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.put(apiUrl, JSON.stringify(user), { headers: httpHeaders });
  }

  public postUser(user: IPostUser) {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.post(apiUrl, JSON.stringify(user), { headers: httpHeaders });
  }
}
