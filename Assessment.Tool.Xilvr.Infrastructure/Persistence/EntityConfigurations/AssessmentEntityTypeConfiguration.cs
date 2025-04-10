﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Assessment.Tool.Xilvr.Domain;
using Assessment.Tool.Xilvr.Shared.Constants;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment.Tool.Xilvr.Domain.Entities.Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment.Tool.Xilvr.Domain.Entities.Assessment> builder)
    {
        builder.ToTable("assessment", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(a => a.Id);
        builder.Property(c => c.Id)
            .HasColumnName("assessment_id")
            .ValueGeneratedOnAdd();
        builder.Property(a => a.Name)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("assessment_name")
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(a => a.TotalMarks)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("total_marks")
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

        builder
             .HasMany(a => a.Questions)
             .WithMany(q => q.Assessments)
             .UsingEntity(j =>
             {
                 j.ToTable("assessment_question", ApplicationContext.DEFAULT_SCHEMA);
                 j.Property<int>("AssessmentId").HasColumnName("assessment_id");
                 j.Property<int>("QuestionId").HasColumnName("question_id");
             });

        builder.HasMany(a => a.ScheduledAssessments)
           .WithOne(sa => sa.Assessment)
           .HasForeignKey(sa => sa.AssessmentId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
