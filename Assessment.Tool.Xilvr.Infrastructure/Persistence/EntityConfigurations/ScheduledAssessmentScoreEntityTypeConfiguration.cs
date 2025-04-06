using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

public class ScheduledAssessmentScoreConfiguration : IEntityTypeConfiguration<ScheduledAssessmentScore>
{
    public void Configure(EntityTypeBuilder<ScheduledAssessmentScore> builder)
    {
        builder.ToTable("scheduled_assessment_score", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(aa => aa.Id);
        builder.Property(c => c.Id)
            .HasColumnName("scheduled_assessment_score_id")
            .ValueGeneratedOnAdd();
        builder.Property(aa => aa.Score)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("total_score")
            .IsRequired();
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

        builder.Property(c => c.ScheduledAssessmentId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("scheduled_assessment_id");
        builder.Property(c => c.EmployeeId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("employee_id");

        builder.HasOne(sas => sas.ScheduledAssessment)
            .WithMany()
            .HasForeignKey(sas => sas.ScheduledAssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sas => sas.Employee)
            .WithMany()
            .HasForeignKey(sas => sas.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
