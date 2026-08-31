import { Component, OnInit, AfterViewInit, OnDestroy, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Chart, registerables } from 'chart.js/auto';
import {
  Dashboard as DashboardService,
  DashboardStats,
  StatusBreakdown,
  MonthlyActivity,
  RecentActivity,
  OverdueItem,
} from '../../services/dashboard';

Chart.register(...registerables);

@Component({
  imports: [CommonModule, RouterLink],
  selector: 'app-dashboard',
  template: `
    @if (loading()) {
      <div class="loading-state">
        <div class="spinner"></div>
        <p>Loading dashboard...</p>
      </div>
    } @else if (error()) {
      <div class="error-state">
        <span class="material-icons">error_outline</span>
        <p>{{ error() }}</p>
        <button class="btn-retry" (click)="loadDashboard()">Retry</button>
      </div>
    } @else {
      <div class="dashboard">
        <div class="page-header">
          <div>
            <h1>Dashboard</h1>
            <p class="subtitle">Overview of your clinical research operations</p>
          </div>
          <div class="header-actions">
            <span class="last-updated">Last updated: {{ lastUpdated() }}</span>
          </div>
        </div>

        <div class="kpi-grid">
          <div class="kpi-card" (click)="navigate('/studies')">
            <div class="kpi-icon" style="background: rgba(26,35,126,0.08); color: var(--primary);">
              <span class="material-icons">science</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().totalStudies }}</span>
              <span class="kpi-label">Total Studies</span>
            </div>
            <div class="kpi-trend">
              <span class="kpi-detail">{{ stats().activeStudies }} active</span>
            </div>
          </div>

          <div class="kpi-card" (click)="navigate('/studies')">
            <div class="kpi-icon" style="background: rgba(46,125,50,0.08); color: var(--success);">
              <span class="material-icons">play_circle</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().activeStudies }}</span>
              <span class="kpi-label">Active Studies</span>
            </div>
          </div>

          <div class="kpi-card" (click)="navigate('/sites')">
            <div class="kpi-icon" style="background: rgba(2,119,189,0.08); color: var(--accent);">
              <span class="material-icons">business</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().totalSites }}</span>
              <span class="kpi-label">Total Sites</span>
            </div>
            <div class="kpi-trend">
              <span class="kpi-detail">{{ stats().activeSites }} active</span>
            </div>
          </div>

          <div class="kpi-card" (click)="navigate('/sites')">
            <div class="kpi-icon" style="background: rgba(2,119,189,0.08); color: #0277bd;">
              <span class="material-icons">domain</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().activeSites }}</span>
              <span class="kpi-label">Active Sites</span>
            </div>
          </div>

          <div class="kpi-card" (click)="navigate('/tasks')">
            <div class="kpi-icon" style="background: rgba(156,39,176,0.08); color: #9c27b0;">
              <span class="material-icons">checklist</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().totalTasks }}</span>
              <span class="kpi-label">Total Tasks</span>
            </div>
          </div>

          <div class="kpi-card" (click)="navigate('/tasks')">
            <div class="kpi-icon" style="background: rgba(46,125,50,0.08); color: var(--success);">
              <span class="material-icons">task_alt</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().completedTasks }}</span>
              <span class="kpi-label">Completed Tasks</span>
            </div>
            <div class="kpi-trend">
              <span class="kpi-detail" [class.text-warning]="taskCompletionRate() < 50">
                {{ taskCompletionRate() }}% completion
              </span>
            </div>
          </div>

          <div class="kpi-card" (click)="navigate('/requests')">
            <div class="kpi-icon" style="background: rgba(245,127,23,0.08); color: var(--warning);">
              <span class="material-icons">pending_actions</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().pendingRequests }}</span>
              <span class="kpi-label">Pending Requests</span>
            </div>
          </div>

          <div class="kpi-card">
            <div class="kpi-icon" style="background: rgba(26,35,126,0.08); color: var(--primary);">
              <span class="material-icons">apartment</span>
            </div>
            <div class="kpi-content">
              <span class="kpi-value">{{ stats().totalDepartments }}</span>
              <span class="kpi-label">Departments</span>
            </div>
          </div>
        </div>

        <div class="charts-row">
          <div class="chart-card">
            <div class="card-header">
              <h3>Studies by Status</h3>
            </div>
            <div class="card-body chart-body">
              <canvas id="studiesBarChart"></canvas>
            </div>
          </div>

          <div class="chart-card">
            <div class="card-header">
              <h3>Tasks by Status</h3>
            </div>
            <div class="card-body chart-body">
              <canvas id="tasksDoughnutChart"></canvas>
            </div>
          </div>

          <div class="chart-card">
            <div class="card-header">
              <h3>Monthly Activity</h3>
            </div>
            <div class="card-body chart-body">
              <canvas id="monthlyLineChart"></canvas>
            </div>
          </div>
        </div>

        <div class="bottom-row">
          <div class="card activity-card">
            <div class="card-header">
              <h3>Recent Activity</h3>
            </div>
            <div class="card-body activity-body">
              @if (recentActivities().length === 0) {
                <div class="empty-state">
                  <span class="material-icons">history</span>
                  <p>No recent activity</p>
                </div>
              } @else {
                @for (activity of recentActivities(); track $index) {
                  <div class="activity-item">
                    <div class="activity-icon">
                      <span class="material-icons">{{ getActivityIcon(activity.entityType) }}</span>
                    </div>
                    <div class="activity-content">
                      <p class="activity-text">
                        <strong>{{ activity.userName }}</strong> {{ activity.action }}
                        <span class="activity-entity">{{ activity.entityType }}{{ activity.entityName ? ' - ' + activity.entityName : '' }}</span>
                      </p>
                      <span class="activity-time">{{ formatTime(activity.timestamp) }}</span>
                    </div>
                  </div>
                }
              }
            </div>
          </div>

          <div class="card">
            <div class="card-header">
              <h3>Overdue Tasks</h3>
              <a routerLink="/tasks" class="btn-text">View all</a>
            </div>
            <div class="card-body list-body">
              @if (overdueTasks().length === 0) {
                <div class="empty-state">
                  <span class="material-icons">check_circle</span>
                  <p>No overdue tasks</p>
                </div>
              } @else {
                @for (task of overdueTasks(); track task.id) {
                  <div class="list-item overdue-item">
                    <div class="list-item-info">
                      <span class="list-item-title">{{ task.title }}</span>
                      <span class="list-item-meta">Due: {{ task.dueDate | date:'mediumDate' }}</span>
                    </div>
                    <span class="badge badge-overdue">{{ task.daysOverdue }}d overdue</span>
                  </div>
                }
              }
            </div>
          </div>

          <div class="card">
            <div class="card-header">
              <h3>Pending Requests</h3>
              <a routerLink="/requests" class="btn-text">View all</a>
            </div>
            <div class="card-body list-body">
              @if (pendingRequests().length === 0) {
                <div class="empty-state">
                  <span class="material-icons">inbox</span>
                  <p>No pending requests</p>
                </div>
              } @else {
                @for (request of pendingRequests(); track request.id) {
                  <div class="list-item">
                    <div class="list-item-info">
                      <span class="list-item-title">{{ request.title }}</span>
                      <span class="list-item-meta">{{ request.status }}</span>
                    </div>
                    <span class="badge badge-pending">Pending</span>
                  </div>
                }
              }
            </div>
          </div>
        </div>
      </div>
    }
  `,
  styles: [`
    :host { display: block; }

    .loading-state, .error-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      min-height: 400px;
      color: var(--text-secondary);
      gap: 1rem;
    }

    .spinner {
      width: 40px;
      height: 40px;
      border: 3px solid var(--border);
      border-top-color: var(--primary);
      border-radius: 50%;
      animation: spin 0.8s linear infinite;
    }

    @keyframes spin {
      to { transform: rotate(360deg); }
    }

    .error-state .material-icons {
      font-size: 3rem;
      color: var(--danger);
    }

    .btn-retry {
      padding: 0.5rem 1.5rem;
      background: var(--primary);
      color: white;
      border-radius: var(--radius);
      font-weight: 500;
    }
    .btn-retry:hover { background: var(--primary-light); }

    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 1.5rem;
    }
    .page-header h1 { margin-bottom: 0.125rem; }
    .subtitle { color: var(--text-secondary); font-size: 0.875rem; }

    .last-updated {
      font-size: 0.75rem;
      color: var(--text-muted);
    }

    .kpi-grid {
      display: grid;
      grid-template-columns: repeat(4, 1fr);
      gap: 1rem;
      margin-bottom: 1.5rem;
    }

    .kpi-card {
      background: white;
      border-radius: var(--radius-lg);
      padding: 1.25rem;
      display: flex;
      align-items: center;
      gap: 1rem;
      box-shadow: var(--shadow-sm);
      border: 1px solid var(--border-light);
      cursor: pointer;
      transition: box-shadow 0.15s, transform 0.15s;
      position: relative;
    }
    .kpi-card:hover {
      box-shadow: var(--shadow-md);
      transform: translateY(-1px);
    }

    .kpi-icon {
      width: 48px;
      height: 48px;
      border-radius: 12px;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
    }
    .kpi-icon .material-icons { font-size: 1.5rem; }

    .kpi-content { flex: 1; min-width: 0; }
    .kpi-value {
      display: block;
      font-size: 1.75rem;
      font-weight: 700;
      color: var(--text);
      line-height: 1.2;
    }
    .kpi-label {
      font-size: 0.8125rem;
      color: var(--text-secondary);
    }

    .kpi-trend {
      position: absolute;
      bottom: 0.75rem;
      right: 0.75rem;
    }
    .kpi-detail {
      font-size: 0.6875rem;
      color: var(--text-muted);
    }
    .text-warning { color: var(--warning) !important; }

    .charts-row {
      display: grid;
      grid-template-columns: 1fr 1fr 1fr;
      gap: 1rem;
      margin-bottom: 1.5rem;
    }

    .chart-card {
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

    .card-body { padding: 0; }

    .chart-body {
      padding: 1rem;
      height: 260px;
      position: relative;
    }

    .bottom-row {
      display: grid;
      grid-template-columns: 1.2fr 1fr 1fr;
      gap: 1rem;
    }

    .card {
      background: white;
      border-radius: var(--radius-lg);
      box-shadow: var(--shadow-sm);
      border: 1px solid var(--border-light);
      overflow: hidden;
    }

    .activity-body {
      max-height: 380px;
      overflow-y: auto;
      padding: 0.5rem 0;
    }

    .activity-item {
      display: flex;
      align-items: flex-start;
      gap: 0.75rem;
      padding: 0.625rem 1.25rem;
      transition: background 0.1s;
    }
    .activity-item:hover { background: var(--bg); }

    .activity-icon {
      width: 32px;
      height: 32px;
      border-radius: 50%;
      background: var(--bg);
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
      margin-top: 2px;
    }
    .activity-icon .material-icons {
      font-size: 1rem;
      color: var(--text-secondary);
    }

    .activity-content { flex: 1; min-width: 0; }
    .activity-text {
      font-size: 0.8125rem;
      line-height: 1.4;
      margin: 0;
    }
    .activity-entity {
      color: var(--accent);
      font-weight: 500;
    }
    .activity-time {
      font-size: 0.6875rem;
      color: var(--text-muted);
    }

    .list-body {
      max-height: 380px;
      overflow-y: auto;
      padding: 0.5rem 0;
    }

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

    .list-item-info { display: flex; flex-direction: column; min-width: 0; flex: 1; }
    .list-item-title {
      font-size: 0.875rem;
      font-weight: 400;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
    .list-item-meta { font-size: 0.75rem; color: var(--text-muted); }

    .badge {
      padding: 0.2rem 0.625rem;
      border-radius: 100px;
      font-size: 0.6875rem;
      font-weight: 500;
      text-transform: capitalize;
      white-space: nowrap;
      flex-shrink: 0;
      margin-left: 0.5rem;
    }

    .badge-overdue {
      background: #fef2f2;
      color: #991b1b;
    }

    .badge-pending {
      background: #fff7ed;
      color: #9a3412;
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

    @media (max-width: 1200px) {
      .kpi-grid { grid-template-columns: repeat(2, 1fr); }
      .charts-row { grid-template-columns: 1fr; }
      .bottom-row { grid-template-columns: 1fr; }
    }
  `]
})
export class Dashboard implements OnInit, AfterViewInit, OnDestroy {
  private dashboardService = inject(DashboardService);

