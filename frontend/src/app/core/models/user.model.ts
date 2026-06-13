export type UserRole = 'Reader' | 'Author' | 'Admin';

export interface User {
  id: string;
  username: string;
  email: string;
  displayName: string;
  role: UserRole;
  bio?: string;
  avatarUrl?: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

export interface LoginRequest {
  usernameOrEmail: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  displayName: string;
}
