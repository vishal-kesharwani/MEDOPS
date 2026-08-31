import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Study, StudyDto } from '../../services/study';
import { Site, SiteDto } from '../../services/site';
import { Task as TaskService, TaskDto } from '../../services/task';
import { Request as RequestService, RequestDto } from '../../services/request';

@Component({
  imports: [CommonModule, RouterLink],
  selector: 'app-dashboard',
  template: `
    <div class="dashboard">
      <div class="page-header">
        <div>
          <h1>Dashboard</h1>
          <p class="subtitle">Overview of your clinical research operations</p>
        </div>
      </div>

      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-icon" style="background: rgba(26,35,126,0.08); color: var(--primary);">
            <span class="material-icons">science</span>
          </div>
          <div class="stat-info">
            <span class="stat-value">{{ studies().length }}</span>
            <span class="stat-label">Studies</span>
          </div>
          <a routerLink="/studies" class="stat-link">View all</a>
        </div>

        <div class="stat-card">
          <div class="stat-icon" style="background: rgba(2,119,189,0.08); color: var(--accent);">
            <span class="material-icons">business</span>
          </div>
          <div class="stat-info">
            <span class="stat-value">{{ sites().length }}</span>
            <span class="stat-label">Sites</span>
          </div>
          <a routerLink="/sites" class="stat-link">View all</a>
        </div>

        <div class="stat-card">
          <div class="stat-icon" style="background: rgba(46,125,50,0.08); color: var(--success);">
            <span class="material-icons">task_alt</span>
          </div>
          <div class="stat-info">
            <span class="stat-value">{{ tasks().length }}</span>
            <span class="stat-label">Tasks</span>
          </div>
          <a routerLink="/tasks" class="stat-link">View all</a>
        </div>

        <div class="stat-card">
          <div class="stat-icon" style="background: rgba(245,127,23,0.08); color: var(--warning);">
            <span class="material-icons">request_quote</span>
          </div>
          <div class="stat-info">
            <span class="stat-value">{{ requests().length }}</span>
            <span class="stat-label">Requests</span>
          </div>
          <a routerLink="/requests" class="stat-link">View all</a>
        </div>
      </div>

      <div class="content-grid">
        <div class="card">
          <div class="card-header">
            <h3>Recent Studies</h3>
            <a routerLink="/studies" class="btn-text">View all</a>
          </div>
          <div class="card-body">
            @if (studies().length === 0) {
              <div class="empty-state">
                <span class="material-icons">science</span>
                <p>No studies yet</p>
              </div>
            } @else {
              @for (study of studies().slice(0, 5); track study.id) {
                <div class="list-item">
                  <div class="list-item-info">
                    <span class="list-item-title">{{ study.name }}</span>
                    <span class="list-item-meta">{{ study.createdAt | date:'mediumDate' }}</span>
                  </div>
                  <span class="badge" [attr.data-status]="study.status">{{ study.status }}</span>
                </div>
              }
            }
          </div>
        </div>

        <div class="card">
          <div class="card-header">
            <h3>Pending Tasks</h3>
            <a routerLink="/tasks" class="btn-text">View all</a>
          </div>
          <div class="card-body">
            @if (tasks().length === 0) {
              <div class="empty-state">
                <span class="material-icons">task_alt</span>
                <p>No tasks yet</p>
              </div>
            } @else {
              @for (task of tasks().slice(0, 5); track task.id) {
                <div class="list-item">
                  <div class="list-item-info">
                    <span class="list-item-title">{{ task.title }}</span>
                    <span class="list-item-meta">{{ task.priority }}</span>
                  </div>
                  <span class="badge" [attr.data-status]="task.status">{{ task.status }}</span>
                </div>
              }
            }
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 1.5rem;
    }
    .page-header h1 { margin-bottom: 0.125rem; }
    .subtitle { color: var(--text-secondary); font-size: 0.875rem; }

    .stats-grid {
      display: grid;
      grid-template-columns: repeat(4, 1fr);
      gap: 1rem;
      margin-bottom: 1.5rem;
    }

    .stat-card {
      background: white;
      border-radius: var(--radius-lg);
      padding: 1.25rem;
      display: flex;
      align-items: center;
      gap: 1rem;
      box-shadow: var(--shadow-sm);
      border: 1px solid var(--border-light);
      position: relative;
    }

    .stat-icon {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
    }
    .stat-icon .material-icons { font-size: 1.5rem; }

    .stat-info { flex: 1; }
    .stat-value { display: block; font-size: 1.75rem; font-weight: 700; color: var(--text); line-height: 1.2; }
    .stat-label { font-size: 0.8125rem; color: var(--text-secondary); }

    .stat-link {
      position: absolute;
      top: 0.75rem;
      right: 0.75rem;
      font-size: 0.75rem;
      color: var(--text-muted);
    }
    .stat-link:hover { color: var(--primary); text-decoration: none; }

    .content-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 1rem;
    }

    .card {
      background: white;
      border-radius: var(--radius-lg);
      box-shadow: var(--shadow-sm);
      border: 1px solid var(--border-light);
      overflow: hidden;
    }

    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1rem 1.25rem;
      border-bottom: 1px solid var(--border-light);
    }
    .card-header h3 { font-size: 0.9375rem; }

    .btn-text {
      font-size: 0.8125rem;
      color: var(--accent);
      font-weight: 500;
    }
    .btn-text:hover { text-decoration: none; }

    .card-body { padding: 0.5rem 0; }

    .empty-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 2rem;
      color: var(--text-muted);
    }
    .empty-state .material-icons { font-size: 2rem; margin-bottom: 0.5rem; opacity: 0.5; }
    .empty-state p { font-size: 0.875rem; }

    .list-item {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0.625rem 1.25rem;
      transition: background 0.1s;
    }
    .list-item:hover { background: var(--bg); }

    .list-item-info { display: flex; flex-direction: column; }
    .list-item-title { font-size: 0.875rem; font-weight: 400; }
    .list-item-meta { font-size: 0.75rem; color: var(--text-muted); }

    .badge {
      padding: 0.2rem 0.625rem;
      border-radius: 100px;
      font-size: 0.6875rem;
      font-weight: 500;
      text-transform: capitalize;
      white-space: nowrap;
    }

    .badge[data-status="Active"],
    .badge[data-status="Completed"] {
      background: #f0fdf4;
      color: #166534;
    }
    .badge[data-status="Planning"],
    .badge[data-status="ToDo"],
    .badge[data-status="Pending"] {
      background: #eff6ff;
      color: #1e40af;
    }
    .badge[data-status="InProgress"],
    .badge[data-status="Active"] {
      background: #fefce8;
      color: #854d0e;
    }
    .badge[data-status="Closed"],
    .badge[data-status="Cancelled"] {
      background: #f5f5f5;
      color: #737373;
    }
  `]
})
export class Dashboard implements OnInit {
  studies = signal<StudyDto[]>([]);
  sites = signal<SiteDto[]>([]);
  tasks = signal<TaskDto[]>([]);
  requests = signal<RequestDto[]>([]);

  constructor(
    private studyService: Study,
    private siteService: Site,
    private taskService: TaskService,
    private requestService: RequestService
  ) {}

  ngOnInit() {
    this.studyService.getAll().subscribe(data => this.studies.set(data));
    this.siteService.getAll().subscribe(data => this.sites.set(data));
    this.taskService.getAll().subscribe(data => this.tasks.set(data));
    this.requestService.getAll().subscribe(data => this.requests.set(data));
  }
}
