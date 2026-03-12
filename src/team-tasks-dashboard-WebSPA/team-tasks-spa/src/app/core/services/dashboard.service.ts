import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DeveloperWorkloadDto, ProjectHealthDto, DelayRiskDto } from '../models/dashboard.model';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  getDeveloperWorkload(): Observable<DeveloperWorkloadDto[]> {
    return this.http.get<DeveloperWorkloadDto[]>(`${this.apiUrl}/dashboard/developer-workload`);
  }

  getProjectHealth(): Observable<ProjectHealthDto[]> {
    return this.http.get<ProjectHealthDto[]>(`${this.apiUrl}/dashboard/project-health`);
  }

  getDelayRisk(): Observable<DelayRiskDto[]> {
    return this.http.get<DelayRiskDto[]>(`${this.apiUrl}/dashboard/developer-delay-risk`);
  }
}