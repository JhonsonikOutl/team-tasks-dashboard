import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { DashboardService } from '../../core/services/dashboard.service';
import { ProjectHealthDto } from '../../core/models/dashboard.model';
import { DatatableComponent, TableColumn } from '../../shared/components/datatable/datatable.component';

@Component({
  selector: 'app-projects-health',
  standalone: true,
  imports: [CommonModule, RouterLink, DatatableComponent],
  templateUrl: './projects-health.component.html',
  styleUrl: './projects-health.component.scss'
})
export class ProjectsHealthComponent implements OnInit {
  projectHealth: ProjectHealthDto[] = [];

  @ViewChild('nameTpl', { static: true }) nameTpl!: TemplateRef<any>;

  columns: TableColumn[] = [];
  templates: { [key: string]: TemplateRef<any> } = {};

  rowClass = (row: ProjectHealthDto) =>
    row.openTasks > row.completedTasks ? 'row-warn' : '';

  constructor(
    private dashboardService: DashboardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.columns = [
      { key: 'projectName',    label: 'Proyecto',    template: 'projectName' },
      { key: 'clientName',     label: 'Cliente' },
      { key: 'totalTasks',     label: 'Total' },
      { key: 'openTasks',      label: 'Abiertas' },
      { key: 'completedTasks', label: 'Completadas' },
    ];
    this.templates = { projectName: this.nameTpl };

    this.dashboardService.getProjectHealth().subscribe(data => {
      this.projectHealth = data;
    });
  }

  onProjectClick(row: ProjectHealthDto): void {
    this.router.navigate(['/projects', row.projectId]);
  }
}