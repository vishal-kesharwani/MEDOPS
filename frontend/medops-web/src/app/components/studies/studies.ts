import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Study, StudyDto, CreateStudyDto } from '../../services/study';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'app-studies',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Studies</h1>
          <p class="subtitle">Manage clinical research studies</p>
        </div>
        <button (click)="showForm.set(true)" class="btn-primary">
          <span class="material-icons">add</span>
          New Study
        </button>
      </div>

      @if (showForm()) {
        <div class="card form-card">
          <div class="card-header">
            <h3>{{ editingId() ? 'Edit' : 'Create' }} Study</h3>
          </div>
          <form (ngSubmit)="onSubmit()" class="card-body">
            <div class="form-grid">
              <div class="form-group">
                <label>Name</label>
                <input type="text" [(ngModel)]="form.name" name="name" required placeholder="Enter study name" />
              </div>
              <div class="form-group form-full">
                <label>Description</label>
                <textarea [(ngModel)]="form.description" name="description" required rows="3" placeholder="Describe the study"></textarea>
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
              <th>Name</th>
              <th>Status</th>
              <th>Created</th>
              <th style="width: 150px;">Actions</th>
            </tr>
          </thead>
          <tbody>
            @for (study of studies(); track study.id) {
              <tr>
                <td><span class="cell-primary">{{ study.name }}</span></td>
                <td><span class="badge" [attr.data-status]="study.status">{{ study.status }}</span></td>
                <td class="cell-muted">{{ study.createdAt | date:'mediumDate' }}</td>
                <td>
                  <div class="action-buttons">
                    <button (click)="edit(study)" class="btn-icon" title="Edit">
                      <span class="material-icons">edit</span>
                    </button>
                    <button (click)="delete(study.id)" class="btn-icon btn-danger" title="Delete">
                      <span class="material-icons">delete</span>
                    </button>
                  </div>
                </td>
              </tr>
            }
            @if (studies().length === 0) {
              <tr>
                <td colspan="4">
                  <div class="empty-state">
                    <span class="material-icons">science</span>
                    <p>No studies found</p>
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
      margin-bottom: 1rem;
    }

    .card-header {
      padding: 1rem 1.25rem;
      border-bottom: 1px solid var(--border-light);
    }
    .card-header h3 { font-size: 0.9375rem; }

    .card-body { padding: 1.25rem; }

    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }
    .form-full { grid-column: 1 / -1; }
    .form-group label { margin-bottom: 0.375rem; }

    .form-actions {
      display: flex;
      gap: 0.5rem;
      margin-top: 1rem;
      padding-top: 1rem;
      border-top: 1px solid var(--border-light);
    }

    .btn-primary {
      display: inline-flex;
      align-items: center;
      gap: 0.375rem;
      padding: 0.5rem 1rem;
      background: var(--primary);
      color: white;
    }
    .btn-primary:hover { background: var(--primary-light); }
    .btn-primary .material-icons { font-size: 1.125rem; }

    .btn-secondary {
      padding: 0.5rem 1rem;
      background: white;
      color: var(--text-secondary);
      border: 1px solid var(--border);
    }
    .btn-secondary:hover { background: var(--bg); }

    .btn-icon {
      width: 32px;
      height: 32px;
      padding: 0;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      background: transparent;
      color: var(--text-secondary);
      border-radius: var(--radius);
    }
    .btn-icon:hover { background: var(--bg); color: var(--text); }
    .btn-icon.btn-danger:hover { background: #fef2f2; color: var(--danger); }
    .btn-icon .material-icons { font-size: 1.125rem; }

    .action-buttons { display: flex; gap: 0.25rem; }

    .cell-primary { font-weight: 500; }
    .cell-muted { color: var(--text-secondary); font-size: 0.8125rem; }

    .badge {
      padding: 0.2rem 0.625rem;
      border-radius: 100px;
      font-size: 0.6875rem;
      font-weight: 500;
      text-transform: capitalize;
    }
    .badge[data-status="Planning"] { background: #eff6ff; color: #1e40af; }
    .badge[data-status="Active"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Completed"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Closed"] { background: #f5f5f5; color: #737373; }

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
export class Studies implements OnInit {
  studies = signal<StudyDto[]>([]);
  showForm = signal(false);
  editingId = signal<string | null>(null);
  form: CreateStudyDto = { name: '', description: '' };

  constructor(private studyService: Study) {}

  ngOnInit() { this.load(); }

  load() { this.studyService.getAll().subscribe(data => this.studies.set(data)); }

  onSubmit() {
    if (this.editingId()) {
      this.studyService.update(this.editingId()!, this.form).subscribe(() => { this.cancelForm(); this.load(); });
    } else {
      this.studyService.create(this.form).subscribe(() => { this.cancelForm(); this.load(); });
    }
  }

  edit(study: StudyDto) {
    this.editingId.set(study.id);
    this.form = { name: study.name, description: study.description };
    this.showForm.set(true);
  }

  delete(id: string) { this.studyService.delete(id).subscribe(() => this.load()); }

  cancelForm() { this.showForm.set(false); this.editingId.set(null); this.form = { name: '', description: '' }; }
}
