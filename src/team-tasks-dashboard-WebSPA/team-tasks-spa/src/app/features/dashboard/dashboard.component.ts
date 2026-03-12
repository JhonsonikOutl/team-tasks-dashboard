import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DashboardService } from '../../core/services/dashboard.service';
import { DeveloperWorkloadDto, ProjectHealthDto, DelayRiskDto } from '../../core/models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  workload: DeveloperWorkloadDto[]  = [];
  projectHealth: ProjectHealthDto[] = [];
  delayRisk: DelayRiskDto[]         = [];

  get totalOpenTasks(): number {
    return this.projectHealth.reduce((sum, p) => sum + p.openTasks, 0);
  }

  get highRiskCount(): number {
    return this.delayRisk.filter(r => r.highRiskFlag === 1).length;
  }

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getDeveloperWorkload().subscribe(d => this.workload      = d);
    this.dashboardService.getProjectHealth().subscribe(d    => this.projectHealth  = d);
    this.dashboardService.getDelayRisk().subscribe(d        => this.delayRisk      = d);
  }
}