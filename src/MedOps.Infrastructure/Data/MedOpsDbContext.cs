namespace MedOps.Infrastructure.Data;

using MedOps.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class MedOpsDbContext : IdentityDbContext<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole<Guid>, Guid>
{
    public DbSet<Study> Studies => Set<Study>();
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<StudySite> StudySites => Set<StudySite>();
    public DbSet<StudyStaff> StudyStaffs => Set<StudyStaff>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<MedOps.Domain.Entities.Task> Tasks => Set<MedOps.Domain.Entities.Task>();
    public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<RequestApproval> RequestApprovals => Set<RequestApproval>();

    public MedOpsDbContext(DbContextOptions<MedOpsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new Data.Configurations.StudyConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.SiteConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.StudySiteConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.StudyStaffConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.TaskConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.TaskAssignmentConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.RequestConfiguration());
        modelBuilder.ApplyConfiguration(new Data.Configurations.RequestApprovalConfiguration());
    }
}