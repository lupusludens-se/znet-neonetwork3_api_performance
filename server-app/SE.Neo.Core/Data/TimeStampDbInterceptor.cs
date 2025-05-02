using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SE.Neo.Core.Entities.Interfaces;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Data
{
    public class TimeStampDbInterceptor : ISaveChangesInterceptor
    {
        private readonly ICurrentUserResolverService _currentUserResolverService;

        public TimeStampDbInterceptor(ICurrentUserResolverService currentUserResolverService)
        {
            _currentUserResolverService = currentUserResolverService;
        }

        public void SaveChangesFailed(DbContextErrorEventData eventData)
        {

        }

        public Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return result;
        }

        public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(Task.FromResult(result));
        }

        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateTimeStamps(eventData.Context);
            return result;
        }

        public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateTimeStamps(eventData.Context);
            return new ValueTask<InterceptionResult<int>>(Task.FromResult(result));
        }

        private void UpdateTimeStamps(DbContext context)
        {
            int? currentUserId = null;
            DateTime now = DateTime.UtcNow;

            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is ITimeStamp entityWithTimestamps)
                {
                    if (currentUserId == null)
                    {
                        currentUserId = _currentUserResolverService.GetCurrentUser()?.Id;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entityWithTimestamps.UpdatedByUserId = currentUserId;
                            entityWithTimestamps.ModifiedOn = now;
                            break;

                        case EntityState.Added:
                            entityWithTimestamps.CreatedByUserId = currentUserId;
                            entityWithTimestamps.CreatedOn = entityWithTimestamps.CreatedOn.HasValue
                                ? entityWithTimestamps.CreatedOn
                                : now;
                            entityWithTimestamps.UpdatedByUserId = entityWithTimestamps.CreatedByUserId;
                            entityWithTimestamps.ModifiedOn = entityWithTimestamps.CreatedOn;
                            break;
                    }
                }
            }
        }
    }
}
