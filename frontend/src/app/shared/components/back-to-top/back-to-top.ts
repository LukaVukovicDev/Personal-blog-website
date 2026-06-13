import { Component, CUSTOM_ELEMENTS_SCHEMA, HostListener, signal } from '@angular/core';

@Component({
  selector: 'app-back-to-top',
  template: `
    <button
      type="button"
      class="back-top-btn"
      [class.active]="active()"
      aria-label="back to top"
      (click)="scrollToTop()"
    >
      <ion-icon name="arrow-up-outline" aria-hidden="true"></ion-icon>
    </button>
  `,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class BackToTop {
  readonly active = signal(false);

  @HostListener('window:scroll')
  onScroll(): void {
    this.active.set(window.scrollY > 100);
  }

  scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
