using Assessment.Tool.Xilvr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

public class ScheduledAssessmentConfiguration : IEntityTypeConfiguration<ScheduledAssessment>
{
    public void Configure(EntityTypeBuilder<ScheduledAssessment> builder)
    {
        builder.ToTable("scheduledAssessments", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(sa => sa.Id);
        builder.Property(c => c.Id)
            .HasColumnName("scheduled_assessment_id")
            .ValueGeneratedOnAdd();
        builder.Property(sa => sa.AssessmentDuration)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("assessment_duration")
            .IsRequired();
        builder.Property(sa => sa.StartDate)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("start_date")
            .IsRequired();
        builder.Property(sa => sa.EndDate)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("end_date")
            .IsRequired();
        builder.Property(sa => sa.AssessmentStatus)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("status")
            .IsRequired();
        builder.Property(sa => sa.CanRandomizeQuestion)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("can_randomize")
            .IsRequired();
        builder.Property(sa => sa.CanDisplayResult)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("can_display_result")
            .IsRequired();
        builder.Property(sa => sa.CanSubmitBeforeEnd)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("can_submit_before_end")
            .IsRequired();
        builder.Property(sa => sa.Link)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("link")
            .HasMaxLength(500);


        builder.HasOne<Batch>()
            .WithOne()
            .HasForeignKey<ScheduledAssessment>(sa => sa.BatchId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Assessment.Tool.Xilvr.Domain.Entities.Assessment>()
            .WithOne()
            .HasForeignKey<ScheduledAssessment>(sa => sa.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
