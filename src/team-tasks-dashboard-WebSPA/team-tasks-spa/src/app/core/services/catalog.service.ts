import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, shareReplay, tap } from 'rxjs';
import { CatalogItem } from '../models/catalog.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly apiUrl = `${environment.apiUrl}/catalog`;

  private statusMap: Record<string, CatalogItem> = {};
  private priorityMap: Record<string, CatalogItem> = {};

  private statuses$: Observable<CatalogItem[]> | null = null;
  private priorities$: Observable<CatalogItem[]> | null = null;

  constructor(private http: HttpClient) {}

  getTaskStatuses(): Observable<CatalogItem[]> {
    if (!this.statuses$) {
      this.statuses$ = this.http.get<CatalogItem[]>(`${this.apiUrl}/task-statuses`).pipe(
        tap(items => items.forEach(i => this.statusMap[i.description] = i)),
        shareReplay(1)
      );
    }
    return this.statuses$;
  }

  getTaskPriorities(): Observable<CatalogItem[]> {
    if (!this.priorities$) {
      this.priorities$ = this.http.get<CatalogItem[]>(`${this.apiUrl}/task-priorities`).pipe(
        tap(items => items.forEach(i => this.priorityMap[i.description] = i)),
        shareReplay(1)
      );
    }
    return this.priorities$;
  }

  getStatusColor(description: string): string {
    return this.statusMap[description]?.colorClass ?? 'secondary';
  }

  getPriorityColor(description: string): string {
    return this.priorityMap[description]?.colorClass ?? 'secondary';
  }

  preload(): Observable<[CatalogItem[], CatalogItem[]]> {
    return new Observable(observer => {
      Promise.all([
        this.getTaskStatuses().toPromise(),
        this.getTaskPriorities().toPromise()
      ]).then(([statuses, priorities]) => {
        observer.next([statuses!, priorities!]);
        observer.complete();
      });
    });
  }
}