using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PersonService.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace PersonService.Data.Interceptors
{
    public class PersonInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public PersonInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var entries = eventData.Context.ChangeTracker.Entries<BaseEntity>();

            string? user = null;
            if (httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("X-Road-UserId", out var xroadUserId) == true) 
            {
                user = xroadUserId.FirstOrDefault();
            }

            UpdateBaseEntityFields(entries, user);
            AddPersonsChangeLog(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        public void UpdateBaseEntityFields(IEnumerable<EntityEntry<BaseEntity>> entries, string? user)
        {
            if (string.IsNullOrWhiteSpace(user))
                return;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
                if (!string.IsNullOrWhiteSpace(user))
                {
                    entry.Property(x => x.UpdatedBy).CurrentValue = user;
                }
            }
        }

        public void AddPersonsChangeLog(DbContextEventData eventData)
        {
            var context = eventData.Context as ApplicationDbContext;
            var modifiedEntities = context!.ChangeTracker.Entries<Person>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted).ToList();
                    
            foreach (var modifiedEntity in modifiedEntities)
            {
                var oldValue = GetOldValue(modifiedEntity);
                var newValue = GetNewValue(modifiedEntity);
                if (modifiedEntity.State == EntityState.Unchanged || (modifiedEntity.State == EntityState.Modified && oldValue.Equals(newValue)))
                {
                    //nothing has changed, don't save changes
                    continue;
                }
                var changeLog = new PersonChangeLog()
                {
                    PersonId = modifiedEntity.Entity.Id,
                    ChangeTime = DateTime.UtcNow,
                    ChangeType = modifiedEntity.State.ToString(),
                    OldValue = oldValue,
                    NewValue = newValue,
                    ChangedBy = modifiedEntity.Entity.UpdatedBy
                };
                if (modifiedEntity.State == EntityState.Added && oldValue.Equals(newValue))
                {
                    changeLog.OldValue = null;
                }
                if (modifiedEntity.State == EntityState.Deleted && oldValue.Equals(newValue))
                {
                    changeLog.NewValue = null;
                }

                context!.PersonChangeLogs.Add(changeLog);
            }
        }

        private string GetOldValue(EntityEntry<Person> modifiedEntity)
        {
            var values = new Dictionary<string, object>();
            foreach (var property in modifiedEntity.OriginalValues.Properties)
            {
                values[property.Name] = modifiedEntity.OriginalValues[property] ?? string.Empty;
            }
            return JsonSerializer.Serialize(values);
        }
        private string GetNewValue(EntityEntry<Person> modifiedEntity)
        {
            var values = new Dictionary<string, object>();
            foreach (var property in modifiedEntity.CurrentValues.Properties)
            {
                values[property.Name] = modifiedEntity.CurrentValues[property] ?? string.Empty;
            }
            return JsonSerializer.Serialize(values);
        }
    }
}
