import { Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AdminCommentService, CommentStatus } from '../../../core/services/admin-comment.service';
import { CommentAdmin } from '../../../core/models/comment.model';

interface StatusFilter {
  label: string;
  value: CommentStatus | undefined;
}

@Component({
  selector: 'app-comment-moderation',
  imports: [DatePipe, RouterLink],
  templateUrl: './comment-moderation.html',
  styleUrl: './comment-moderation.css',
})
export class CommentModeration {
  private readonly commentService = inject(AdminCommentService);

  readonly comments = signal<CommentAdmin[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly activeFilter = signal<CommentStatus | undefined>('Pending');

  readonly filters: StatusFilter[] = [
    { label: 'Na čekanju', value: 'Pending' },
    { label: 'Odobreni', value: 'Approved' },
    { label: 'Spam', value: 'Spam' },
    { label: 'Svi', value: undefined },
  ];

  constructor() {
    this.load();
  }

  setFilter(filter: CommentStatus | undefined): void {
    this.activeFilter.set(filter);
    this.load();
  }

  approve(comment: CommentAdmin): void {
    this.moderate(comment, 'Approved');
  }

  markSpam(comment: CommentAdmin): void {
    this.moderate(comment, 'Spam');
  }

  deleteComment(comment: CommentAdmin): void {
    if (!confirm('Obrisati ovaj komentar?')) {
      return;
    }

    this.commentService.delete(comment.id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Brisanje komentara nije uspelo.'),
    });
  }

  private moderate(comment: CommentAdmin, status: CommentStatus): void {
    this.commentService.moderate(comment.id, status).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Izmena statusa nije uspela.'),
    });
  }

  private load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.commentService.getAll(this.activeFilter()).subscribe({
      next: (comments) => {
        this.comments.set(comments);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Učitavanje komentara nije uspelo.');
        this.loading.set(false);
      },
    });
  }
}
