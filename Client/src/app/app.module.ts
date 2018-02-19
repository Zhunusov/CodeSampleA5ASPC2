// Angular
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';

// Material


import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatFormFieldModule,

  ErrorStateMatcher,
  ShowOnDirtyErrorStateMatcher,
} from '@angular/material';

// Components
import { FooterComponent } from './components/core/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { SingInComponent } from './components/user/sing-in/sing-in.component';
import { SingUpComponent } from './components/user/sing-up/sing-up.component';
import { PageNotFoundComponent } from './components/core/page-not-found/page-not-found.component';
import { AppComponent } from './components/core/app/app.component';
import { UserMenuComponent } from './components/user/user-menu/user-menu.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { ListFilmsComponent } from './components/films/list-films/list-films.component';
import { AddFilmComponent } from './components/films/add-film/add-film.component';

// Validators
import { EqualValidator } from './validators/equal.validator';

// Servises
import { AuthorizationService } from './services/authorization-service.service';
import { UserService } from './services/user.service';
import { SnackBarsServiceService } from './services/snack-bars-service.service';
import { FilmsService } from './services/films.service';

// Interceptors
import { AuthenticationInterceptor } from './interceptors/authentication.interceptor';
import { JwtInterceptor } from './interceptors/jwtInterceptor.interceptor';


// Guards
import { AuthorizationGuard } from './guards/authorization.guard';
import { MainSidenavComponent } from './components/core/main-sidenav/main-sidenav.component';


@NgModule({
  declarations: [
    // Components
    AppComponent,
    FooterComponent,
    HomeComponent,
    SingInComponent,
    SingUpComponent,
    PageNotFoundComponent,
    UserMenuComponent,
    SettingsComponent,
    ListFilmsComponent,
    AddFilmComponent,
    MainSidenavComponent,
    // Validators
    EqualValidator,
  ],
  imports: [
    // Bootstrap

    // Material
    MatFormFieldModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatStepperModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,

    BrowserAnimationsModule,

    BrowserModule,
    AppRoutingModule,

    FormsModule,
    ReactiveFormsModule,

    HttpClientModule
  ],
  providers: [
    {
      provide: ErrorStateMatcher,
      useClass: ShowOnDirtyErrorStateMatcher
    },
    // Services
    UserService,
    FilmsService,
    SnackBarsServiceService,
    AuthorizationService,
    // Interceptors
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    // Guards
    AuthorizationGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
