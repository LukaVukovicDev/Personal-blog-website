import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserRole } from '../models/user.model';
import { AuthService } from '../services/auth.service';

/** Dozvoljava pristup samo korisnicima sa jednom od navedenih uloga. */
export function roleGuard(...roles: UserRole[]): CanActivateFn {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);
    const user = auth.currentUser();

    if (user && roles.includes(user.role)) {
      return true;
    }

    return router.createUrlTree(['/']);
  };
}
