namespace MedOps.Infrastructure.Data.Configurations;

using MedOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RequestApprovalConfiguration : IEntityTypeConfiguration<RequestApproval>
{
    public void Configure(EntityTypeBuilder<RequestApproval> builder)
    {
        builder.HasKey(ra => ra.Id);
        builder.Property(ra => ra.RequestId).IsRequired();
        builder.Property(ra => ra.ApprovedBy).IsRequired();
        builder.Property(ra => ra.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(ra => ra.Comment).HasMaxLength(2000);
        builder.Property(ra => ra.CommentedAt).IsRequired();
    }
}