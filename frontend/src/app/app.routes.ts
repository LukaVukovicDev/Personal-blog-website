import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/blog/home/home').then((m) => m.Home),
    title: 'Luka Vukovic - Personal Blog',
  },
  {
    path: 'posts',
    loadComponent: () => import('./features/blog/post-list/post-list').then((m) => m.PostList),
    title: 'Svi postovi',
  },
  {
    path: 'post/:slug',
    loadComponent: () => import('./features/blog/article/article').then((m) => m.Article),
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
    title: 'Prijava',
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then((m) => m.Register),
    title: 'Registracija',
  },

  // ── Admin panel (Author/Admin) ─────────────────────────────────
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard('Author', 'Admin')],
    loadComponent: () =>
      import('./features/admin/admin-layout/admin-layout').then((m) => m.AdminLayout),
    title: 'Admin panel',
    children: [
      { path: '', redirectTo: 'posts', pathMatch: 'full' },
      {
        path: 'posts',
        loadComponent: () =>
          import('./features/admin/post-list/post-list').then((m) => m.PostList),
        title: 'Admin · Postovi',
      },
      {
        path: 'posts/new',
        loadComponent: () =>
          import('./features/admin/post-editor/post-editor').then((m) => m.PostEditor),
        title: 'Admin · Novi post',
      },
      {
        path: 'posts/:id/edit',
        loadComponent: () =>
          import('./features/admin/post-editor/post-editor').then((m) => m.PostEditor),
        title: 'Admin · Izmena posta',
      },
      {
        path: 'categories',
        canActivate: [roleGuard('Admin')],
        loadComponent: () =>
          import('./features/admin/category-list/category-list').then((m) => m.CategoryList),
        title: 'Admin · Kategorije',
      },
      {
        path: 'comments',
        canActivate: [roleGuard('Admin')],
        loadComponent: () =>
          import('./features/admin/comment-moderation/comment-moderation').then(
            (m) => m.CommentModeration,
          ),
        title: 'Admin · Komentari',
      },
    ],
  },

  { path: '**', redirectTo: '' },
];
