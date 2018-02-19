import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthorizationService } from '../../../services/authorization-service.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { ISingIn } from '../../../models/authorization/singIn';
import { HttpErrorResponse } from '@angular/common/http';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';

@Component({
  selector: 'app-sing-in',
  templateUrl: './sing-in.component.html',
  styleUrls: ['./sing-in.component.scss']
})
export class SingInComponent implements OnInit {

  constructor(private _authorizationService: AuthorizationService,
    private _router: Router,
    private _userService: UserService,
    private _snackBarsService: SnackBarsServiceService) { }

  errors: string[] = [];

  form = new FormGroup({
    "userName": new FormControl("", [Validators.required, Validators.minLength(4), Validators.pattern("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]),
    "password": new FormControl("", [Validators.required, Validators.minLength(8)]),
  });

  ngOnInit() {
  }

  submit() {
    this.clearFormErrors();

    if (!this.form.valid) {
      this.showFormErrors(['Fill all fields']);
      return;
    }

    let formData = this.form.value;

    let singInObject: ISingIn = {
      userName: formData.userName,
      password: formData.password
    };
    this._authorizationService.getJwtToken(singInObject).subscribe(
      result => {
        this._authorizationService.setJwtToken(result);
        this._userService.getCurrentUser().subscribe(
          result => this._userService.currentUser.next(result),
          (error: HttpErrorResponse) => {
            this.showServerErrorSnackBar(error);
          }
        );
        this._authorizationService.runGetAccessjwtTokenSheduler();
        this._router.navigate(['/home']);
      },
      (error: HttpErrorResponse) => {
        console.log(error);
        if (error.status == 400) {
          this.errors.push("Invalid password.");
          return;
        }
        if (error.status == 404) {
          this.errors.push("User with received email or name not found.");
          return;
        }
        this.showServerErrorSnackBar(error);
      }
    )
  }

  protected clearFormErrors() {
    this.errors.splice(0, this.errors.length);
  }

  protected showFormErrors(errors: Array<string>) {
    this.clearFormErrors();
    errors.forEach(element => {
      this.errors.push(element);
    });
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString)
  }

}
