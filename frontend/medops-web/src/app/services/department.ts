import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DepartmentDto {
  id: string;
  name: string;
  description: string;
}

export interface CreateDepartmentDto {
  name: string;
  description: string;
}

@Injectable({ providedIn: 'root' })
export class Department {
  private readonly apiUrl = '/api/departments';

  constructor(private http: HttpClient) {}

  getAll(): Observable<DepartmentDto[]> {
    return this.http.get<DepartmentDto[]>(this.apiUrl);
  }

  getById(id: string): Observable<DepartmentDto> {
    return this.http.get<DepartmentDto>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateDepartmentDto): Observable<DepartmentDto> {
    return this.http.post<DepartmentDto>(this.apiUrl, dto);
  }

  update(id: string, dto: CreateDepartmentDto): Observable<DepartmentDto> {
    return this.http.put<DepartmentDto>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
