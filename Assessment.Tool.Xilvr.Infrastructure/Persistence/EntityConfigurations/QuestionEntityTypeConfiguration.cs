using Assessment.Tool.Xilvr.Domain.Entities;
using Assessment.Tool.Xilvr.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assessment.Tool.Xilvr.Infrastructure.Persistence.EntityConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("question", ApplicationContext.DEFAULT_SCHEMA);
        builder.HasKey(q => q.Id);
        builder.Property(c => c.Id)
            .HasColumnName("question_id")
            .ValueGeneratedOnAdd();
        builder.Property(q => q.Text)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("question_text")
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(q => q.QuestionType)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("question_type")
            .IsRequired();
        builder.Property(q => q.Points)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("question_points")
            .IsRequired();
        builder.Property(q => q.Options)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnType("jsonb")
            .HasColumnName("question_options")
            .IsRequired(false);
        builder.Property(q => q.Answer)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnType("jsonb")
            .HasColumnName("answer")
            .IsRequired(false);
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

        builder.HasOne<Assessment.Tool.Xilvr.Domain.Entities.Assessment>()
            .WithMany(a => a.Questions)
            .HasForeignKey(q => q.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
