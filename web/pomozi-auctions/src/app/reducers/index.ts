import { isDevMode } from '@angular/core';
import {
  ActionReducerMap,
  MetaReducer
} from '@ngrx/store';
import { AuthState } from '../core/auth/store/auth.state';
import { authReducer } from '../core/auth/store/auth.reducer';
import { LandingState } from '../features/landing/store/landing.state';
import { landingReducer } from '../features/landing/store/landing.reducer';

export interface State {
  auth: AuthState;
  landing: LandingState;
}

export const reducers: ActionReducerMap<State> = {
  auth: authReducer,
  landing: landingReducer,
};

export const metaReducers: MetaReducer<State>[] = isDevMode() ? [] : [];