import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import * as AuthActions from './auth.actions';
import { AuthService } from '../services/auth.service';
import { CookieService } from '../services/cookie.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthEffects {

    login$ = createEffect(() =>
        this.actions$.pipe(
            ofType(AuthActions.login),
            mergeMap(({ loginRequest }) =>
                this.authService.login(loginRequest).pipe(
                    map(response => {
                        this.cookieService.setCookie('accessToken', response.accessToken);
                        return AuthActions.loginSuccess({ token: response.accessToken });
                    }),
                    catchError(error => {
                        return of(AuthActions.loginFailure({ error }));
                    })
                )
            )
        ));

    constructor(
        private actions$: Actions,
        private authService: AuthService,
        private cookieService: CookieService,
        private router: Router) { }
}
