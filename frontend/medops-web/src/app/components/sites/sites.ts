import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Site, SiteDto, CreateSiteDto } from '../../services/site';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'app-sites',
  template: `
    <div class="page">
      <div class="page-header">
        <div>
          <h1>Sites</h1>
          <p class="subtitle">Manage clinical research sites</p>
        </div>
        <button (click)="showForm.set(true)" class="btn-primary">
          <span class="material-icons">add</span>
          New Site
        </button>
      </div>

      @if (showForm()) {
        <div class="card form-card">
          <div class="card-header"><h3>{{ editingId() ? 'Edit' : 'Create' }} Site</h3></div>
          <form (ngSubmit)="onSubmit()" class="card-body">
            <div class="form-grid">
              <div class="form-group">
                <label>Name</label>
                <input type="text" [(ngModel)]="formName" name="name" required placeholder="Site name" />
              </div>
              <div class="form-group">
                <label>Description</label>
                <input type="text" [(ngModel)]="formDescription" name="description" required placeholder="Brief description" />
              </div>
              <div class="form-group">
                <label>Street</label>
                <input type="text" [(ngModel)]="formStreet" name="street" placeholder="Street address" />
              </div>
              <div class="form-group">
                <label>City</label>
                <input type="text" [(ngModel)]="formCity" name="city" placeholder="City" />
              </div>
              <div class="form-group">
                <label>State</label>
                <input type="text" [(ngModel)]="formState" name="state" placeholder="State" />
              </div>
              <div class="form-group">
                <label>Country</label>
                <input type="text" [(ngModel)]="formCountry" name="country" placeholder="Country" />
              </div>
              <div class="form-group">
                <label>Zip Code</label>
                <input type="text" [(ngModel)]="formZipCode" name="zipCode" placeholder="Zip code" />
              </div>
              <div class="form-group">
                <label>Contact Email</label>
                <input type="email" [(ngModel)]="formContactEmail" name="contactEmail" placeholder="email&#64;example.com" />
              </div>
              <div class="form-group">
                <label>Contact Phone</label>
                <input type="tel" [(ngModel)]="formContactPhone" name="contactPhone" placeholder="+1 555-0100" />
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
            <tr><th>Name</th><th>Status</th><th>City</th><th style="width: 180px;">Actions</th></tr>
          </thead>
          <tbody>
            @for (site of sites(); track site.id) {
              <tr>
                <td><span class="cell-primary">{{ site.name }}</span></td>
                <td><span class="badge" [attr.data-status]="site.status">{{ site.status }}</span></td>
                <td class="cell-muted">{{ site.address?.city || '—' }}</td>
                <td>
                  <div class="action-buttons">
                    <button (click)="edit(site)" class="btn-icon" title="Edit"><span class="material-icons">edit</span></button>
                    @if (site.status === 'Active') {
                      <button (click)="deactivate(site.id)" class="btn-sm btn-warning">Deactivate</button>
                    } @else {
                      <button (click)="activate(site.id)" class="btn-sm btn-success">Activate</button>
                    }
                    <button (click)="delete(site.id)" class="btn-icon btn-danger" title="Delete"><span class="material-icons">delete</span></button>
                  </div>
                </td>
              </tr>
            }
            @if (sites().length === 0) {
              <tr><td colspan="4"><div class="empty-state"><span class="material-icons">business</span><p>No sites found</p></div></td></tr>
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
    .form-group label { margin-bottom: 0.375rem; }
    .form-actions { display: flex; gap: 0.5rem; margin-top: 1rem; padding-top: 1rem; border-top: 1px solid var(--border-light); }
    .btn-primary { display: inline-flex; align-items: center; gap: 0.375rem; padding: 0.5rem 1rem; background: var(--primary); color: white; }
    .btn-primary:hover { background: var(--primary-light); }
    .btn-primary .material-icons { font-size: 1.125rem; }
    .btn-secondary { padding: 0.5rem 1rem; background: white; color: var(--text-secondary); border: 1px solid var(--border); }
    .btn-secondary:hover { background: var(--bg); }
    .btn-icon { width: 32px; height: 32px; padding: 0; display: inline-flex; align-items: center; justify-content: center; background: transparent; color: var(--text-secondary); border-radius: var(--radius); }
    .btn-icon:hover { background: var(--bg); color: var(--text); }
    .btn-icon.btn-danger:hover { background: #fef2f2; color: var(--danger); }
    .btn-icon .material-icons { font-size: 1.125rem; }
    .action-buttons { display: flex; gap: 0.25rem; align-items: center; }
    .cell-primary { font-weight: 500; }
    .cell-muted { color: var(--text-secondary); font-size: 0.8125rem; }
    .badge { padding: 0.2rem 0.625rem; border-radius: 100px; font-size: 0.6875rem; font-weight: 500; text-transform: capitalize; }
    .badge[data-status="Active"] { background: #f0fdf4; color: #166534; }
    .badge[data-status="Inactive"] { background: #f5f5f5; color: #737373; }
    .btn-sm { padding: 0.25rem 0.625rem; font-size: 0.75rem; border-radius: var(--radius); font-weight: 500; }
    .btn-success { background: #f0fdf4; color: #166534; border: 1px solid #bbf7d0; }
    .btn-success:hover { background: #dcfce7; }
    .btn-warning { background: #fffbeb; color: #92400e; border: 1px solid #fde68a; }
    .btn-warning:hover { background: #fef3c7; }
    .empty-state { display: flex; flex-direction: column; align-items: center; padding: 2rem; color: var(--text-muted); }
    .empty-state .material-icons { font-size: 2rem; margin-bottom: 0.5rem; opacity: 0.5; }
  `]
})
export class Sites implements OnInit {
  sites = signal<SiteDto[]>([]);
  showForm = signal(false);
  editingId = signal<string | null>(null);
  formName = '';
  formDescription = '';
  formStreet = '';
  formCity = '';
  formState = '';
  formCountry = '';
  formZipCode = '';
  formContactEmail = '';
  formContactPhone = '';

