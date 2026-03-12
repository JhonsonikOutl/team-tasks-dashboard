import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Project } from '../models/project.model';
import { Task } from '../models/task.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiUrl}/projects`);
  }

  getById(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.apiUrl}/projects/${id}`);
  }

  getTasksByProject(
    projectId: number,
    statusId?: number | null,
    assigneeId?: number | null,
    page: number = 1,
    pageSize: number = 3
  ): Observable<Task[]> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (statusId != null) params = params.set('statusId', statusId);
    if (assigneeId != null) params = params.set('assigneeId', assigneeId);

    return this.http.get<Task[]>(`${this.apiUrl}/projects/${projectId}/tasks`, { params });
  }
}
