import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Auth } from '../../services/auth';
import { Notification as NotificationService, NotificationDto } from '../../services/notification';

@Component({
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
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
          <div class="notification-bell" (click)="toggleNotifications()">
            <span class="material-icons">notifications</span>
            @if (unreadCount() > 0) {
              <span class="notification-badge">{{ unreadCount() }}</span>
            }
          </div>
          @if (showNotifications()) {
            <div class="notification-panel">
              <div class="panel-header">
                <h4>Notifications</h4>
                @if (unreadCount() > 0) {
                  <button class="btn-link" (click)="markAllRead()">Mark all read</button>
                }
              </div>
              <div class="panel-body">
                @for (n of notifications(); track n.id) {
                  <div class="notification-item" [class.unread]="!n.isRead" (click)="markRead(n.id)">
                    <span class="material-icons notif-icon" [attr.data-type]="n.type">
                      {{ n.type === 'Success' ? 'check_circle' : n.type === 'Warning' ? 'warning' : n.type === 'Error' ? 'error' : 'info' }}
                    </span>
                    <div class="notif-content">
                      <p class="notif-title">{{ n.title }}</p>
                      <p class="notif-message">{{ n.message }}</p>
                      <span class="notif-time">{{ n.createdAt | date:'short' }}</span>
                    </div>
                  </div>
                }
                @if (notifications().length === 0) {
                  <div class="notif-empty">
                    <span class="material-icons">notifications_off</span>
                    <p>No notifications</p>
                  </div>
                }
              </div>
            </div>
          }
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
            <a routerLink="/audit" routerLinkActive="active" class="nav-item">
              <span class="material-icons">history</span>
              Audit Log
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

    .notification-bell {
      position: relative;
      cursor: pointer;
      padding: 0.375rem;
      border-radius: var(--radius);
      transition: background 0.15s;
    }
    .notification-bell:hover { background: rgba(255,255,255,0.15); }
    .notification-bell .material-icons { font-size: 1.375rem; }
    .notification-badge {
      position: absolute;
      top: 0;
      right: 0;
      background: #ef4444;
      color: white;
      font-size: 0.625rem;
      font-weight: 700;
      min-width: 16px;
      height: 16px;
      border-radius: 100px;
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 0 4px;
    }

    .notification-panel {
      position: absolute;
      top: calc(var(--nav-height) - 4px);
      right: 5rem;
      width: 380px;
      max-height: 480px;
      background: white;
      border-radius: var(--radius-lg);
      box-shadow: 0 8px 24px rgba(0,0,0,0.15);
      border: 1px solid var(--border-light);
      z-index: 200;
      overflow: hidden;
    }
    .panel-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 0.875rem 1rem;
      border-bottom: 1px solid var(--border-light);
    }
    .panel-header h4 { font-size: 0.875rem; font-weight: 600; color: var(--text); margin: 0; }
    .btn-link {
      background: none;
      border: none;
      color: var(--primary);
      font-size: 0.75rem;
      cursor: pointer;
      font-weight: 500;
    }
    .btn-link:hover { text-decoration: underline; }
    .panel-body {
      max-height: 400px;
      overflow-y: auto;
    }
    .notification-item {
      display: flex;
      gap: 0.75rem;
      padding: 0.75rem 1rem;
      cursor: pointer;
      border-bottom: 1px solid var(--border-light);
      transition: background 0.1s;
    }
    .notification-item:hover { background: var(--bg); }
    .notification-item.unread { background: #f0f4ff; }
    .notif-icon { font-size: 1.25rem; flex-shrink: 0; margin-top: 2px; }
    .notif-icon[data-type="Success"] { color: #16a34a; }
    .notif-icon[data-type="Warning"] { color: #d97706; }
    .notif-icon[data-type="Error"] { color: #dc2626; }
    .notif-icon[data-type="Info"] { color: #2563eb; }
    .notif-content { flex: 1; min-width: 0; }
    .notif-title { font-size: 0.8125rem; font-weight: 500; color: var(--text); margin: 0 0 2px; }
    .notif-message { font-size: 0.75rem; color: var(--text-secondary); margin: 0 0 4px; }
    .notif-time { font-size: 0.6875rem; color: var(--text-muted); }
    .notif-empty {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 2rem;
      color: var(--text-muted);
    }
    .notif-empty .material-icons { font-size: 2rem; opacity: 0.4; margin-bottom: 0.5rem; }

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
export class Layout implements OnInit {
  showNotifications = signal(false);
  notifications = signal<NotificationDto[]>([]);
  unreadCount = signal(0);

  constructor(public auth: Auth, private notificationService: NotificationService) {}

  ngOnInit() {
    this.loadNotifications();
    setInterval(() => this.loadNotifications(), 30000);
  }

  loadNotifications() {
    this.notificationService.getUnreadCount().subscribe(count => this.unreadCount.set(count));
    this.notificationService.getAll().subscribe(n => this.notifications.set(n));
  }

  toggleNotifications() {
    this.showNotifications.set(!this.showNotifications());
    if (this.showNotifications()) this.loadNotifications();
  }

  markRead(id: string) {
    this.notificationService.markAsRead(id).subscribe(() => this.loadNotifications());
  }

  markAllRead() {
    this.notificationService.markAllAsRead().subscribe(() => this.loadNotifications());
  }
}
