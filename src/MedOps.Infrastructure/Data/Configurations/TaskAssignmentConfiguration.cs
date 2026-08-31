namespace MedOps.Infrastructure.Data.Configurations;

using MedOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
{
    public void Configure(EntityTypeBuilder<TaskAssignment> builder)
    {
        builder.HasKey(ta => ta.Id);
        builder.Property(ta => ta.TaskId).IsRequired();
        builder.Property(ta => ta.AssignedTo).IsRequired();
        builder.Property(ta => ta.IsCompleted).IsRequired();
        builder.Property(ta => ta.AssignedDate).IsRequired();
        builder.HasIndex(ta => ta.TaskId);
    }
}