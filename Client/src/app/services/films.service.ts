import { Injectable } from '@angular/core';
import { IFilm } from '../models/film';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


@Injectable()
export class FilmsService {

  protected apiUrl = 'api/films';

  constructor(protected _httpClient: HttpClient) { }

  public search(count: number, searchString: string, offset: number = 0): Observable<Array<IFilm>> {
    let params = new HttpParams();
    params = params.append('count', count.toString());
    if (offset > 0) {
      params = params.append('offset', offset.toString());
    }
    params = params.append('searchString', searchString);
    return this._httpClient.get<Array<IFilm>>(this.apiUrl + '/search', { params: params });
  }

  public getCount(): Observable<number> {
    return this._httpClient.get<number>(this.apiUrl + '/count');
  }

  public get(count: number = 0, offset: number = 0): Observable<Array<IFilm>> {
    let params = new HttpParams();
    params = params.append('count', count.toString());
    if (offset > 0) {
      params = params.append('offset', offset.toString());
    }
    return this._httpClient.get<Array<IFilm>>(this.apiUrl, { params: params });
  }

  public getById(id: number): Observable<IFilm> {
    return this._httpClient.get<IFilm>(this.apiUrl + '/' + id);
  }

  public put(entity: IFilm) {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.put(this.apiUrl, JSON.stringify(entity), { headers: httpHeaders });
  }

  public post(entity: IFilm): Observable<IFilm> {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.post<IFilm>(this.apiUrl, JSON.stringify(entity), { headers: httpHeaders });
  }

  public delete(id: number) {
    let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._httpClient.delete(this.apiUrl + '/' + id);
  }
}
