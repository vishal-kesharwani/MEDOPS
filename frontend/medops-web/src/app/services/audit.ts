import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AuditLogDto {
  id: string;
  entityName: string;
  entityId: string;
  action: string;
  userId: string;
  userName: string;
  timestamp: string;
  oldValues?: string;
  newValues?: string;
  description?: string;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({ providedIn: 'root' })
export class Audit {
  private http = inject(HttpClient);
  private apiUrl = '/api/audit';

  getAll(params: { search?: string; sortBy?: string; sortDescending?: boolean; page?: number; pageSize?: number; status?: string } = {}): Observable<PaginatedResult<AuditLogDto>> {
    return this.http.get<PaginatedResult<AuditLogDto>>(this.apiUrl, { params: params as any });
  }

  getEntityLogs(entityType: string, entityId: string, page = 1, pageSize = 20): Observable<PaginatedResult<AuditLogDto>> {
    return this.http.get<PaginatedResult<AuditLogDto>>(`${this.apiUrl}/${entityType}/${entityId}`, { params: { page, pageSize } });
  }
}
