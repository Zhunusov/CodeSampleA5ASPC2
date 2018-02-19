import { Injectable } from '@angular/core';
import { IJwtToken } from '../models/authorization/jwtToken';
import { Observable } from 'rxjs/Observable';
import { ISingIn } from '../models/authorization/singIn';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UserService } from './user.service';

import 'rxjs/add/operator/retry';
import 'rxjs/add/operator/delay';
import { Router } from '@angular/router';

const authorizationUrl = '/api/Authorization';

const AccessTokenLifeTime = 60000;

@Injectable()
export class AuthorizationService {

    private getAccessTokenIntervalId: any;

    constructor(
        private _httpClient: HttpClient,
        private _userService: UserService,
        private _router: Router) { }

    public logout(): void {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('userId');
        this._userService.currentUser.next(null);
        this.stopGetAccessjwtTokenSheduler();
        this._router.navigate(['/home']);
    }

    public static accessTokenExistInLocalStorage(): boolean {
        let token = localStorage.getItem('accessToken');
        return token != null;
    }

    public getAccessTokenFromLocalStorage() {
        return localStorage.getItem('accessToken');
    }

    public stopGetAccessjwtTokenSheduler() {
        clearTimeout(this.getAccessTokenIntervalId);
    }

    public runGetAccessjwtTokenSheduler() {
        this.getAccessTokenIntervalId = setInterval(() => { this.updateAccessjwtToken(); console.log("Updated Jwt token"); }, AccessTokenLifeTime - 5000);
    }

    public updateAccessjwtToken() {
        if (AuthorizationService.accessTokenExistInLocalStorage()) {
            this.getNewAccessJwtToken().retry(10).delay(1000).subscribe(
                result => {
                    localStorage.setItem('accessToken', result.accessToken)
                },
                (error: HttpErrorResponse) => {
                    if (error.status == 400) {
                        this.stopGetAccessjwtTokenSheduler();
                        this.logout();
                    }
                }
            )
        } else {
            this.stopGetAccessjwtTokenSheduler();
            this.logout();
        }
    }

    public getNewAccessJwtToken(): Observable<IJwtToken> {
        let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
        let resetToken = localStorage.getItem("resetToken");
        let params = { resetToken: resetToken };
        return this._httpClient.post<IJwtToken>(authorizationUrl + '/UpdateAccessJwtToken', JSON.stringify(params), { headers: httpHeaders });
    }

    public setJwtToken(jwtToken: IJwtToken) {
        localStorage.setItem('resetToken', jwtToken.resetToken);
        localStorage.setItem('accessToken', jwtToken.accessToken);
        localStorage.setItem('userId', jwtToken.userId);
    }

    public getJwtToken(singIn: ISingIn): Observable<IJwtToken> {
        let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
        return this._httpClient.post<IJwtToken>(authorizationUrl, JSON.stringify(singIn), { headers: httpHeaders });
    }

    public getJwtTokenFromLocalStorage(): IJwtToken {
        let jwtToken: IJwtToken = {
            resetToken: localStorage.getItem('resetToken'),
            accessToken: localStorage.getItem('accessToken'),
            userId: localStorage.getItem('userId')
        }
        return jwtToken;
    }

}
