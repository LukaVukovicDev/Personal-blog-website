export interface Tag {
  id?: string;
  name: string;
  slug: string;
}

export interface Category {
  id: string;
  name: string;
  slug: string;
  description?: string;
}

export interface PostListItem {
  id: string;
  slug: string;
  title: string;
  excerpt: string;
  coverImageUrl?: string;
  status: string;
  readMinutes: number;
  viewCount: number;
  publishedAt?: string;
  createdAt: string;
  authorName: string;
  categoryName?: string;
  categorySlug?: string;
  tags: Tag[];
}

export interface PostDetail extends PostListItem {
  contentHtml: string;
  authorBio?: string;
  authorAvatarUrl?: string;
}

export interface PostQuery {
  search?: string;
  category?: string;
  tag?: string;
  page?: number;
  pageSize?: number;
}

export interface CreatePostRequest {
  title: string;
  excerpt: string;
  contentHtml: string;
  coverImageUrl?: string;
  categoryId?: string;
  tagNames: string[];
  publish: boolean;
}

export interface UpdatePostRequest {
  title: string;
  excerpt: string;
  contentHtml: string;
  coverImageUrl?: string;
  categoryId?: string;
  tagNames: string[];
}
