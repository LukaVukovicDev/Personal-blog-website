import { Component, computed, DestroyRef, effect, inject, input, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { PostService } from '../../../core/services/post.service';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Comment } from '../../../core/models/comment.model';
import { PostDetail } from '../../../core/models/post.model';

@Component({
  selector: 'app-article',
  imports: [RouterLink, DatePipe, FormsModule],
  templateUrl: './article.html',
  styleUrl: './article.css',
})
export class Article {
  /** Bind-uje se iz rute `post/:slug`. */
  readonly slug = input<string>();

  private readonly postService = inject(PostService);
  private readonly sanitizer = inject(DomSanitizer);
  private readonly notifications = inject(NotificationService);
  private readonly destroyRef = inject(DestroyRef);
  readonly auth = inject(AuthService);

  private joinedPostId: string | null = null;

  readonly post = signal<PostDetail | null>(null);
  readonly loading = signal(true);
  readonly notFound = signal(false);

  readonly comments = signal<Comment[]>([]);
  readonly commentSubmitted = signal(false);
  readonly submitting = signal(false);
  commentBody = '';

  /** Sadržaj posta je naš (poverljiv) HTML iz baze, pa ga bezbedno renderujemo. */
  readonly safeContent = computed<SafeHtml>(() =>
    this.sanitizer.bypassSecurityTrustHtml(this.post()?.contentHtml ?? ''),
  );

  constructor() {
    effect(() => {
      const slug = this.slug();
      if (slug) {
        this.load(slug);
      }
    });

    // Live komentari: kad admin odobri komentar na ovom postu, dodaj ga odmah u listu.
    const unsubscribe = this.notifications.onCommentAdded((comment) => {
      this.comments.update((list) => [...list, comment]);
    });

    this.destroyRef.onDestroy(() => {
      unsubscribe();
      if (this.joinedPostId) {
        this.notifications.leavePostGroup(this.joinedPostId);
      }
    });
  }

  submitComment(): void {
    const post = this.post();
    const body = this.commentBody.trim();
    if (!post || body.length === 0) {
      return;
    }

    this.submitting.set(true);
    this.postService.addComment(post.id, { body }).subscribe({
      next: () => {
        this.commentBody = '';
        this.commentSubmitted.set(true);
        this.submitting.set(false);
      },
      error: () => this.submitting.set(false),
    });
  }

  private load(slug: string): void {
    this.loading.set(true);
    this.notFound.set(false);
    this.commentSubmitted.set(false);

    this.postService.getBySlug(slug).subscribe({
      next: (post) => {
        this.post.set(post);
        this.loading.set(false);
        this.loadComments(post.id);
      },
      error: () => {
        this.notFound.set(true);
        this.loading.set(false);
      },
    });
  }

  private loadComments(postId: string): void {
    this.postService.getComments(postId).subscribe({
      next: (comments) => this.comments.set(comments),
    });

    if (this.joinedPostId !== postId) {
      if (this.joinedPostId) {
        this.notifications.leavePostGroup(this.joinedPostId);
      }
      this.notifications.joinPostGroup(postId);
      this.joinedPostId = postId;
    }
  }
}
