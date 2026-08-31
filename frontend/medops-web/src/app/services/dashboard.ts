import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DashboardStats {
  totalStudies: number;
  activeStudies: number;
  totalSites: number;
  activeSites: number;
  totalTasks: number;
  completedTasks: number;
  pendingRequests: number;
  totalDepartments: number;
}

export interface StatusBreakdown {
  status: string;
  count: number;
}

export interface MonthlyActivity {
  month: string;
  studiesCreated: number;
  tasksCompleted: number;
  requestsProcessed: number;
}

export interface RecentActivity {
  userName: string;
  action: string;
  entityType: string;
  entityName?: string;
  timestamp: string;
}

export interface OverdueItem {
  id: string;
  title: string;
  status: string;
  dueDate?: string;
  daysOverdue?: number;
}

export interface DashboardDto {
  stats: DashboardStats;
  studiesByStatus: StatusBreakdown[];
  tasksByStatus: StatusBreakdown[];
  requestsByStatus: StatusBreakdown[];
  monthlyActivity: MonthlyActivity[];
  recentActivities: RecentActivity[];
  overdueTasks: OverdueItem[];
  pendingRequests: OverdueItem[];
}

@Injectable({ providedIn: 'root' })
export class Dashboard {
  private http = inject(HttpClient);
  private apiUrl = '/api/dashboard';

  getDashboard(): Observable<DashboardDto> {
    return this.http.get<DashboardDto>(this.apiUrl);
  }

  getRecentActivity(page = 1, pageSize = 20): Observable<{ items: RecentActivity[]; totalCount: number }> {
    return this.http.get<{ items: RecentActivity[]; totalCount: number }>(`${this.apiUrl}/activity`, { params: { page, pageSize } });
  }
}
