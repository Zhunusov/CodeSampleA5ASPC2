import 'rxjs/add/operator/do';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse, HttpClient } from '@angular/common/http';
import { Injector, Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/repeat';
import { AuthorizationService } from '../services/authorization-service.service';


@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

    private _router: Router;

    constructor(private _inj: Injector) {
        this._router = _inj.get(Router);
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request)
        .catch((error, caught) => {
            if (error instanceof HttpErrorResponse) {
                if (error.status === 401) {
                    this.removeTokensAndToSingInPage();
                }else{
                    if(error.status === 403){
                        this._router.navigate(['/404']);
                    }
                }
            }
        return Observable.throw(error);
        }) as any;
    }

    private removeTokensAndToSingInPage() {
        this.removeTokens();
        this._router.navigate(['/singin']);
    }

    private removeTokens(){
        localStorage.removeItem('accessToken');
        localStorage.removeItem('resetToken');
    }
}