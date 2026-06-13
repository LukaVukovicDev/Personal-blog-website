import { Injectable } from '@angular/core';

const ACCESS_KEY = 'blog.accessToken';
const REFRESH_KEY = 'blog.refreshToken';

/** Čuva JWT tokene u localStorage (aplikacija je SPA, izvršava se samo u browseru). */
@Injectable({ providedIn: 'root' })
export class TokenService {
  get accessToken(): string | null {
    return localStorage.getItem(ACCESS_KEY);
  }

  get refreshToken(): string | null {
    return localStorage.getItem(REFRESH_KEY);
  }

  setTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(ACCESS_KEY, accessToken);
    localStorage.setItem(REFRESH_KEY, refreshToken);
  }

  clear(): void {
    localStorage.removeItem(ACCESS_KEY);
    localStorage.removeItem(REFRESH_KEY);
  }
}
