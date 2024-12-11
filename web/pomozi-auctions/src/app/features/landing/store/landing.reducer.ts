import { createReducer, on } from '@ngrx/store';
import * as LandingActions from './landing.actions';
import { LandingState } from './landing.state';

export const initialState: LandingState = {
    auctions: [],
    loading: false,
    error: null,
    currentPage: 1,
    pageSize: 10,
    totalResults: 0,
};

export const landingReducer = createReducer(
    initialState,

    on(LandingActions.createAuction, (state) => ({
        ...state,
        loading: true,
    })),
    on(LandingActions.createAuctionSuccess, (state, { auction }) => ({
        ...state,
        auction,
        loading: false,
    })),
    on(LandingActions.createAuctionFailure, (state, { error }) => ({
        ...state,
        error,
        loading: false,
    })),
);
