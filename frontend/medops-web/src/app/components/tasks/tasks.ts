import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task as TaskService, TaskDto } from '../../services/task';

@Component({
  imports: [CommonModule],
  selector: 'app-tasks',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Tasks</h1>
          <p class="subtitle">Track and manage research tasks</p>
        </div>
      </div>

      <div class="card">
        <table>
          <thead>
            <tr>
              <th>Title</th>
              <th>Status</th>
              <th>Priority</th>
              <th style="width: 150px;">Actions</th>
            </tr>
          </thead>
          <tbody>
            @for (task of tasks(); track task.id) {
              <tr>
                <td><span class="cell-primary">{{ task.title }}</span></td>
                <td><span class="badge" [attr.data-status]="task.status">{{ task.status }}</span></td>
                <td><span class="priority" [attr.data-priority]="task.priority">{{ task.priority }}</span></td>
                <td>
                  <div class="action-buttons">
                    @if (task.status === 'ToDo') {
                      <button (click)="start(task.id)" class="btn-sm btn-primary">
                        <span class="material-icons">play_arrow</span> Start
                      </button>
                    }
                    @if (task.status === 'InProgress') {
                      <button (click)="complete(task.id)" class="btn-sm btn-success">
                        <span class="material-icons">check</span> Complete
                      </button>
                      <button (click)="cancel(task.id)" class="btn-sm btn-danger">
                        <span class="material-icons">close</span> Cancel
                      </button>
                    }
                  </div>
                </td>
              </tr>
            }
            @if (tasks().length === 0) {
              <tr>
                <td colspan="4">
                  <div class="empty-state">
                    <span class="material-icons">task_alt</span>
                    <p>No tasks found</p>
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
    .badge[data-status="ToDo"] { background: #eff6ff; color: #1e40af; }
    .badge[data-status="InProgress"] { background: #fefce8; color: #854d0e; }
    .badge[data-status="Completed"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Cancelled"] { background: #f5f5f5; color: #737373; }

    .priority {
      font-size: 0.75rem;
      font-weight: 500;
      text-transform: capitalize;
    }
    .priority[data-priority="High"] { color: var(--danger); }
    .priority[data-priority="Medium"] { color: var(--warning); }
    .priority[data-priority="Low"] { color: var(--text-muted); }

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
    .btn-primary { background: var(--primary); color: white; }
    .btn-primary:hover { background: var(--primary-light); }
    .btn-success { background: #f0fdf4; color: #166534; border: 1px solid #bbf7d0; }
    .btn-success:hover { background: #dcfce7; }
    .btn-danger { background: #fef2f2; color: #991b1b; border: 1px solid #fecaca; }
    .btn-danger:hover { background: #fee2e2; }

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
export class Tasks implements OnInit {
  tasks = signal<TaskDto[]>([]);

  constructor(private taskService: TaskService) {}

  ngOnInit() { this.load(); }

  load() { this.taskService.getAll().subscribe(data => this.tasks.set(data)); }

  start(id: string) { this.taskService.start(id).subscribe(() => this.load()); }
  complete(id: string) { this.taskService.complete(id).subscribe(() => this.load()); }
  cancel(id: string) { this.taskService.cancel(id).subscribe(() => this.load()); }
}