  loading = signal(true);
  error = signal<string | null>(null);
  lastUpdated = signal('');

  stats = signal<DashboardStats>({
    totalStudies: 0,
    activeStudies: 0,
    totalSites: 0,
    activeSites: 0,
    totalTasks: 0,
    completedTasks: 0,
    pendingRequests: 0,
    totalDepartments: 0,
  });

  studiesByStatus = signal<StatusBreakdown[]>([]);
  tasksByStatus = signal<StatusBreakdown[]>([]);
  monthlyActivity = signal<MonthlyActivity[]>([]);
  recentActivities = signal<RecentActivity[]>([]);
  overdueTasks = signal<OverdueItem[]>([]);
  pendingRequests = signal<OverdueItem[]>([]);

  taskCompletionRate = signal(0);

  private charts: Chart[] = [];

  ngOnInit(): void {
    this.loadDashboard();
  }

  ngAfterViewInit(): void {
    // Charts are rendered after data loads in loadDashboard
  }

  ngOnDestroy(): void {
    this.destroyCharts();
  }

  loadDashboard(): void {
    this.loading.set(true);
    this.error.set(null);

    this.dashboardService.getDashboard().subscribe({
      next: (data) => {
        this.stats.set(data.stats);
        this.studiesByStatus.set(data.studiesByStatus);
        this.tasksByStatus.set(data.tasksByStatus);
        this.monthlyActivity.set(data.monthlyActivity);
        this.recentActivities.set(data.recentActivities.slice(0, 10));
        this.overdueTasks.set(data.overdueTasks);
        this.pendingRequests.set(data.pendingRequests);

        if (data.stats.totalTasks > 0) {
          this.taskCompletionRate.set(
            Math.round((data.stats.completedTasks / data.stats.totalTasks) * 100)
          );
        }

        this.lastUpdated.set(new Date().toLocaleTimeString());
        this.loading.set(false);

        setTimeout(() => this.renderCharts(), 0);
      },
      error: (err) => {
        console.error('Dashboard load error:', err);
        this.error.set('Failed to load dashboard data. Please try again.');
        this.loading.set(false);
      },
    });
  }

