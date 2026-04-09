using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data.Configurations
{
    public class TaskLabelConfiguration : IEntityTypeConfiguration<TaskLabel>
    {
        public void Configure(EntityTypeBuilder<TaskLabel> builder)
        {
            builder.HasKey(tl => new { tl.LabelId, tl.TaskId });

            builder.HasOne(tl => tl.Task)
                .WithMany(t => t.TaskLabels)
                .HasForeignKey(tl => tl.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tl => tl.Label)
                .WithMany(l => l.TaskLabels)
                .HasForeignKey(tl => tl.LabelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
