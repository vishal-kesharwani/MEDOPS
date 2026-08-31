import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard/auth-guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./components/login/login').then(m => m.Login) },
  { path: 'register', loadComponent: () => import('./components/register/register').then(m => m.Register) },
  {
    path: '',
    loadComponent: () => import('./components/layout/layout').then(m => m.Layout),
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', loadComponent: () => import('./components/dashboard/dashboard').then(m => m.Dashboard) },
      { path: 'studies', loadComponent: () => import('./components/studies/studies').then(m => m.Studies) },
      { path: 'sites', loadComponent: () => import('./components/sites/sites').then(m => m.Sites) },
      { path: 'tasks', loadComponent: () => import('./components/tasks/tasks').then(m => m.Tasks) },
      { path: 'requests', loadComponent: () => import('./components/requests/requests').then(m => m.Requests) },
      { path: 'departments', loadComponent: () => import('./components/departments/departments').then(m => m.Departments) },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: '' }
];
