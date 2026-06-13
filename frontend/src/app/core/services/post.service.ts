import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Comment, CreateComment } from '../models/comment.model';
import { PagedResult } from '../models/paged-result.model';
import {
  CreatePostRequest,
  PostDetail,
  PostListItem,
  PostQuery,
  UpdatePostRequest,
} from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class PostService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/posts`;

  getPosts(query: PostQuery = {}): Observable<PagedResult<PostListItem>> {
    let params = new HttpParams();
    if (query.search) {
      params = params.set('search', query.search);
    }
    if (query.category) {
      params = params.set('category', query.category);
    }
    if (query.tag) {
      params = params.set('tag', query.tag);
    }
    if (query.page) {
      params = params.set('page', query.page);
    }
    if (query.pageSize) {
      params = params.set('pageSize', query.pageSize);
    }

    return this.http.get<PagedResult<PostListItem>>(this.baseUrl, { params });
  }

  getBySlug(slug: string): Observable<PostDetail> {
    return this.http.get<PostDetail>(`${this.baseUrl}/${slug}`);
  }

  getComments(postId: string): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.baseUrl}/${postId}/comments`);
  }

  addComment(postId: string, comment: CreateComment): Observable<Comment> {
    return this.http.post<Comment>(`${this.baseUrl}/${postId}/comments`, comment);
  }

  // ── Admin / Author upravljanje postovima ──────────────────────

  /** Lista za admin/author panel — uključuje i nacrte (draft) postove. */
  getManaged(query: PostQuery = {}): Observable<PagedResult<PostListItem>> {
    let params = new HttpParams();
    if (query.search) {
      params = params.set('search', query.search);
    }
    if (query.page) {
      params = params.set('page', query.page);
    }
    if (query.pageSize) {
      params = params.set('pageSize', query.pageSize);
    }

    return this.http.get<PagedResult<PostListItem>>(`${this.baseUrl}/manage`, { params });
  }

  getById(id: string): Observable<PostDetail> {
    return this.http.get<PostDetail>(`${this.baseUrl}/manage/${id}`);
  }

  create(request: CreatePostRequest): Observable<PostDetail> {
    return this.http.post<PostDetail>(this.baseUrl, request);
  }

  update(id: string, request: UpdatePostRequest): Observable<PostDetail> {
    return this.http.put<PostDetail>(`${this.baseUrl}/${id}`, request);
  }

  publish(id: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/publish`, {});
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
