import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface StudyDto {
  id: string;
  name: string;
  description: string;
  status: string;
  startDate?: string;
  endDate?: string;
  createdBy: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateStudyDto {
  name: string;
  description: string;
  startDate?: string;
  endDate?: string;
}

@Injectable({ providedIn: 'root' })
export class Study {
  private readonly apiUrl = '/api/studies';

  constructor(private http: HttpClient) {}

  getAll(): Observable<StudyDto[]> {
    return this.http.get<StudyDto[]>(this.apiUrl);
  }

  getById(id: string): Observable<StudyDto> {
    return this.http.get<StudyDto>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateStudyDto): Observable<StudyDto> {
    return this.http.post<StudyDto>(this.apiUrl, dto);
  }

  update(id: string, dto: CreateStudyDto): Observable<StudyDto> {
    return this.http.put<StudyDto>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
