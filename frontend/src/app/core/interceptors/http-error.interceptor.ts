import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { TokenService } from '../services/token.service';

/** Na 401 (van auth ruta) čisti sesiju; greške prosleđuje dalje komponentama. */
export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const tokens = inject(TokenService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('/auth/')) {
        tokens.clear();
      }
      return throwError(() => error);
    }),
  );
};
