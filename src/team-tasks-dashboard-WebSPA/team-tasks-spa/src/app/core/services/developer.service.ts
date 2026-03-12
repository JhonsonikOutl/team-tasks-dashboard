import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Developer } from '../models/developer.model';

@Injectable({ providedIn: 'root' })
export class DeveloperService {
  private readonly apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  getActive(): Observable<Developer[]> {
    return this.http.get<Developer[]>(`${this.apiUrl}/developers`);
  }
}