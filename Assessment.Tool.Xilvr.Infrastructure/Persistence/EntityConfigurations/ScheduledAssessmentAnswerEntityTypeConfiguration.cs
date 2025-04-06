using Assessment.Tool.Xilvr.Domain.Aggregates;
using Assessment.Tool.Xilvr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

public class ScheduledAssessmentAnswerConfiguration : IEntityTypeConfiguration<ScheduledAssessmentAnswer>
{
    public void Configure(EntityTypeBuilder<ScheduledAssessmentAnswer> builder)
    {
        builder.ToTable("scheduled_assessment_answer", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(aa => aa.Id);
        builder.Property(c => c.Id)
            .HasColumnName("scheduled_assessment_answer_id")
            .ValueGeneratedOnAdd();
        builder.Property(aa => aa.Answer)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("answer")
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(aa => aa.IsCorrect)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_correct")
            .IsRequired();
        builder.Property(aa => aa.Score)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("score")
            .IsRequired();

        builder.Property(c => c.ScheduledAssessmentId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("scheduled_assessment_id");
        builder.Property(c => c.QuestionId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("question_id");
        builder.Property(c => c.EmployeeId)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("employee_id");

        builder.HasOne(aa => aa.ScheduledAssessment)
            .WithMany()
            .HasForeignKey(aa => aa.ScheduledAssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(aa => aa.Question)
            .WithMany()
            .HasForeignKey(aa => aa.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(aa => aa.Employee)
            .WithMany()
            .HasForeignKey(aa => aa.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

