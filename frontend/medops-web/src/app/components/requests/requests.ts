import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Request as RequestService, RequestDto } from '../../services/request';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'app-requests',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Requests</h1>
          <p class="subtitle">Review and manage approval requests</p>
        </div>
        <button (click)="showForm.set(true); formTitle=''; formDescription=''; formPriority='Medium'" class="btn-primary">
          <span class="material-icons">add</span>
          New Request
        </button>
      </div>

      @if (showForm()) {
        <div class="card form-card">
          <div class="card-header"><h3>Create Request</h3></div>
          <form (ngSubmit)="onSubmit()" class="card-body">
            <div class="form-grid">
              <div class="form-group">
                <label>Title</label>
                <input type="text" [(ngModel)]="formTitle" name="title" required placeholder="Request title" />
              </div>
              <div class="form-group">
                <label>Priority</label>
                <select [(ngModel)]="formPriority" name="priority">
                  <option value="Low">Low</option>
                  <option value="Medium">Medium</option>
                  <option value="High">High</option>
                </select>
              </div>
              <div class="form-group form-full">
                <label>Description</label>
                <textarea [(ngModel)]="formDescription" name="description" required rows="3" placeholder="Describe the request"></textarea>
              </div>
            </div>
            <div class="form-actions">
              <button type="submit" class="btn-primary"><span class="material-icons">send</span> Submit Request</button>
              <button type="button" (click)="cancelForm()" class="btn-secondary">Cancel</button>
            </div>
          </form>
        </div>
      }

      <div class="card">
        <table>
          <thead>
            <tr><th>Title</th><th>Status</th><th>Priority</th><th>Created</th><th style="width: 200px;">Actions</th></tr>
          </thead>
          <tbody>
            @for (req of requests(); track req.id) {
              <tr>
                <td><span class="cell-primary">{{ req.title }}</span></td>
                <td><span class="badge" [attr.data-status]="req.status">{{ req.status }}</span></td>
                <td><span class="priority" [attr.data-priority]="req.priority">{{ req.priority }}</span></td>
                <td class="cell-muted">{{ req.createdAt | date:'mediumDate' }}</td>
                <td>
                  @if (req.status === 'Pending') {
                    <div class="action-buttons">
                      <button (click)="approve(req.id)" class="btn-sm btn-success">Approve</button>
                      <button (click)="reject(req.id)" class="btn-sm btn-danger">Reject</button>
                    </div>
                  }
                </td>
              </tr>
            }
            @if (requests().length === 0) {
              <tr><td colspan="5"><div class="empty-state"><span class="material-icons">request_quote</span><p>No requests found</p></div></td></tr>
            }
          </tbody>
        </table>
      </div>
    </div>
  `,
  styles: [`
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 1.5rem; }
    .page-header h1 { margin-bottom: 0.125rem; }
    .subtitle { color: var(--text-secondary); font-size: 0.875rem; }
    .card { background: white; border-radius: var(--radius-lg); box-shadow: var(--shadow-sm); border: 1px solid var(--border-light); overflow: hidden; margin-bottom: 1rem; }
    .card-header { padding: 1rem 1.25rem; border-bottom: 1px solid var(--border-light); }
    .card-header h3 { font-size: 0.9375rem; }
    .card-body { padding: 1.25rem; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }
    .form-full { grid-column: 1 / -1; }
    .form-group label { margin-bottom: 0.375rem; }
    .form-actions { display: flex; gap: 0.5rem; margin-top: 1rem; padding-top: 1rem; border-top: 1px solid var(--border-light); }
    .btn-primary { display: inline-flex; align-items: center; gap: 0.375rem; padding: 0.5rem 1rem; background: var(--primary); color: white; }
    .btn-primary:hover { background: var(--primary-light); }
    .btn-primary .material-icons { font-size: 1.125rem; }
    .btn-secondary { padding: 0.5rem 1rem; background: white; color: var(--text-secondary); border: 1px solid var(--border); }
    .btn-secondary:hover { background: var(--bg); }
    .cell-primary { font-weight: 500; }
    .cell-muted { color: var(--text-secondary); font-size: 0.8125rem; }
    .badge { padding: 0.2rem 0.625rem; border-radius: 100px; font-size: 0.6875rem; font-weight: 500; text-transform: capitalize; }
    .badge[data-status="Pending"] { background: #eff6ff; color: #1e40af; }
    .badge[data-status="Approved"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Rejected"] { background: #fef2f2; color: #991b1b; }
    .badge[data-status="Cancelled"] { background: #f5f5f5; color: #737373; }
    .priority { font-size: 0.75rem; font-weight: 500; text-transform: capitalize; }
    .priority[data-priority="High"] { color: var(--danger); }
    .priority[data-priority="Medium"] { color: var(--warning); }
    .priority[data-priority="Low"] { color: var(--text-muted); }
    .action-buttons { display: flex; gap: 0.375rem; }
    .btn-sm { padding: 0.25rem 0.625rem; font-size: 0.75rem; border-radius: var(--radius); font-weight: 500; }
    .btn-success { background: #f0fdf4; color: #166534; border: 1px solid #bbf7d0; }
    .btn-success:hover { background: #dcfce7; }
    .btn-danger { background: #fef2f2; color: #991b1b; border: 1px solid #fecaca; }
    .btn-danger:hover { background: #fee2e2; }
    .empty-state { display: flex; flex-direction: column; align-items: center; padding: 2rem; color: var(--text-muted); }
    .empty-state .material-icons { font-size: 2rem; margin-bottom: 0.5rem; opacity: 0.5; }
  `]
})
export class Requests implements OnInit {
  requests = signal<RequestDto[]>([]);
  showForm = signal(false);
  formTitle = '';
  formDescription = '';
  formPriority = 'Medium';

  constructor(private requestService: RequestService) {}

  ngOnInit() { this.load(); }

  load() { this.requestService.getAll().subscribe(data => this.requests.set(data)); }

  onSubmit() {
    this.requestService.create({
      title: this.formTitle, description: this.formDescription, priority: this.formPriority
    }).subscribe(() => { this.cancelForm(); this.load(); });
  }

  approve(id: string) { this.requestService.approve(id).subscribe(() => this.load()); }
  reject(id: string) { this.requestService.reject(id, 'Rejected by admin').subscribe(() => this.load()); }

  cancelForm() { this.showForm.set(false); this.formTitle = ''; this.formDescription = ''; this.formPriority = 'Medium'; }
}
