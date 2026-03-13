import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../core/services/dashboard.service';
import { DelayRiskDto } from '../../core/models/dashboard.model';
import { DatatableComponent, TableColumn } from '../../shared/components/datatable/datatable.component';

@Component({
  selector: 'app-risk',
  standalone: true,
  imports: [CommonModule, DatatableComponent],
  templateUrl: './risk.component.html',
  styleUrl: './risk.component.scss'
})
export class RiskComponent implements OnInit {
  delayRisk: DelayRiskDto[] = [];

  @ViewChild('riskTpl', { static: true }) riskTpl!: TemplateRef<any>;

  columns: TableColumn<DelayRiskDto>[] = [];
  templates: { [key: string]: TemplateRef<any> } = {};

  rowClass = (row: DelayRiskDto) =>
    row.highRiskFlag === 1 ? 'row-danger' : '';

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.columns = [
      { key: 'developerName',           label: 'Desarrollador' },
      { key: 'openTasksCount',          label: 'Tareas abiertas' },
      { key: 'avgDelayDays',            label: 'Días retraso prom.' },
      { key: 'nearestDueDate',          label: 'Próx. vencimiento' },
      { key: 'latestDueDate',           label: 'Último vencimiento' },
      { key: 'predictedCompletionDate', label: 'Completitud estimada' },
      { key: 'highRiskFlag',            label: 'Riesgo', template: 'highRiskFlag' },
    ];
    this.templates = { highRiskFlag: this.riskTpl };

    this.dashboardService.getDelayRisk().subscribe(data => {
      this.delayRisk = data;
    });
  }
}