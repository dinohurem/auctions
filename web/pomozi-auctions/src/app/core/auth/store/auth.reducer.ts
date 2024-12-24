import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './auth.actions';
import { AuthState } from './auth.state';

const initialState: AuthState = {
    loading: false,
    token: null,
    error: null,
};

export const authReducer = createReducer(
    initialState,
    on(AuthActions.login, state => ({
        ...state,
        loading: true
    })),
    on(AuthActions.loginSuccess, (state, { token }) => {
        return {
            ...state,
            loading: false,
            token,
        };
    }),
    on(AuthActions.loginFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),
);
