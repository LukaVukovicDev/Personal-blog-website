import { HttpClient } from '@angular/common/http';
import { effect, inject, Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { Comment } from '../models/comment.model';
import { AppNotification } from '../models/notification.model';
import { AuthService } from './auth.service';
import { TokenService } from './token.service';

/**
 * Upravlja jednom SignalR konekcijom ka NotificationsHub-u (`/hubs/notifications`).
 * Koristi se i za lične notifikacije (zvonce u headeru) i za live komentare na postu
 * (anonimni korisnici takođe mogu da se povežu i prate grupu posta).
 */
@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly http = inject(HttpClient);
  private readonly tokens = inject(TokenService);
  private readonly auth = inject(AuthService);
  private readonly baseUrl = `${environment.apiUrl}/notifications`;

  private connection: HubConnection | null = null;
  private connectionStarting: Promise<void> | null = null;

  readonly notifications = signal<AppNotification[]>([]);
  readonly unreadCount = signal(0);

  constructor() {
    this.connect();

    // Restartuj konekciju kad se promeni auth stanje, da bi server znao ko je korisnik.
    effect(() => {
      const authenticated = this.auth.isAuthenticated();

      if (authenticated) {
        this.loadInitial();
      } else {
        this.notifications.set([]);
        this.unreadCount.set(0);
      }

      this.reconnect();
    });
  }

  markAllAsRead(): void {
    this.http.post(`${this.baseUrl}/read-all`, {}).subscribe({
      next: () => {
        this.notifications.update((list) => list.map((n) => ({ ...n, isRead: true })));
        this.unreadCount.set(0);
      },
    });
  }

  /** Pridruži se grupi posta da bi se primali live komentari (`CommentAdded`). */
  async joinPostGroup(postId: string): Promise<void> {
    await this.connectionStarting;
    await this.connection?.invoke('JoinPostGroup', postId).catch(() => {});
  }

  async leavePostGroup(postId: string): Promise<void> {
    await this.connection?.invoke('LeavePostGroup', postId).catch(() => {});
  }

  /** Registruje listener za nove (odobrene) komentare; vraća funkciju za odjavu. */
  onCommentAdded(callback: (comment: Comment) => void): () => void {
    this.connection?.on('CommentAdded', callback);
    return () => this.connection?.off('CommentAdded', callback);
  }

  private loadInitial(): void {
    this.http.get<AppNotification[]>(this.baseUrl).subscribe({
      next: (list) => this.notifications.set(list),
      error: () => {},
    });

    this.http.get<number>(`${this.baseUrl}/unread-count`).subscribe({
      next: (count) => this.unreadCount.set(count),
      error: () => {},
    });
  }

  private connect(): void {
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.hubsUrl}/notifications`, {
        accessTokenFactory: () => this.tokens.accessToken ?? '',
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    this.connection.on('ReceiveNotification', (notification: AppNotification) => {
      this.notifications.update((list) => [notification, ...list].slice(0, 20));
      this.unreadCount.update((count) => count + 1);
    });

    this.connectionStarting = this.connection.start().catch(() => {});
  }

  private reconnect(): void {
    if (!this.connection || this.connection.state === HubConnectionState.Disconnected) {
      this.connectionStarting = this.connection?.start().catch(() => {}) ?? null;
      return;
    }

    this.connectionStarting = this.connection
      .stop()
      .then(() => this.connection?.start())
      .catch(() => {});
  }
}
