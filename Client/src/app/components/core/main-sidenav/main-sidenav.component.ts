import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { Route } from '@angular/compiler/src/core';
import { IUser } from '../../../models/user/user';
import { Subscription } from 'rxjs';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-main-sidenav',
  templateUrl: './main-sidenav.component.html',
  styleUrls: ['./main-sidenav.component.css']
})
export class MainSidenavComponent implements OnDestroy {

  mobileQuery: MediaQueryList;

  currentUser: IUser = null;

  currentUserSubscription: Subscription;

  private _mobileQueryListener: () => void;

  constructor(
    private changeDetectorRef: ChangeDetectorRef, 
    private media: MediaMatcher,
    private _userService: UserService) {

    this.mobileQuery = media.matchMedia('(max-width: 650px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
    this.currentUserSubscription = this._userService.currentUser.subscribe(
      result => this.currentUser = result
    );
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
    this.currentUserSubscription.unsubscribe();
  }


}
