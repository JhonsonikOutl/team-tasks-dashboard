import { Component, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.scss'
})
export class ShellComponent {
  sidebarOpen = signal(window.innerWidth > 768);

  toggleSidebar() {
    this.sidebarOpen.update(v => !v);
  }

  readonly navItems = [
    { path: '/dashboard',       label: 'Inicio',                  icon: 'bi-house-fill' },
    { path: '/workload',        label: 'Carga por desarrollador', icon: 'bi-people-fill' },
    { path: '/projects-health', label: 'Estado por proyecto',     icon: 'bi-kanban-fill' },
    { path: '/risk',            label: 'Riesgo de retraso',       icon: 'bi-exclamation-triangle-fill' },
  ];
}