namespace MedOps.Infrastructure.Data.Configurations;

using MedOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StudyConfiguration : IEntityTypeConfiguration<Study>
{
    public void Configure(EntityTypeBuilder<Study> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Description).IsRequired().HasMaxLength(2000);
        builder.Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(s => s.CreatedBy).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();
        builder.HasMany(s => s.StudySites).WithOne().HasForeignKey(ss => ss.StudyId);
        builder.HasMany(s => s.StudyStaff).WithOne().HasForeignKey(ss => ss.StudyId);
        builder.HasMany(s => s.Tasks).WithOne().HasForeignKey(t => t.StudyId);
        builder.HasMany(s => s.Requests).WithOne().HasForeignKey(r => r.StudyId);
    }
}