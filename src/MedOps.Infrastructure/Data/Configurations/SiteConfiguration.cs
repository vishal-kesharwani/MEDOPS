namespace MedOps.Infrastructure.Data.Configurations;

using MedOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Description).HasMaxLength(2000);
        builder.Property(s => s.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(s => s.CreatedBy).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();
        builder.OwnsOne(s => s.Address, a =>
        {
            a.Property(ai => ai.Street).HasMaxLength(300);
            a.Property(ai => ai.City).HasMaxLength(100);
            a.Property(ai => ai.State).HasMaxLength(100);
            a.Property(ai => ai.Country).HasMaxLength(100);
            a.Property(ai => ai.ZipCode).HasMaxLength(20);
        });
        builder.OwnsOne(s => s.ContactInfo, ci =>
        {
            ci.Property(ai => ai.Email).HasMaxLength(256);
            ci.Property(ai => ai.Phone).HasMaxLength(20);
        });
        builder.HasMany(s => s.StudySites).WithOne().HasForeignKey(ss => ss.SiteId);
    }
}