import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CommentAdmin } from '../models/comment.model';

export type CommentStatus = 'Pending' | 'Approved' | 'Spam';

@Injectable({ providedIn: 'root' })
export class AdminCommentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/comments`;

  /** Samo Admin — svi komentari, opciono filtrirani po statusu. */
  getAll(status?: CommentStatus): Observable<CommentAdmin[]> {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<CommentAdmin[]>(this.baseUrl, { params });
  }

  moderate(id: string, status: CommentStatus): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/moderate`, { status });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
