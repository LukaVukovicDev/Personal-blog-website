import { DatePipe } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, HostListener, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-header',
  imports: [RouterLink, DatePipe],
  templateUrl: './header.html',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class Header {
  readonly auth = inject(AuthService);
  readonly notifications = inject(NotificationService);
  private readonly router = inject(Router);

  /** Header / back-to-top become "active" once scrolled past 100px. */
  readonly scrolled = signal(false);
  /** Mobile navbar open state. */
  readonly navActive = signal(false);
  /** Notification dropdown open state. */
  readonly notificationsOpen = signal(false);

  @HostListener('window:scroll')
  onScroll(): void {
    this.scrolled.set(window.scrollY > 100);
  }

  toggleNav(): void {
    this.setNav(!this.navActive());
  }

  closeNav(): void {
    this.setNav(false);
  }

  toggleNotifications(): void {
    const open = !this.notificationsOpen();
    this.notificationsOpen.set(open);

    if (open && this.notifications.unreadCount() > 0) {
      this.notifications.markAllAsRead();
    }
  }

  closeNotifications(): void {
    this.notificationsOpen.set(false);
  }

  logout(): void {
    this.auth.logout();
    this.closeNav();
    this.router.navigateByUrl('/');
  }

  private setNav(open: boolean): void {
    this.navActive.set(open);
    document.body.classList.toggle('nav-active', open);
  }
}