  navigate(path: string): void {
    window.location.href = path;
  }

  getActivityIcon(entityType: string): string {
    const icons: Record<string, string> = {
      Study: 'science',
      Site: 'business',
      Task: 'task_alt',
      Request: 'request_quote',
      Department: 'apartment',
    };
    return icons[entityType] || 'info';
  }

  formatTime(timestamp: string): string {
    const date = new Date(timestamp);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMins / 60);
    const diffDays = Math.floor(diffHours / 24);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins}m ago`;
    if (diffHours < 24) return `${diffHours}h ago`;
    if (diffDays < 7) return `${diffDays}d ago`;
    return date.toLocaleDateString();
  }

  private destroyCharts(): void {
    this.charts.forEach((chart) => chart.destroy());
    this.charts = [];
  }

  private renderCharts(): void {
    this.destroyCharts();

    this.renderStudiesBarChart();
    this.renderTasksDoughnutChart();
    this.renderMonthlyLineChart();
  }

  private renderStudiesBarChart(): void {
    const canvas = document.getElementById('studiesBarChart') as HTMLCanvasElement;
    if (!canvas) return;

    const data = this.studiesByStatus();
    const labels = data.map((d) => d.status);
    const values = data.map((d) => d.count);

    const colors = labels.map((status) => {
      const map: Record<string, string> = {
        Active: '#2e7d32',
        Planning: '#1a237e',
        Completed: '#0277bd',
        OnHold: '#f57f17',
        Cancelled: '#9e9e9e',
        Recruiting: '#7b1fa2',
        Closed: '#616161',
      };
      return map[status] || '#1a237e';
    });

    const chart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels,
        datasets: [
          {
            label: 'Studies',
            data: values,
            backgroundColor: colors,
            borderRadius: 6,
            barThickness: 32,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#1a237e',
            titleFont: { family: 'Roboto' },
            bodyFont: { family: 'Roboto' },
            padding: 10,
            cornerRadius: 6,
          },
        },
        scales: {
          x: {
            grid: { display: false },
            ticks: { font: { family: 'Roboto', size: 11 } },
          },
          y: {
            beginAtZero: true,
            grid: { color: '#f5f5f5' },
            ticks: {
              font: { family: 'Roboto', size: 11 },
              stepSize: 1,
            },
          },
        },
      },
    });

    this.charts.push(chart);
  }

  private renderTasksDoughnutChart(): void {
    const canvas = document.getElementById('tasksDoughnutChart') as HTMLCanvasElement;
    if (!canvas) return;

    const data = this.tasksByStatus();
    const labels = data.map((d) => d.status);
    const values = data.map((d) => d.count);

    const colors = labels.map((status) => {
      const map: Record<string, string> = {
        Completed: '#2e7d32',
        ToDo: '#1a237e',
        InProgress: '#f57f17',
        Pending: '#0277bd',
        Cancelled: '#9e9e9e',
      };
      return map[status] || '#616161';
    });

    const chart = new Chart(canvas, {
      type: 'doughnut',
      data: {
        labels,
        datasets: [
          {
            data: values,
            backgroundColor: colors,
            borderWidth: 2,
            borderColor: '#ffffff',
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '65%',
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 16,
              usePointStyle: true,
              pointStyle: 'circle',
              font: { family: 'Roboto', size: 11 },
            },
          },
          tooltip: {
            backgroundColor: '#1a237e',
            titleFont: { family: 'Roboto' },
            bodyFont: { family: 'Roboto' },
            padding: 10,
            cornerRadius: 6,
          },
        },
      },
    });

    this.charts.push(chart);
  }

  private renderMonthlyLineChart(): void {
    const canvas = document.getElementById('monthlyLineChart') as HTMLCanvasElement;
    if (!canvas) return;

    const data = this.monthlyActivity();
    const labels = data.map((d) => {
      const date = new Date(d.month + '-01');
      return date.toLocaleDateString('en-US', { month: 'short', year: '2-digit' });
    });

    const chart = new Chart(canvas, {
      type: 'line',
      data: {
        labels,
        datasets: [
          {
            label: 'Studies Created',
            data: data.map((d) => d.studiesCreated),
            borderColor: '#1a237e',
            backgroundColor: 'rgba(26,35,126,0.1)',
            fill: true,
            tension: 0.4,
            pointRadius: 4,
            pointBackgroundColor: '#1a237e',
          },
          {
            label: 'Tasks Completed',
            data: data.map((d) => d.tasksCompleted),
            borderColor: '#2e7d32',
            backgroundColor: 'rgba(46,125,50,0.1)',
            fill: true,
            tension: 0.4,
            pointRadius: 4,
            pointBackgroundColor: '#2e7d32',
          },
          {
            label: 'Requests Processed',
            data: data.map((d) => d.requestsProcessed),
            borderColor: '#f57f17',
            backgroundColor: 'rgba(245,127,23,0.1)',
            fill: true,
            tension: 0.4,
            pointRadius: 4,
            pointBackgroundColor: '#f57f17',
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        interaction: {
          mode: 'index',
          intersect: false,
        },
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 16,
              usePointStyle: true,
              pointStyle: 'circle',
              font: { family: 'Roboto', size: 11 },
            },
          },
          tooltip: {
            backgroundColor: '#1a237e',
            titleFont: { family: 'Roboto' },
            bodyFont: { family: 'Roboto' },
            padding: 10,
            cornerRadius: 6,
          },
        },
        scales: {
          x: {
            grid: { display: false },
            ticks: { font: { family: 'Roboto', size: 11 } },
          },
          y: {
            beginAtZero: true,
            grid: { color: '#f5f5f5' },
            ticks: {
              font: { family: 'Roboto', size: 11 },
              stepSize: 1,
            },
          },
        },
      },
    });

    this.charts.push(chart);
  }
}
