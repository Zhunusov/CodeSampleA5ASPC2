import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthorizationService } from '../../../services/authorization-service.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IUser } from '../../../models/user/user';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.scss']
})
export class UserMenuComponent implements OnInit, OnDestroy  {

  currentUser: IUser = null;

  currentUserSubscription: Subscription;

  constructor(
    private _authorizationService: AuthorizationService,
    private _userService: UserService,
    private _router: Router) { }

  logout() {
    this._authorizationService.logout();
  }

  ngOnInit() {
    this.currentUserSubscription = this._userService.currentUser.subscribe(
      result => this.currentUser = result
    );
  }

  ngOnDestroy(): void {
    this.currentUserSubscription.unsubscribe();
  }
}
