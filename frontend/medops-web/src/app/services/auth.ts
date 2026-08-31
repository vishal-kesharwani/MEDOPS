import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

interface LoginResponse {
  token: string;
  expires: string;
}

@Injectable({ providedIn: 'root' })
export class Auth {
  private readonly tokenKey = 'medops_token';
  private readonly _isAuthenticated = signal(!!localStorage.getItem(this.tokenKey));

  readonly isAuthenticated = this._isAuthenticated.asReadonly();

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string) {
    return this.http.post<LoginResponse>('/api/auth/login', { email, password }).pipe(
      tap(res => {
        localStorage.setItem(this.tokenKey, res.token);
        this._isAuthenticated.set(true);
        this.router.navigate(['/dashboard']);
      })
    );
  }

  register(email: string, password: string, firstName: string, lastName: string) {
    return this.http.post<{ message: string }>('/api/auth/register', { email, password, firstName, lastName });
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this._isAuthenticated.set(false);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }
}
