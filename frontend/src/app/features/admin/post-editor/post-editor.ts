import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { PostService } from '../../../core/services/post.service';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/post.model';

@Component({
  selector: 'app-post-editor',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './post-editor.html',
  styleUrl: './post-editor.css',
})
export class PostEditor {
  /** Bind-uje se iz rute `admin/posts/:id/edit`; nedostaje za "novi post". */
  readonly id = input<string>();

  private readonly fb = inject(FormBuilder);
  private readonly postService = inject(PostService);
  private readonly categoryService = inject(CategoryService);
  private readonly router = inject(Router);

  readonly isEdit = computed(() => !!this.id());
  readonly categories = signal<Category[]>([]);
  readonly loading = signal(false);
  readonly submitting = signal(false);
  readonly error = signal<string | null>(null);
  readonly currentStatus = signal<string | null>(null);

  /** Slug kategorije učitanog posta — primenjuje se na formu kad kategorije stignu. */
  private readonly pendingCategorySlug = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.minLength(3)]],
    excerpt: ['', [Validators.required, Validators.maxLength(280)]],
    contentHtml: ['', Validators.required],
    coverImageUrl: [''],
    categoryId: [''],
    tagNames: [''],
    publish: [false],
  });

  constructor() {
    this.categoryService.getAll().subscribe({
      next: (categories) => this.categories.set(categories),
    });

    effect(() => {
      const id = this.id();
      if (id) {
        this.loadPost(id);
      }
    });

    // Kategorije i post se učitavaju nezavisno — kad oba stignu, popuni categoryId.
    effect(() => {
      const categories = this.categories();
      const slug = this.pendingCategorySlug();
      if (slug && categories.length > 0) {
        const match = categories.find((c) => c.slug === slug);
        if (match) {
          this.form.patchValue({ categoryId: match.id });
        }
        this.pendingCategorySlug.set(null);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const tagNames = value.tagNames
      .split(',')
      .map((tag) => tag.trim())
      .filter((tag) => tag.length > 0);

    this.submitting.set(true);
    this.error.set(null);

    const id = this.id();
    if (id) {
      this.postService
        .update(id, {
          title: value.title,
          excerpt: value.excerpt,
          contentHtml: value.contentHtml,
          coverImageUrl: value.coverImageUrl || undefined,
          categoryId: value.categoryId || undefined,
          tagNames,
        })
        .subscribe({
          next: () => this.router.navigateByUrl('/admin/posts'),
          error: () => {
            this.error.set('Čuvanje izmena nije uspelo.');
            this.submitting.set(false);
          },
        });
    } else {
      this.postService
        .create({
          title: value.title,
          excerpt: value.excerpt,
          contentHtml: value.contentHtml,
          coverImageUrl: value.coverImageUrl || undefined,
          categoryId: value.categoryId || undefined,
          tagNames,
          publish: value.publish,
        })
        .subscribe({
          next: () => this.router.navigateByUrl('/admin/posts'),
          error: () => {
            this.error.set('Kreiranje posta nije uspelo.');
            this.submitting.set(false);
          },
        });
    }
  }

  private loadPost(id: string): void {
    this.loading.set(true);

    this.postService.getById(id).subscribe({
      next: (post) => {
        this.currentStatus.set(post.status);
        this.form.patchValue({
          title: post.title,
          excerpt: post.excerpt,
          contentHtml: post.contentHtml,
          coverImageUrl: post.coverImageUrl ?? '',
          tagNames: post.tags.map((tag) => tag.name).join(', '),
        });

        if (post.categorySlug) {
          this.pendingCategorySlug.set(post.categorySlug);
        }

        this.loading.set(false);
      },
      error: () => {
        this.error.set('Učitavanje posta nije uspelo.');
        this.loading.set(false);
      },
    });
  }
}
