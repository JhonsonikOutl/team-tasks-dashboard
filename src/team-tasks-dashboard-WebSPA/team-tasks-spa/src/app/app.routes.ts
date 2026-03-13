import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: '',
    loadComponent: () => import('./layout/shell/shell.component').then(m => m.ShellComponent),
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'workload',
        loadComponent: () => import('./features/workload/workload.component').then(m => m.WorkloadComponent)
      },
      {
        path: 'projects-health',
        loadComponent: () => import('./features/projects-health/projects-health.component').then(m => m.ProjectsHealthComponent)
      },
      {
        path: 'risk',
        loadComponent: () => import('./features/risk/risk.component').then(m => m.RiskComponent)
      },
      {
        path: 'projects/:id',
        loadComponent: () => import('./features/project-tasks/project-tasks.component').then(m => m.ProjectTasksComponent)
      }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];