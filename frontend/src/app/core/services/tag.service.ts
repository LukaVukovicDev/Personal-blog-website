import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Tag } from '../models/post.model';

@Injectable({ providedIn: 'root' })
export class TagService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tags`;

  getAll(): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.baseUrl);
  }
}
