import { Route } from '@angular/router';

export const routes: Route[] = [
  {
    path: 'home',
    loadChildren: () => import('./domains/website/routes'),
  },
  {
    path: 'auth',
    loadChildren: () => import('./domains/auth/routes'),
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'admin',
  },
  {
    path: 'admin',
    loadChildren: () => import('./domains/admin/routes'),
  },
  {
    path: 'coming-soon',
    loadChildren: () => import('./domains/coming-soon/routes'),
  },
];
