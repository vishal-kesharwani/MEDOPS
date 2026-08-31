import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Task as TaskService, TaskDto, CreateTaskDto } from '../../services/task';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'app-tasks',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Tasks</h1>
          <p class="subtitle">Track and manage research tasks</p>
        </div>
        <button (click)="showForm.set(true)" class="btn-primary">
          <span class="material-icons">add</span>
          New Task
        </button>
      </div>

      @if (showForm()) {
        <div class="card form-card">
          <div class="card-header">
            <h3>{{ editingId() ? 'Edit' : 'Create' }} Task</h3>
          </div>
          <form (ngSubmit)="onSubmit()" class="card-body">
            <div class="form-grid">
              <div class="form-group">
                <label>Title</label>
                <input type="text" [(ngModel)]="form.title" name="title" required placeholder="Task title" />
              </div>
              <div class="form-group">
                <label>Priority</label>
                <select [(ngModel)]="form.priority" name="priority">
                  <option value="Low">Low</option>
                  <option value="Medium">Medium</option>
                  <option value="High">High</option>
                  <option value="Critical">Critical</option>
                </select>
              </div>
              <div class="form-group form-full">
                <label>Description</label>
                <textarea [(ngModel)]="form.description" name="description" required rows="3" placeholder="Describe the task"></textarea>
              </div>
            </div>
            <div class="form-actions">
              <button type="submit" class="btn-primary">
                <span class="material-icons">save</span>
                {{ editingId() ? 'Update' : 'Create' }}
              </button>
              <button type="button" (click)="cancelForm()" class="btn-secondary">Cancel</button>
            </div>
          </form>
        </div>
      }

      <div class="card">
        <table>
          <thead>
            <tr>
              <th>Title</th>
              <th>Status</th>
              <th>Priority</th>
              <th style="width: 200px;">Actions</th>
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
                    <button (click)="edit(task)" class="btn-icon" title="Edit">
                      <span class="material-icons">edit</span>
                    </button>
                    @if (task.status === 'ToDo') {
                      <button (click)="start(task.id)" class="btn-sm btn-primary">Start</button>
                    }
                    @if (task.status === 'InProgress') {
                      <button (click)="complete(task.id)" class="btn-sm btn-success">Complete</button>
                      <button (click)="cancel(task.id)" class="btn-sm btn-danger">Cancel</button>
                    }
                    <button (click)="delete(task.id)" class="btn-icon btn-danger" title="Delete">
                      <span class="material-icons">delete</span>
                    </button>
                  </div>
                </td>
              </tr>
            }
            @if (tasks().length === 0) {
              <tr><td colspan="4"><div class="empty-state"><span class="material-icons">task_alt</span><p>No tasks found</p></div></td></tr>
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
    .btn-icon { width: 32px; height: 32px; padding: 0; display: inline-flex; align-items: center; justify-content: center; background: transparent; color: var(--text-secondary); border-radius: var(--radius); }
    .btn-icon:hover { background: var(--bg); }
    .btn-icon.btn-danger:hover { background: #fef2f2; color: var(--danger); }
    .btn-icon .material-icons { font-size: 1.125rem; }
    .action-buttons { display: flex; gap: 0.25rem; align-items: center; }
    .cell-primary { font-weight: 500; }
    .badge { padding: 0.2rem 0.625rem; border-radius: 100px; font-size: 0.6875rem; font-weight: 500; text-transform: capitalize; }
    .badge[data-status="ToDo"] { background: #eff6ff; color: #1e40af; }
    .badge[data-status="InProgress"] { background: #fefce8; color: #854d0e; }
    .badge[data-status="Completed"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Cancelled"] { background: #f5f5f5; color: #737373; }
    .priority { font-size: 0.75rem; font-weight: 500; text-transform: capitalize; }
    .priority[data-priority="High"] { color: var(--danger); }
    .priority[data-priority="Critical"] { color: #7f1d1d; font-weight: 700; }
    .priority[data-priority="Medium"] { color: var(--warning); }
    .priority[data-priority="Low"] { color: var(--text-muted); }
    .btn-sm { padding: 0.25rem 0.625rem; font-size: 0.75rem; border-radius: var(--radius); font-weight: 500; }
    .btn-success { background: #f0fdf4; color: #166534; border: 1px solid #bbf7d0; }
    .btn-success:hover { background: #dcfce7; }
    .btn-danger { background: #fef2f2; color: #991b1b; border: 1px solid #fecaca; }
    .btn-danger:hover { background: #fee2e2; }
    .empty-state { display: flex; flex-direction: column; align-items: center; padding: 2rem; color: var(--text-muted); }
    .empty-state .material-icons { font-size: 2rem; margin-bottom: 0.5rem; opacity: 0.5; }
  `]
})
export class Tasks implements OnInit {
  tasks = signal<TaskDto[]>([]);
  showForm = signal(false);
  editingId = signal<string | null>(null);
  form: CreateTaskDto = { title: '', description: '', assignedTo: '00000000-0000-0000-0000-000000000000', priority: 'Medium' };

  constructor(private taskService: TaskService) {}

  ngOnInit() { this.load(); }

  load() { this.taskService.getAll().subscribe(data => this.tasks.set(data)); }

  onSubmit() {
    this.taskService.create(this.form).subscribe(() => { this.cancelForm(); this.load(); });
  }

  edit(task: TaskDto) {
    this.editingId.set(task.id);
    this.form = { title: task.title, description: task.description, assignedTo: task.assignedTo, priority: task.priority };
    this.showForm.set(true);
  }

  start(id: string) { this.taskService.start(id).subscribe(() => this.load()); }
  complete(id: string) { this.taskService.complete(id).subscribe(() => this.load()); }
  cancel(id: string) { this.taskService.cancel(id).subscribe(() => this.load()); }
  delete(id: string) { this.taskService.delete(id).subscribe(() => this.load()); }

  cancelForm() {
    this.showForm.set(false);
    this.editingId.set(null);
    this.form = { title: '', description: '', assignedTo: '00000000-0000-0000-0000-000000000000', priority: 'Medium' };
  }
}
