import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface TaskDto {
  id: string;
  title: string;
  description: string;
  status: string;
  priority: string;
  assignedTo: string;
  createdBy: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTaskDto {
  title: string;
  description: string;
  assignedTo: string;
  studyId?: string;
}

@Injectable({ providedIn: 'root' })
export class Task {
  private readonly apiUrl = '/api/tasks';

  constructor(private http: HttpClient) {}

  getAll(): Observable<TaskDto[]> {
    return this.http.get<TaskDto[]>(this.apiUrl);
  }

  getById(id: string): Observable<TaskDto> {
    return this.http.get<TaskDto>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateTaskDto): Observable<TaskDto> {
    return this.http.post<TaskDto>(this.apiUrl, dto);
  }

  start(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/start`, {});
  }

  complete(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/complete`, {});
  }

  cancel(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/cancel`, {});
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
