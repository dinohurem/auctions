import { Routes } from '@angular/router';
import { PageLayoutComponent } from '../../core/page-layout/page-layout.component';

export const LANDING_ROUTES: Routes = [
    {
        path: '',
        component: PageLayoutComponent,
        children: [
            { path: '', loadComponent: () => import('./components/landing/landing.component').then(m => m.LandingComponent) },
        ],
    },
];