import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', loadChildren: () => import('./features/landing').then(m => m.LANDING_ROUTES) },
    // {
    //     path: 'signup',
    // canActivate: [SomeGuard], 
    //     component: SignUpComponent
    // },
    { path: '**', redirectTo: '' }
];