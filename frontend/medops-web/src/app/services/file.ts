import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface FileAttachmentDto {
  id: string;
  entityType: string;
  entityId: string;
  uploadedBy: string;
  fileName: string;
  originalFileName: string;
  contentType: string;
  fileSize: number;
  uploadedAt: string;
}

@Injectable({ providedIn: 'root' })
export class FileService {
  private http = inject(HttpClient);

  getAttachments(entityType: string, entityId: string): Observable<FileAttachmentDto[]> {
    return this.http.get<FileAttachmentDto[]>(`/api/files/${entityType}/${entityId}`);
  }

  upload(entityType: string, entityId: string, file: File): Observable<FileAttachmentDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<FileAttachmentDto>(`/api/files/${entityType}/${entityId}`, formData);
  }

  download(fileId: string): Observable<Blob> {
    return this.http.get(`/api/files/download/${fileId}`, { responseType: 'blob' });
  }

  delete(fileId: string): Observable<void> {
    return this.http.delete<void>(`/api/files/${fileId}`);
  }
}
