import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CommentDto {
  id: string;
  entityType: string;
  entityId: string;
  userId: string;
  userName: string;
  content: string;
  createdAt: string;
  updatedAt?: string;
}

@Injectable({ providedIn: 'root' })
export class Comment {
  private http = inject(HttpClient);

  getComments(entityType: string, entityId: string): Observable<CommentDto[]> {
    return this.http.get<CommentDto[]>(`/api/comments/${entityType}/${entityId}`);
  }

  addComment(entityType: string, entityId: string, content: string): Observable<CommentDto> {
    return this.http.post<CommentDto>(`/api/comments/${entityType}/${entityId}`, { content });
  }

  updateComment(id: string, content: string): Observable<CommentDto> {
    return this.http.put<CommentDto>(`/api/comments/${id}`, { content });
  }

  deleteComment(id: string): Observable<void> {
    return this.http.delete<void>(`/api/comments/${id}`);
  }
}
