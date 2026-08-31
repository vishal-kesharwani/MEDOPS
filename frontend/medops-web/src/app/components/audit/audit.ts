import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Audit, AuditLogDto, PaginatedResult } from '../../services/audit';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'app-audit-log',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Audit Log</h1>
          <p class="subtitle">Track all system changes and user actions</p>
        </div>
      </div>

      <div class="card filters">
        <div class="filter-row">
          <input type="text" [(ngModel)]="searchTerm" (keyup.enter)="load()" placeholder="Search logs..." class="search-input" />
          <select [(ngModel)]="sortBy" (change)="load()">
            <option value="timestamp">Date</option>
            <option value="entityname">Entity</option>
            <option value="action">Action</option>
            <option value="username">User</option>
          </select>
          <button (click)="sortDescending = !sortDescending; load()" class="btn-sm">
            <span class="material-icons">{{ sortDescending ? 'arrow_downward' : 'arrow_upward' }}</span>
          </button>
          <button (click)="load()" class="btn-primary btn-sm"><span class="material-icons">refresh</span> Refresh</button>
        </div>
      </div>

      <div class="card">
        <table>
          <thead>
            <tr>
              <th>Timestamp</th>
              <th>Action</th>
              <th>Entity</th>
              <th>User</th>
              <th>Description</th>
            </tr>
          </thead>
          <tbody>
            @for (log of logs().items; track log.id) {
              <tr>
                <td class="cell-muted">{{ log.timestamp | date:'medium' }}</td>
                <td>
                  <span class="action-badge" [attr.data-action]="log.action">{{ log.action }}</span>
                </td>
                <td>
                  <span class="cell-primary">{{ log.entityName }}</span>
                  <span class="cell-id">{{ log.entityId | slice:0:8 }}...</span>
                </td>
                <td>{{ log.userName }}</td>
                <td class="cell-muted">{{ log.description || '—' }}</td>
              </tr>
            }
            @if (logs().items.length === 0) {
              <tr>
                <td colspan="5">
                  <div class="empty-state">
                    <span class="material-icons">history</span>
                    <p>No audit logs found</p>
                  </div>
                </td>
              </tr>
            }
          </tbody>
        </table>

        @if (logs().totalCount > 0) {
          <div class="pagination">
            <span class="page-info">Showing {{ (page - 1) * pageSize + 1 }}-{{ Math.min(page * pageSize, logs().totalCount) }} of {{ logs().totalCount }}</span>
            <div class="page-buttons">
              <button (click)="page = page - 1; load()" [disabled]="page <= 1" class="btn-sm">Previous</button>
              <span class="page-num">Page {{ page }} of {{ logs().totalPages }}</span>
              <button (click)="page = page + 1; load()" [disabled]="page >= logs().totalPages" class="btn-sm">Next</button>
            </div>
          </div>
        }
      </div>
    </div>
  `,
  styles: [`
    .page-header { margin-bottom: 1.5rem; }
    .page-header h1 { margin-bottom: 0.125rem; }
    .subtitle { color: var(--text-secondary); font-size: 0.875rem; }
    .card { background: white; border-radius: var(--radius-lg); box-shadow: var(--shadow-sm); border: 1px solid var(--border-light); overflow: hidden; margin-bottom: 1rem; }
    .filters { padding: 1rem 1.25rem; }
    .filter-row { display: flex; gap: 0.5rem; align-items: center; }
    .search-input { flex: 1; padding: 0.5rem 0.75rem; border: 1px solid var(--border); border-radius: var(--radius); font-size: 0.875rem; }
    .search-input:focus { outline: none; border-color: var(--primary); box-shadow: 0 0 0 3px rgba(26,35,126,0.08); }
    select { padding: 0.5rem 0.75rem; border: 1px solid var(--border); border-radius: var(--radius); font-size: 0.875rem; background: white; }
    .btn-sm { padding: 0.375rem 0.75rem; font-size: 0.8125rem; display: inline-flex; align-items: center; gap: 0.25rem; }
    .btn-primary { background: var(--primary); color: white; border: none; border-radius: var(--radius); cursor: pointer; }
    .btn-primary:hover { background: var(--primary-light); }
    .btn-primary .material-icons { font-size: 1rem; }
    .btn-sm .material-icons { font-size: 1rem; }
    table { width: 100%; border-collapse: collapse; }
    th { text-align: left; padding: 0.75rem 1rem; font-size: 0.75rem; font-weight: 600; color: var(--text-secondary); text-transform: uppercase; letter-spacing: 0.5px; background: var(--bg); border-bottom: 1px solid var(--border-light); }
    td { padding: 0.75rem 1rem; font-size: 0.875rem; border-bottom: 1px solid var(--border-light); }
    .cell-primary { font-weight: 500; }
    .cell-muted { color: var(--text-secondary); font-size: 0.8125rem; }
    .cell-id { color: var(--text-muted); font-size: 0.75rem; margin-left: 0.375rem; font-family: monospace; }
    .action-badge { padding: 0.2rem 0.625rem; border-radius: 100px; font-size: 0.6875rem; font-weight: 500; }
    .action-badge[data-action="Created"] { background: #f0fdf4; color: #166534; }
    .action-badge[data-action="Updated"] { background: #eff6ff; color: #1e40af; }
    .action-badge[data-action="Deleted"] { background: #fef2f2; color: #991b1b; }
    .action-badge[data-action="StatusChanged"] { background: #fefce8; color: #854d0e; }
    .pagination { display: flex; justify-content: space-between; align-items: center; padding: 0.75rem 1rem; border-top: 1px solid var(--border-light); }
    .page-info { font-size: 0.8125rem; color: var(--text-secondary); }
    .page-buttons { display: flex; align-items: center; gap: 0.75rem; }
    .page-num { font-size: 0.8125rem; color: var(--text-secondary); }
    .btn-sm:disabled { opacity: 0.4; cursor: not-allowed; }
    .empty-state { display: flex; flex-direction: column; align-items: center; padding: 2rem; color: var(--text-muted); }
    .empty-state .material-icons { font-size: 2rem; margin-bottom: 0.5rem; opacity: 0.5; }
  `]
})
export class AuditLog implements OnInit {
  logs = signal<PaginatedResult<AuditLogDto>>({ items: [], totalCount: 0, page: 1, pageSize: 20, totalPages: 0, hasPrevious: false, hasNext: false });
  searchTerm = '';
  sortBy = 'timestamp';
  sortDescending = true;
  page = 1;
  pageSize = 20;
  Math = Math;

  constructor(private auditService: Audit) {}

  ngOnInit() { this.load(); }

  load() {
    this.auditService.getAll({
      search: this.searchTerm || undefined,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      page: this.page,
      pageSize: this.pageSize
    }).subscribe(data => this.logs.set(data));
  }
}
