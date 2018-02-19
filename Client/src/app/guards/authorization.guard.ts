import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { retry } from 'rxjs/operator/retry';

@Injectable()
export class AuthorizationGuard implements CanActivate {

  public backRoutUrl: string = null;

  constructor(
    private _router: Router
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    let token = localStorage.getItem('accessToken');
    if (token == null) {
      this.backRoutUrl = this._router.url;
      this._router.navigate(['/singin']);
      return false;
    };
    return true;
  }
}
