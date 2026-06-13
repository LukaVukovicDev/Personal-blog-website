import { Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { PostService } from '../../../core/services/post.service';
import { PostListItem } from '../../../core/models/post.model';

@Component({
  selector: 'app-post-list',
  imports: [RouterLink, DatePipe, FormsModule],
  templateUrl: './post-list.html',
  styleUrl: './post-list.css',
})
export class PostList {
  private readonly postService = inject(PostService);

  private static readonly PAGE_SIZE = 10;

  readonly posts = signal<PostListItem[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  readonly page = signal(1);
  readonly totalPages = signal(1);

  /** Vezano za pretragu preko [(ngModel)]; pretraga se pokreće na submit forme. */
  search = '';

  constructor() {
    this.load();
  }

  submitSearch(): void {
    this.page.set(1);
    this.load();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages()) {
      return;
    }
    this.page.set(page);
    this.load();
  }

  togglePublish(post: PostListItem): void {
    if (post.status === 'Published') {
      return;
    }

    this.postService.publish(post.id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Objavljivanje nije uspelo.'),
    });
  }

  deletePost(post: PostListItem): void {
    if (!confirm(`Obrisati post "${post.title}"?`)) {
      return;
    }

    this.postService.delete(post.id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Brisanje nije uspelo.'),
    });
  }

  private load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.postService
      .getManaged({ search: this.search || undefined, page: this.page(), pageSize: PostList.PAGE_SIZE })
      .subscribe({
        next: (result) => {
          this.posts.set(result.items);
          this.totalPages.set(result.totalPages);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Učitavanje postova nije uspelo.');
          this.loading.set(false);
        },
      });
  }
}
