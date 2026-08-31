namespace MedOps.Infrastructure.Data.Configurations;

using MedOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StudySiteConfiguration : IEntityTypeConfiguration<StudySite>
{
    public void Configure(EntityTypeBuilder<StudySite> builder)
    {
        builder.HasKey(ss => ss.Id);
        builder.Property(ss => ss.StudyId).IsRequired();
        builder.Property(ss => ss.SiteId).IsRequired();
        builder.Property(ss => ss.Role).IsRequired().HasMaxLength(100);
        builder.Property(ss => ss.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(ss => ss.CreatedAt).IsRequired();
        builder.HasIndex(ss => new { ss.StudyId, ss.SiteId }).IsUnique();
    }
}