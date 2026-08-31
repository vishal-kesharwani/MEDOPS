import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Site, SiteDto } from '../../services/site';

@Component({
  imports: [CommonModule],
  selector: 'app-sites',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Sites</h1>
          <p class="subtitle">Manage clinical research sites</p>
        </div>
      </div>

      <div class="card">
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Status</th>
              <th style="width: 150px;">Actions</th>
            </tr>
          </thead>
          <tbody>
            @for (site of sites(); track site.id) {
              <tr>
                <td><span class="cell-primary">{{ site.name }}</span></td>
                <td><span class="badge" [attr.data-status]="site.status">{{ site.status }}</span></td>
                <td>
                  <div class="action-buttons">
                    @if (site.status === 'Active') {
                      <button (click)="deactivate(site.id)" class="btn-sm btn-warning">Deactivate</button>
                    } @else {
                      <button (click)="activate(site.id)" class="btn-sm btn-success">Activate</button>
                    }
                    <button (click)="delete(site.id)" class="btn-icon btn-danger" title="Delete">
                      <span class="material-icons">delete</span>
                    </button>
                  </div>
                </td>
              </tr>
            }
            @if (sites().length === 0) {
              <tr>
                <td colspan="3">
                  <div class="empty-state">
                    <span class="material-icons">business</span>
                    <p>No sites found</p>
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

    .badge {
      padding: 0.2rem 0.625rem;
      border-radius: 100px;
      font-size: 0.6875rem;
      font-weight: 500;
      text-transform: capitalize;
    }
    .badge[data-status="Active"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Inactive"] { background: #f5f5f5; color: #737373; }

    .action-buttons { display: flex; gap: 0.375rem; align-items: center; }

    .btn-sm {
      padding: 0.25rem 0.625rem;
      font-size: 0.75rem;
      border-radius: var(--radius);
      font-weight: 500;
    }
    .btn-success { background: #f0fdf4; color: #166534; border: 1px solid #bbf7d0; }
    .btn-success:hover { background: #dcfce7; }
    .btn-warning { background: #fffbeb; color: #92400e; border: 1px solid #fde68a; }
    .btn-warning:hover { background: #fef3c7; }

    .btn-icon {
      width: 30px;
      height: 30px;
      padding: 0;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      background: transparent;
      color: var(--text-secondary);
      border-radius: var(--radius);
    }
    .btn-icon:hover { background: var(--bg); }
    .btn-icon.btn-danger:hover { background: #fef2f2; color: var(--danger); }
    .btn-icon .material-icons { font-size: 1rem; }

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
export class Sites implements OnInit {
  sites = signal<SiteDto[]>([]);

  constructor(private siteService: Site) {}

  ngOnInit() { this.load(); }

  load() { this.siteService.getAll().subscribe(data => this.sites.set(data)); }

  activate(id: string) { this.siteService.activate(id).subscribe(() => this.load()); }
  deactivate(id: string) { this.siteService.deactivate(id).subscribe(() => this.load()); }
  delete(id: string) { this.siteService.delete(id).subscribe(() => this.load()); }
}
