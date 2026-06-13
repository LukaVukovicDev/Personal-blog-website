import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Category } from '../models/post.model';

export interface CreateCategoryRequest {
  name: string;
  description?: string;
}

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/categories`;

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl);
  }

  /** Samo Admin — kreira novu kategoriju. */
  create(request: CreateCategoryRequest): Observable<Category> {
    return this.http.post<Category>(this.baseUrl, request);
  }
}
