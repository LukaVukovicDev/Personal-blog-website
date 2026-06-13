export interface AppNotification {
  id: string;
  type: 'NewComment' | 'CommentApproved' | 'NewPost';
  message: string;
  linkUrl?: string;
  isRead: boolean;
  createdAt: string;
}
