using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

public class ScheduledAssessmentConfiguration : IEntityTypeConfiguration<ScheduledAssessment>
{
    public void Configure(EntityTypeBuilder<ScheduledAssessment> builder)
    {
        builder.ToTable("scheduled_assessment", ApplicationContext.DEFAULT_SCHEMA);
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
        builder.Property(c => c.CreatedDateTime)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_date")
            .IsRequired();
        builder.Property(c => c.UpdatedDateTime)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("updated_date");
        builder.Property(c => c.CreatedBy)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("created_by").HasMaxLength(Constants.CREATED_BY_AND_UPDATED_BY_LENGTH);
        builder.Property(c => c.UpdatedBy)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("updated_by").HasMaxLength(Constants.CREATED_BY_AND_UPDATED_BY_LENGTH);


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
