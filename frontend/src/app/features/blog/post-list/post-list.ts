import { CUSTOM_ELEMENTS_SCHEMA, Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { PostService } from '../../../core/services/post.service';
import { CategoryService } from '../../../core/services/category.service';
import { TagService } from '../../../core/services/tag.service';
import { Category, PostListItem, Tag } from '../../../core/models/post.model';

const PAGE_SIZE = 9;

@Component({
  selector: 'app-post-list',
  imports: [RouterLink, DatePipe, FormsModule],
  templateUrl: './post-list.html',
  styleUrl: './post-list.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class PostList {
  private readonly postService = inject(PostService);
  private readonly categoryService = inject(CategoryService);
  private readonly tagService = inject(TagService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly posts = signal<PostListItem[]>([]);
  readonly categories = signal<Category[]>([]);
  readonly tags = signal<Tag[]>([]);

  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  readonly page = signal(1);
  readonly totalPages = signal(1);
  readonly totalCount = signal(0);

  search = '';
  selectedCategory = '';
  selectedTag = '';

  readonly pageNumbers = computed(() =>
    Array.from({ length: this.totalPages() }, (_, i) => i + 1),
  );

  constructor() {
    this.categoryService.getAll().subscribe({
      next: (categories) => this.categories.set(categories),
      error: () => {},
    });

    this.tagService.getAll().subscribe({
      next: (tags) => this.tags.set(tags),
      error: () => {},
    });

    this.route.queryParamMap.pipe(takeUntilDestroyed()).subscribe((params) => {
      this.search = params.get('search') ?? '';
      this.selectedCategory = params.get('category') ?? '';
      this.selectedTag = params.get('tag') ?? '';
      this.page.set(Number(params.get('page')) || 1);

      this.loadPosts();
    });
  }

  applyFilters(): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        search: this.search || null,
        category: this.selectedCategory || null,
        tag: this.selectedTag || null,
        page: null,
      },
      queryParamsHandling: 'merge',
    });
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages() || page === this.page()) {
      return;
    }

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page },
      queryParamsHandling: 'merge',
    });
  }

  private loadPosts(): void {
    this.loading.set(true);
    this.error.set(null);

    this.postService
      .getPosts({
        search: this.search || undefined,
        category: this.selectedCategory || undefined,
        tag: this.selectedTag || undefined,
        page: this.page(),
        pageSize: PAGE_SIZE,
      })
      .subscribe({
        next: (result) => {
          this.posts.set(result.items);
          this.totalPages.set(result.totalPages);
          this.totalCount.set(result.totalCount);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Greška pri učitavanju postova.');
          this.loading.set(false);
        },
      });
  }
}
