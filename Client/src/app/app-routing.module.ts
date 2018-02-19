import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { SingInComponent } from './components/user/sing-in/sing-in.component';
import { PageNotFoundComponent } from './components/core/page-not-found/page-not-found.component';
import { SingUpComponent } from './components/user/sing-up/sing-up.component';
import { SettingsComponent } from './components/user/settings/settings.component';
import { AuthorizationGuard } from './guards/authorization.guard';
import { AddFilmComponent } from './components/films/add-film/add-film.component';
import { ListFilmsComponent } from './components/films/list-films/list-films.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'films', component: ListFilmsComponent },
  { path: 'add-film', component: AddFilmComponent, canActivate: [AuthorizationGuard]  },
  { path: 'settings', component: SettingsComponent, canActivate: [AuthorizationGuard] },
  { path: 'singin', component: SingInComponent },
  { path: 'singup', component: SingUpComponent },
  { path: '**', component: PageNotFoundComponent },
  { path: '404', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
