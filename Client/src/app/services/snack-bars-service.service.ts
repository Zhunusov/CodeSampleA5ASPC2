import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class SnackBarsServiceService {

  private static _duration = 4000;

  constructor(public snackBar: MatSnackBar) { }

  showError(error: string, throwError: boolean = false) {
    let conf = new MatSnackBarConfig();
    conf.extraClasses = ['danger-snack-bar'];
    conf.duration = SnackBarsServiceService._duration;
    this.snackBar.open(error, null, conf);
    if(throwError){
      throw new Error(error);
    }
  }

  showMessage(message: string){
    let conf = new MatSnackBarConfig();
    conf.extraClasses = ['message-snack-bar'];
    conf.duration = SnackBarsServiceService._duration;
    this.snackBar.open(message, null, conf);
  }

  showSuccess(message: string) {
    let conf = new MatSnackBarConfig();
    conf.extraClasses = ['success-snack-bar'];
    conf.duration = SnackBarsServiceService._duration;
    this.snackBar.open(message, null, conf);
  }

}
