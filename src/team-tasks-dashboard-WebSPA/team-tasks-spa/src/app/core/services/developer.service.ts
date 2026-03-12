import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Developer } from '../models/developer.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class DeveloperService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getActive(): Observable<Developer[]> {
    return this.http.get<Developer[]>(`${this.apiUrl}/developers`);
  }
}