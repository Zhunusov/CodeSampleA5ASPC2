import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, PageEvent, MatPaginator, MatSnackBar, MatSnackBarConfig } from '@angular/material';
import { IFilm } from '../../../models/film';
import { FilmsService } from '../../../services/films.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-list-films',
  templateUrl: './list-films.component.html',
  styleUrls: ['./list-films.component.scss']
})
export class ListFilmsComponent {

  displayedColumns = ['cover', 'name', 'description', 'director', 'year'];
  dataSource: MatTableDataSource<IFilm>;

  filter: string = '';

  length = 100;
  pageSize = 5;
  pageSizeOptions = [5, 10, 25, 100];
  pageEvent: PageEvent;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private _filmsService: FilmsService,
    private _snackBar: MatSnackBar) {
  }

  ngAfterViewInit() {
    this._filmsService.get(this.pageSize).subscribe(
      films => {
        this.dataSource = new MatTableDataSource(films);
        this.dataSource.paginator = this.paginator;
        // this.dataSource.sort = this.sort;
        this._filmsService.getCount().subscribe(
          count => {
            this.length = count;
          },
          (error: HttpErrorResponse) => {
            this.showServerErrorSnackBar(error);
          }
        );
      },
      (error: HttpErrorResponse) => {
        this.showServerErrorSnackBar(error);
      }
    );
  }

  onPageEvent() {
    if (this.filter.length > 2) {
      this._filmsService.search(this.pageEvent.pageSize, this.filter, this.pageEvent.pageIndex * this.pageEvent.pageSize).subscribe(
        films => {
          this.dataSource = new MatTableDataSource(films);
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
    } else {
      this._filmsService.get(this.pageEvent.pageSize, this.pageEvent.pageIndex * this.pageEvent.pageSize).subscribe(
        films => {
          this.dataSource = new MatTableDataSource(films);
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
    }
  }

  applyFilter() {

    let filter = this.filter.trim(); // Remove whitespace
    filter = this.filter.toLowerCase(); // Datasource defaults to lowercase matches

    if (filter.length == 0) {
      this._filmsService.get(this.pageSize).subscribe(
        films => {
          this.dataSource = new MatTableDataSource(films);
          this._filmsService.getCount().subscribe(
            count => {
              this.length = count;
            },
            (error: HttpErrorResponse) => {
              this.showServerErrorSnackBar(error);
            }
          );
        },
        (error: HttpErrorResponse) => {
          this.showServerErrorSnackBar(error);
        }
      );
      return;
    }

    if (filter.length < 2) return;

    this._filmsService.search(this.pageSize, filter).subscribe(
      films => {
        this.dataSource = new MatTableDataSource(films);
        this.length = films.length;
      },
      (error: HttpErrorResponse) => {
        this.showServerErrorSnackBar(error);
      }
    );
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let conf = new MatSnackBarConfig();
    conf.extraClasses = ['danger-snack-bar'];
    conf.duration = 4000;
    this._snackBar.open('Server error ' + error.status + ': ' + error.statusText, null, conf);
    throw new Error('Server error ' + error.status + ': ' + error.statusText);
  }

}
