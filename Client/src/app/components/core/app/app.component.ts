import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { AuthorizationService } from '../../../services/authorization-service.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MediaMatcher } from '@angular/cdk/layout';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

    title = 'app';

    mobileQuery: MediaQueryList;

    fillerNav = Array(50).fill(0).map((_, i) => `Nav Item ${i + 1} 11111`);

    fillerContent = Array(50).fill(0).map(() =>
        `Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
       labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
       laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in
       voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
       cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.`);

    private _mobileQueryListener: () => void;

    constructor(
        private _userService: UserService,
        private _authorizationService: AuthorizationService,
        public snackBar: MatSnackBar,
        changeDetectorRef: ChangeDetectorRef, media: MediaMatcher

    ) {
        this.mobileQuery = media.matchMedia('(max-width: 600px)');
        this._mobileQueryListener = () => changeDetectorRef.detectChanges();
        this.mobileQuery.addListener(this._mobileQueryListener)
    }

    ngOnInit(): void {
        if (AuthorizationService.accessTokenExistInLocalStorage()) {
            this._authorizationService.getNewAccessJwtToken().subscribe(
                result => {
                    this._authorizationService.setJwtToken(result);
                    this._authorizationService.runGetAccessjwtTokenSheduler();
                    this._userService.getCurrentUser().subscribe(
                        result => this._userService.currentUser.next(result),
                        error => {
                            this.snackBar.open('Failed to get user information. Authorization will be reset.', null, { duration: 4000 });
                            this._authorizationService.logout();
                        }
                    )
                }
            )
        }
    }
}