import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  selector: 'app-layout',
  template: `
    <div class="layout">
      <header class="topnav">
        <div class="topnav-left">
          <div class="brand">
            <span class="brand-icon">M</span>
            <span class="brand-text">MedOps <span class="brand-sub">Admin</span></span>
          </div>
        </div>
        <div class="topnav-right">
          <span class="user-info">
            <span class="material-icons user-icon">account_circle</span>
          </span>
          <button class="btn-logout" (click)="auth.logout()">
            <span class="material-icons">logout</span>
            Logout
          </button>
        </div>
      </header>
      <div class="body">
        <aside class="sidebar">
          <nav class="nav-menu">
            <a routerLink="/dashboard" routerLinkActive="active" class="nav-item">
              <span class="material-icons">dashboard</span>
              Dashboard
            </a>
            <a routerLink="/studies" routerLinkActive="active" class="nav-item">
              <span class="material-icons">science</span>
              Studies
            </a>
            <a routerLink="/sites" routerLinkActive="active" class="nav-item">
              <span class="material-icons">business</span>
              Sites
            </a>
            <a routerLink="/tasks" routerLinkActive="active" class="nav-item">
              <span class="material-icons">task_alt</span>
              Tasks
            </a>
            <a routerLink="/requests" routerLinkActive="active" class="nav-item">
              <span class="material-icons">request_quote</span>
              Requests
            </a>
            <a routerLink="/departments" routerLinkActive="active" class="nav-item">
              <span class="material-icons">corporate_fare</span>
              Departments
            </a>
          </nav>
        </aside>
        <main class="content">
          <router-outlet />
        </main>
      </div>
    </div>
  `,
  styles: [`
    .layout { display: flex; flex-direction: column; min-height: 100vh; }

    .topnav {
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: var(--nav-height);
      padding: 0 1.5rem;
      background: var(--primary);
      color: white;
      box-shadow: var(--shadow-md);
      position: sticky;
      top: 0;
      z-index: 100;
    }

    .topnav-left { display: flex; align-items: center; gap: 2rem; }

    .brand { display: flex; align-items: center; gap: 0.75rem; }

    .brand-icon {
      width: 36px;
      height: 36px;
      background: rgba(255,255,255,0.15);
      border-radius: 8px;
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 700;
      font-size: 1.25rem;
    }

    .brand-text { font-size: 1.125rem; font-weight: 500; letter-spacing: -0.3px; }
    .brand-sub { font-weight: 300; opacity: 0.8; }

    .topnav-right { display: flex; align-items: center; gap: 1rem; }

    .user-info { display: flex; align-items: center; gap: 0.5rem; }
    .user-icon { font-size: 2rem; opacity: 0.9; }

    .btn-logout {
      display: flex;
      align-items: center;
      gap: 0.375rem;
      padding: 0.4rem 0.75rem;
      background: rgba(255,255,255,0.1);
      color: white;
      border: 1px solid rgba(255,255,255,0.2);
      border-radius: var(--radius);
      font-size: 0.8125rem;
      transition: background 0.15s;
    }
    .btn-logout:hover { background: rgba(255,255,255,0.2); }
    .btn-logout .material-icons { font-size: 1.125rem; }

    .body { display: flex; flex: 1; }

    .sidebar {
      width: var(--sidebar-width);
      background: white;
      border-right: 1px solid var(--border);
      padding: 0.75rem 0;
      position: sticky;
      top: var(--nav-height);
      height: calc(100vh - var(--nav-height));
      overflow-y: auto;
      flex-shrink: 0;
    }

    .nav-menu { display: flex; flex-direction: column; gap: 2px; padding: 0 0.5rem; }

    .nav-item {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      padding: 0.625rem 0.875rem;
      border-radius: var(--radius);
      color: var(--text-secondary);
      font-size: 0.875rem;
      font-weight: 400;
      text-decoration: none;
      transition: all 0.15s;
    }
    .nav-item:hover {
      background: var(--bg);
      color: var(--text);
      text-decoration: none;
    }
    .nav-item.active {
      background: rgba(26,35,126,0.08);
      color: var(--primary);
      font-weight: 500;
    }
    .nav-item .material-icons { font-size: 1.25rem; }

    .content {
      flex: 1;
      padding: 1.5rem 2rem;
      min-width: 0;
    }
  `]
})
export class Layout {
  constructor(public auth: Auth) {}
}
