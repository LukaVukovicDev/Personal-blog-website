import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './shared/components/header/header';
import { Footer } from './shared/components/footer/footer';
import { BackToTop } from './shared/components/back-to-top/back-to-top';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Footer, BackToTop],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  private readonly auth = inject(AuthService);

  constructor() {
    // Ako postoji sačuvan token, učitaj profil korisnika na startu.
    this.auth.loadCurrentUser();
  }
}
