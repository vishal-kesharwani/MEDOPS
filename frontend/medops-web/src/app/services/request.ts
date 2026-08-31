import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RequestDto {
  id: string;
  title: string;
  description: string;
  status: string;
  createdBy: string;
  createdAt: string;
}

export interface CreateRequestDto {
  title: string;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class Request {
  private readonly apiUrl = '/api/requests';

  constructor(private http: HttpClient) {}

  getAll(): Observable<RequestDto[]> {
    return this.http.get<RequestDto[]>(this.apiUrl);
  }

  getById(id: string): Observable<RequestDto> {
    return this.http.get<RequestDto>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateRequestDto): Observable<RequestDto> {
    return this.http.post<RequestDto>(this.apiUrl, dto);
  }

  approve(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/approve`, {});
  }

  reject(id: string, comment: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/reject`, { comment });
  }

  cancel(id: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/cancel`, {});
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
