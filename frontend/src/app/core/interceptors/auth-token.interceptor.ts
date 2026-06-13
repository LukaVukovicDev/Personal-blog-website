import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenService } from '../services/token.service';

/** Dodaje "Authorization: Bearer <token>" na sve zahteve osim login/register. */
export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(TokenService).accessToken;
  const isAuthEntry = req.url.includes('/auth/login') || req.url.includes('/auth/register');

  if (token && !isAuthEntry) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }

  return next(req);
};
