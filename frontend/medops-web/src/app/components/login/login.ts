import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  imports: [FormsModule, RouterLink],
  selector: 'app-login',
  template: `
    <div class="auth-page">
      <div class="auth-card">
        <div class="auth-header">
          <div class="auth-logo">
            <span class="logo-icon">M</span>
          </div>
          <h1>MedOps Admin</h1>
          <p>Sign in to your account</p>
        </div>
        <form (ngSubmit)="onLogin()" class="auth-form">
          <div class="form-group">
            <label for="email">Email Address</label>
            <div class="input-wrapper">
              <span class="material-icons input-icon">email</span>
              <input id="email" type="email" [(ngModel)]="email" name="email" required placeholder="admin&#64;medpace.com" />
            </div>
          </div>
          <div class="form-group">
            <label for="password">Password</label>
            <div class="input-wrapper">
              <span class="material-icons input-icon">lock</span>
              <input id="password" type="password" [(ngModel)]="password" name="password" required placeholder="Enter your password" />
            </div>
          </div>
          @if (error()) {
            <div class="alert alert-error">
              <span class="material-icons">error</span>
              {{ error() }}
            </div>
          }
          <button type="submit" class="btn-primary btn-block" [disabled]="loading()">
            @if (loading()) {
              <span class="spinner"></span>
            }
            Sign In
          </button>
        </form>
        <div class="auth-footer">
          Don't have an account? <a routerLink="/register">Create one</a>
        </div>
      </div>
      <div class="auth-bg">
        <div class="bg-content">
          <h2>Welcome to MedOps</h2>
          <p>Enterprise administrative platform for clinical research operations management.</p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .auth-page {
      display: flex;
      min-height: 100vh;
    }

    .auth-card {
      flex: 0 0 480px;
      display: flex;
      flex-direction: column;
      justify-content: center;
      padding: 3rem;
      background: white;
    }

    .auth-header { margin-bottom: 2rem; }

    .auth-logo { margin-bottom: 1.5rem; }
    .logo-icon {
      width: 48px;
      height: 48px;
      background: var(--primary);
      color: white;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 700;
      font-size: 1.5rem;
    }

    .auth-header h1 {
      font-size: 1.5rem;
      margin-bottom: 0.25rem;
    }
    .auth-header p {
      color: var(--text-secondary);
      font-size: 0.875rem;
    }

    .auth-form { display: flex; flex-direction: column; gap: 1.25rem; }

    .form-group label { margin-bottom: 0.375rem; }

    .input-wrapper {
      position: relative;
      display: flex;
      align-items: center;
    }
    .input-icon {
      position: absolute;
      left: 0.75rem;
      color: var(--text-muted);
      font-size: 1.125rem;
      pointer-events: none;
    }
    .input-wrapper input {
      padding-left: 2.5rem;
    }

    .alert {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.75rem 1rem;
      border-radius: var(--radius);
      font-size: 0.8125rem;
    }
    .alert-error {
      background: #fef2f2;
      color: var(--danger);
      border: 1px solid #fecaca;
    }
    .alert .material-icons { font-size: 1rem; }

    .btn-primary {
      padding: 0.7rem 1.5rem;
      background: var(--primary);
      color: white;
      font-weight: 500;
    }
    .btn-primary:hover:not(:disabled) { background: var(--primary-light); }
    .btn-block { width: 100%; display: flex; align-items: center; justify-content: center; gap: 0.5rem; }

    .spinner {
      width: 16px;
      height: 16px;
      border: 2px solid rgba(255,255,255,0.3);
      border-top-color: white;
      border-radius: 50%;
      animation: spin 0.6s linear infinite;
    }
    @keyframes spin { to { transform: rotate(360deg); } }

    .auth-footer {
      margin-top: 1.5rem;
      text-align: center;
      font-size: 0.875rem;
      color: var(--text-secondary);
    }
    .auth-footer a { font-weight: 500; }

    .auth-bg {
      flex: 1;
      background: linear-gradient(135deg, var(--primary) 0%, var(--primary-dark) 100%);
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 3rem;
      color: white;
    }
    .bg-content { max-width: 400px; }
    .bg-content h2 { color: white; font-size: 2rem; margin-bottom: 1rem; font-weight: 300; }
    .bg-content p { opacity: 0.85; font-size: 1rem; line-height: 1.7; }
  `]
})
export class Login {
  email = '';
  password = '';
  loading = signal(false);
  error = signal('');

  constructor(private auth: Auth, private router: Router) {
    if (this.auth.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
  }

  onLogin() {
    this.loading.set(true);
    this.error.set('');
    this.auth.login(this.email, this.password).subscribe({
      next: () => this.loading.set(false),
      error: (err) => {
        this.loading.set(false);
        this.error.set(err.error?.error || 'Invalid credentials');
      }
    });
  }
}
