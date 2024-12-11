import { createAction, props } from '@ngrx/store';
import { LoginRequest } from '../../../shared/models/login.model';

export const login = createAction('[Auth] Login', props<{ loginRequest: LoginRequest }>());
export const loginSuccess = createAction('[Auth] Login Success', props<{ token: string }>());
export const loginFailure = createAction('[Auth] Login Failure', props<{ error: any }>());