  constructor(private siteService: Site) {}

  ngOnInit() { this.load(); }

  load() { this.siteService.getAll().subscribe(data => this.sites.set(data)); }

  onSubmit() {
    const dto: CreateSiteDto = {
      name: this.formName, description: this.formDescription,
      address: { street: this.formStreet, city: this.formCity, state: this.formState, country: this.formCountry, zipCode: this.formZipCode },
      contactInfo: { email: this.formContactEmail, phone: this.formContactPhone }
    };
    if (this.editingId()) {
      this.siteService.update(this.editingId()!, dto).subscribe(() => { this.cancelForm(); this.load(); });
    } else {
      this.siteService.create(dto).subscribe(() => { this.cancelForm(); this.load(); });
    }
  }

  edit(site: SiteDto) {
    this.editingId.set(site.id);
    this.formName = site.name;
    this.formDescription = site.description;
    this.formStreet = site.address?.street || '';
    this.formCity = site.address?.city || '';
    this.formState = site.address?.state || '';
    this.formCountry = site.address?.country || '';
    this.formZipCode = site.address?.zipCode || '';
    this.formContactEmail = site.contactInfo?.email || '';
    this.formContactPhone = site.contactInfo?.phone || '';
    this.showForm.set(true);
  }

  activate(id: string) { this.siteService.activate(id).subscribe(() => this.load()); }
  deactivate(id: string) { this.siteService.deactivate(id).subscribe(() => this.load()); }
  delete(id: string) { this.siteService.delete(id).subscribe(() => this.load()); }

  cancelForm() {
    this.showForm.set(false); this.editingId.set(null);
    this.formName = ''; this.formDescription = '';
    this.formStreet = ''; this.formCity = ''; this.formState = ''; this.formCountry = ''; this.formZipCode = '';
    this.formContactEmail = ''; this.formContactPhone = '';
  }
}
