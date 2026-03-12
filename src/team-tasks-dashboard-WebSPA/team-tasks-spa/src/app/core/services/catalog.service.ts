import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CatalogItem } from '../models/catalog.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getTaskStatuses(): Observable<CatalogItem[]> {
    return this.http.get<CatalogItem[]>(`${this.apiUrl}/task-statuses`);
  }

  getTaskPriorities(): Observable<CatalogItem[]> {
    return this.http.get<CatalogItem[]>(`${this.apiUrl}/task-priorities`);
  }
}