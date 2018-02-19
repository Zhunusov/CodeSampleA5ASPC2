import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { AuthorizationService } from '../../../services/authorization-service.service';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { IPostUser } from '../../../models/user/postUser';
import { HttpErrorResponse } from '@angular/common/http';
import { ISingIn } from '../../../models/authorization/singIn';


@Component({
  selector: 'app-sing-up',
  templateUrl: './sing-up.component.html',
  styleUrls: ['./sing-up.component.scss']
})
export class SingUpComponent implements OnInit {

  constructor(private _userService: UserService,
    private _authorizationService: AuthorizationService,
    private _router: Router,
    public snackBar: MatSnackBar) { }

  ngOnInit() {
  }

  errors: string[] = [];

  form = new FormGroup({
    "username": new FormControl("", [Validators.required, Validators.minLength(4), Validators.pattern("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]),
    "email": new FormControl("", [Validators.required, Validators.email]),
    "password": new FormControl("", [Validators.required, Validators.minLength(8)]),
    "confirmPassword": new FormControl("", [Validators.required])
  });


  submit() {
    this.clearFormErrors();

    if (!this.form.valid) {
      this.showFormErrors(['Fill all fields']);
      return;
    }

    let formData = this.form.value;
    let postUser: IPostUser = {
      username: formData.username,
      email: formData.email,
      password: formData.password
    };

    this._userService.postUser(postUser).subscribe(
      result => {
        this.singInAndToHome(postUser.username, postUser.password);
      },
      (error: HttpErrorResponse) => {
        if (error.status == 400) {
          this.errors = <Array<string>>error.error;
        } else {
          this.showServerErrorSnackBar(error);
        }
      }
    );
  }

  protected singInAndToHome(userName: string, password: string) {
    let singInObject: ISingIn = {
      userName: userName,
      password: password
    };
    this._authorizationService.getJwtToken(singInObject).subscribe(
      result => {
        this._authorizationService.setJwtToken(result);
        this._userService.getCurrentUser().subscribe(
          result => {
            this._userService.currentUser.next(result);
            this._router.navigate(['/home']);
          },
          error => {
            this.showServerErrorSnackBar(error);
          }
        );
      },
      (error: HttpErrorResponse) => {
        this.showServerErrorSnackBar(error);
      }
    )
  }

  protected showFormErrors(errors: Array<string>) {
    this.clearFormErrors();
    errors.forEach(element => {
      this.errors.push(element);
    });
  }

  protected clearFormErrors() {
    this.errors.splice(0, this.errors.length);
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let conf = new MatSnackBarConfig();
    conf.extraClasses = ['danger-snack-bar'];
    conf.duration = 4000;
    this.snackBar.open('Server error ' + error.status + ': ' + error.statusText, null, conf);
    throw new Error('Server error ' + error.status + ': ' + error.statusText);
  }

}
