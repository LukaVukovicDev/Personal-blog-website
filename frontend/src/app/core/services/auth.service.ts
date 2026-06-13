import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest, User } from '../models/user.model';
import { TokenService } from './token.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokens = inject(TokenService);
  private readonly baseUrl = `${environment.apiUrl}/auth`;

  private readonly currentUserSig = signal<User | null>(null);

  /** Trenutno prijavljeni korisnik (ili null). */
  readonly currentUser = this.currentUserSig.asReadonly();
  readonly isAuthenticated = computed(() => this.currentUserSig() !== null);
  readonly isAdmin = computed(() => this.currentUserSig()?.role === 'Admin');
  readonly isAuthor = computed(() => {
    const role = this.currentUserSig()?.role;
    return role === 'Author' || role === 'Admin';
  });

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.baseUrl}/login`, request)
      .pipe(tap((response) => this.setSession(response)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.baseUrl}/register`, request)
      .pipe(tap((response) => this.setSession(response)));
  }

  logout(): void {
    const refreshToken = this.tokens.refreshToken;
    if (refreshToken) {
      this.http.post(`${this.baseUrl}/logout`, { refreshToken }).subscribe({ error: () => {} });
    }
    this.tokens.clear();
    this.currentUserSig.set(null);
  }

  /** Na startu aplikacije: ako postoji token, učitaj profil i popuni stanje. */
  loadCurrentUser(): void {
    if (!this.tokens.accessToken) {
      return;
    }

    this.http.get<User>(`${this.baseUrl}/me`).subscribe({
      next: (user) => this.currentUserSig.set(user),
      error: () => {
        this.tokens.clear();
        this.currentUserSig.set(null);
      },
    });
  }

  private setSession(response: AuthResponse): void {
    this.tokens.setTokens(response.accessToken, response.refreshToken);
    this.currentUserSig.set(response.user);
  }
}
