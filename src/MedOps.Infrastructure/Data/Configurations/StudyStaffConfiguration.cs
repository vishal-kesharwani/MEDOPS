namespace MedOps.Infrastructure.Data.Configurations;

using MedOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StudyStaffConfiguration : IEntityTypeConfiguration<StudyStaff>
{
    public void Configure(EntityTypeBuilder<StudyStaff> builder)
    {
        builder.HasKey(ss => ss.Id);
        builder.Property(ss => ss.StudyId).IsRequired();
        builder.Property(ss => ss.UserId).IsRequired();
        builder.Property(ss => ss.Role).IsRequired();
        builder.Property(ss => ss.IsActive).IsRequired();
        builder.Property(ss => ss.AssignedAt).IsRequired();
    }
}