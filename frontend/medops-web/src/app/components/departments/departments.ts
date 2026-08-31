import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Department as DepartmentService, DepartmentDto } from '../../services/department';

@Component({
  imports: [CommonModule],
  selector: 'app-departments',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Departments</h1>
          <p class="subtitle">Manage organizational departments</p>
        </div>
      </div>

      <div class="card">
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Description</th>
              <th style="width: 80px;">Actions</th>
            </tr>
          </thead>
          <tbody>
            @for (dept of departments(); track dept.id) {
              <tr>
                <td><span class="cell-primary">{{ dept.name }}</span></td>
                <td class="cell-muted">{{ dept.description }}</td>
                <td>
                  <button (click)="delete(dept.id)" class="btn-icon btn-danger" title="Delete">
                    <span class="material-icons">delete</span>
                  </button>
                </td>
              </tr>
            }
            @if (departments().length === 0) {
              <tr>
                <td colspan="3">
                  <div class="empty-state">
                    <span class="material-icons">corporate_fare</span>
                    <p>No departments found</p>
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
export class Departments implements OnInit {
  departments = signal<DepartmentDto[]>([]);

  constructor(private departmentService: DepartmentService) {}

  ngOnInit() { this.load(); }

  load() { this.departmentService.getAll().subscribe(data => this.departments.set(data)); }

  delete(id: string) { this.departmentService.delete(id).subscribe(() => this.load()); }
}
