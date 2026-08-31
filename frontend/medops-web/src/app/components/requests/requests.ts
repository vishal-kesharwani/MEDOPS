import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Request as RequestService, RequestDto } from '../../services/request';

@Component({
  imports: [CommonModule],
  selector: 'app-requests',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Requests</h1>
          <p class="subtitle">Review and manage approval requests</p>
        </div>
      </div>

      <div class="card">
        <table>
          <thead>
            <tr>
              <th>Title</th>
              <th>Status</th>
              <th>Created</th>
              <th style="width: 200px;">Actions</th>
            </tr>
          </thead>
          <tbody>
            @for (req of requests(); track req.id) {
              <tr>
                <td><span class="cell-primary">{{ req.title }}</span></td>
                <td><span class="badge" [attr.data-status]="req.status">{{ req.status }}</span></td>
                <td class="cell-muted">{{ req.createdAt | date:'mediumDate' }}</td>
                <td>
                  @if (req.status === 'Pending') {
                    <div class="action-buttons">
                      <button (click)="approve(req.id)" class="btn-sm btn-success">
                        <span class="material-icons">check</span> Approve
                      </button>
                      <button (click)="reject(req.id)" class="btn-sm btn-danger">
                        <span class="material-icons">close</span> Reject
                      </button>
                      <button (click)="cancel(req.id)" class="btn-sm btn-secondary">
                        Cancel
                      </button>
                    </div>
                  }
                </td>
              </tr>
            }
            @if (requests().length === 0) {
              <tr>
                <td colspan="4">
                  <div class="empty-state">
                    <span class="material-icons">request_quote</span>
                    <p>No requests found</p>
                  </div>
                </td>
              </tr>
            }
          </tbody>
        </table>
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

    .card {
      background: white;
      border-radius: var(--radius-lg);
      box-shadow: var(--shadow-sm);
      border: 1px solid var(--border-light);
      overflow: hidden;
    }

    .cell-primary { font-weight: 500; }
    .cell-muted { color: var(--text-secondary); font-size: 0.8125rem; }

    .badge {
      padding: 0.2rem 0.625rem;
      border-radius: 100px;
      font-size: 0.6875rem;
      font-weight: 500;
      text-transform: capitalize;
    }
    .badge[data-status="Pending"] { background: #eff6ff; color: #1e40af; }
    .badge[data-status="Approved"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Rejected"] { background: #fef2f2; color: #991b1b; }
    .badge[data-status="Cancelled"] { background: #f5f5f5; color: #737373; }

    .action-buttons { display: flex; gap: 0.375rem; }

    .btn-sm {
      display: inline-flex;
      align-items: center;
      gap: 0.25rem;
      padding: 0.25rem 0.625rem;
      font-size: 0.75rem;
      border-radius: var(--radius);
      font-weight: 500;
    }
    .btn-sm .material-icons { font-size: 0.875rem; }
    .btn-success { background: #f0fdf4; color: #166534; border: 1px solid #bbf7d0; }
    .btn-success:hover { background: #dcfce7; }
    .btn-danger { background: #fef2f2; color: #991b1b; border: 1px solid #fecaca; }
    .btn-danger:hover { background: #fee2e2; }
    .btn-secondary {
      padding: 0.25rem 0.625rem;
      font-size: 0.75rem;
      background: white;
      color: var(--text-secondary);
      border: 1px solid var(--border);
    }
    .btn-secondary:hover { background: var(--bg); }

    .empty-state {
      display: flex;
      flex-direction: column;
      align-items: center;
      padding: 2rem;
      color: var(--text-muted);
    }
    .empty-state .material-icons { font-size: 2rem; margin-bottom: 0.5rem; opacity: 0.5; }
  `]
})
export class Requests implements OnInit {
  requests = signal<RequestDto[]>([]);

  constructor(private requestService: RequestService) {}

  ngOnInit() { this.load(); }

  load() { this.requestService.getAll().subscribe(data => this.requests.set(data)); }

  approve(id: string) { this.requestService.approve(id).subscribe(() => this.load()); }
  reject(id: string) { this.requestService.reject(id, 'Rejected by admin').subscribe(() => this.load()); }
  cancel(id: string) { this.requestService.cancel(id).subscribe(() => this.load()); }
}
