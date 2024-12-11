import { createFeatureSelector, createSelector } from '@ngrx/store';
import { LandingState } from './landing.state';

export const landingState = createFeatureSelector<LandingState>('landing');

export const selectAuctions = createSelector(
    landingState,
    (state) => state.auctions
);

export const selectLoading = createSelector(
    landingState,
    (state) => state.loading
);