using Assessment.Tool.Xilvr.Base.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Assessment.Tool.Xilvr.Application.BackgroundJobs;

public class ScheduledAssessmentStatusUpdater : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly ILogger<ScheduledAssessmentStatusUpdater> _logger;

    public ScheduledAssessmentStatusUpdater(IServiceScopeFactory serviceScopeFactory, ILogger<ScheduledAssessmentStatusUpdater> logger)
    {
        Ensure.IsNotNull(serviceScopeFactory, nameof(serviceScopeFactory));
        _serviceScopeFactory = serviceScopeFactory;
        Ensure.IsNotNull(logger, nameof(logger));
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Running ScheduledAssessmentStatusUpdater at {Time}", DateTime.UtcNow);

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var now = DateTime.UtcNow;

                var toInProgress = await dbContext.ScheduledAssessments
                    .Where(a => a.StartDate <= now && a.AssessmentStatus == Shared.Enum.AssessmentStatus.Upcoming)
                    .ToListAsync(stoppingToken);

                var toCompleted = await dbContext.ScheduledAssessments
                    .Where(a => a.EndDate <= now && a.AssessmentStatus == Shared.Enum.AssessmentStatus.InProgress)
                    .ToListAsync(stoppingToken);

                foreach (var a in toInProgress)
                    a.AssessmentStatus = Shared.Enum.AssessmentStatus.InProgress;

                foreach (var a in toCompleted)
                    a.AssessmentStatus = Shared.Enum.AssessmentStatus.Completed;

                if (toInProgress.Any() || toCompleted.Any())
                    await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ScheduledAssessmentStatusUpdater encountered an error.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
