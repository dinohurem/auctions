import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideStore } from '@ngrx/store';
import { reducers } from './reducers'; // Import the regular reducers
import { provideEffects } from '@ngrx/effects';
import { AuthEffects } from './core/auth/store/auth.effects';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { apiInterceptor } from './core/interceptors/api.interceptor';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { LandingEffects } from './features/landing/store/landing.effects';
import { localStorageSync } from 'ngrx-store-localstorage';

// Function to sync the state to localStorage
export function localStorageSyncReducer(reducer: any): any {
  return localStorageSync({ keys: ['auth'], rehydrate: true })(reducer);
}

// Define metaReducers to include localStorageSyncReducer
const metaReducers = [localStorageSyncReducer];

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes,
    ),
    provideStore(reducers, { metaReducers }),
    provideEffects([
      AuthEffects,
      LandingEffects,
    ]),
    provideHttpClient(withInterceptors([apiInterceptor])),
    provideStoreDevtools({
      maxAge: 25,
      logOnly: environment.production,
      trace: true,
      traceLimit: 25,
      features: {
        pause: true,
        lock: true,
        persist: true,
        export: true,
        import: 'custom',
        jump: true,
        skip: true,
        reorder: true,
        dispatch: true,
        test: true,
      }
    }),

  ]
};

