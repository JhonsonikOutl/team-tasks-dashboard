import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../core/services/dashboard.service';
import { DeveloperWorkloadDto } from '../../core/models/dashboard.model';
import { DatatableComponent } from '../../shared/components/datatable/datatable.component';
import { TableColumn } from '../../shared/components/datatable/datatable.component';

@Component({
  selector: 'app-workload',
  standalone: true,
  imports: [CommonModule, DatatableComponent],
  templateUrl: './workload.component.html',
  styleUrl: './workload.component.scss'
})
export class WorkloadComponent implements OnInit {
  workload: DeveloperWorkloadDto[] = [];

  columns: TableColumn[] = [
    { key: 'developerName',              label: 'Desarrollador',        sortable: true },
    { key: 'openTasksCount',             label: 'Tareas abiertas',      sortable: true },
    { key: 'averageEstimatedComplexity', label: 'Complejidad promedio', sortable: true },
  ];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getDeveloperWorkload().subscribe(data => {
      this.workload = data;
    });
  }
}