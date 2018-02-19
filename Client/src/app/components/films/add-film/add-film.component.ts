import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { FilmsService } from '../../../services/films.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IFilm } from '../../../models/film';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-film',
  templateUrl: './add-film.component.html',
  styleUrls: ['./add-film.component.scss']
})
export class AddFilmComponent {

  errors: string[] = [];

  constructor(
    private _filmsService: FilmsService,
    private _snackBar: MatSnackBar,
    private _router: Router) { }

  form = new FormGroup({
    "name": new FormControl("", [Validators.required]),
    "coverUrl": new FormControl("", [Validators.required, Validators.pattern("^(https?://)?(www\\.)?([-a-z0-9]{1,63}\\.)*?[a-z0-9][-a-z0-9]{0,61}[a-z0-9]\\.[a-z]{2,6}(/[-\\w@\\+\\.,~#\\?&/=%]*)?$")]),
    "description": new FormControl("", [Validators.required]),
    "director": new FormControl("", [Validators.required]),
    "year": new FormControl("", [Validators.required, Validators.min(1895), Validators.max((new Date()).getFullYear())])
  });

  submit() {
    this.clearFormErrors();

    if (!this.form.valid) {
      this.showFormErrors(['Fill in required fields']);
      return;
    }

    let formData = this.form.value;
    let film: IFilm = {
      name: formData.name,
      coverUrl: formData.coverUrl,
      description: formData.description,
      director: formData.director,
      year: formData.year,
      id: 0
    };

    this._filmsService.post(film).subscribe(
      result => {
        this.showSuccessSnackBar("Added")
        this.GoToShopList()
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

  protected GoToShopList() {
    this._router.navigate(['/films']);
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

  protected showSuccessSnackBar(message: string) {
    let conf = new MatSnackBarConfig();
    conf.extraClasses = ['success-snack-bar'];
    conf.duration = 4000;
    this._snackBar.open(message, null, conf);
  }

  protected showServerErrorSnackBar(error: HttpErrorResponse) {
    let conf = new MatSnackBarConfig();
    conf.extraClasses = ['danger-snack-bar'];
    conf.duration = 4000;
    this._snackBar.open('Server error ' + error.status + ': ' + error.statusText, null, conf);
    throw new Error('Server error ' + error.status + ': ' + error.statusText);
  }

}
