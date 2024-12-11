import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import * as LandingActions from './landing.actions';
import { LandingService } from '../landing.service';

@Injectable()
export class LandingEffects {

    createAuction$ = createEffect(() =>
        this.actions$.pipe(
            ofType(LandingActions.createAuction),
            mergeMap(({ auction }) =>
                this.landingService.createAuction(auction).pipe(
                    map((auction) => LandingActions.createAuctionSuccess({ auction: auction })),
                    catchError((error) => {
                        return of(LandingActions.createAuctionFailure({ error }));
                    })
                )
            )
        )
    );

    constructor(private actions$: Actions, private landingService: LandingService) { }
}
