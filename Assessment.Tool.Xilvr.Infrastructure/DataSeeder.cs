using Assessment.Tool.Xilvr.Application.DataSeeders;
using Assessment.Tool.Xilvr.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Assessment.Tool.Xilvr.Infrastructure;

/// <summary>
/// Seeder class for user management
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Seed the data
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        SeedUserStatus(modelBuilder);
    }

    /// <summary>
    /// seed user status
    /// </summary>
    /// <param name="modelBuilder"></param>
    private static void SeedUserStatus(ModelBuilder modelBuilder)
    {
        foreach (var status in UserStatusSeeder.GetData())
        {
            modelBuilder.Entity<UserStatus>().HasData(status);
        }
    }
}
