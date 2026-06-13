export interface Comment {
  id: string;
  body: string;
  status: string;
  createdAt: string;
  authorId: string;
  authorName: string;
  parentCommentId?: string;
}

export interface CreateComment {
  body: string;
  parentCommentId?: string;
}

/** Komentar sa podacima o postu — koristi admin panel za moderaciju. */
export interface CommentAdmin extends Comment {
  postTitle: string;
  postSlug: string;
}
