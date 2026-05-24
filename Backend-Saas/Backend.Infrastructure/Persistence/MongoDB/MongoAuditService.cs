using Backend.Application.Common.Interfaces;
using Backend.Domain.Audit;
using MongoDB.Driver;

namespace Backend.Infrastructure.Persistence.MongoDB;

public class MongoAuditService : IAuditService
{
    private readonly IMongoCollection<AuditEntry> _collection;
    private readonly ICurrentUserService _currentUser;

    public MongoAuditService(IMongoDatabase database, ICurrentUserService currentUser)
    {
        _collection = database.GetCollection<AuditEntry>("audit_logs");
        _currentUser = currentUser;
    }

    public async Task LogAsync(string action, string entityType, string entityId, string? details = null)
    {
        var entry = new AuditEntry
        {
            Id = Guid.NewGuid().ToString(),
            UserId = _currentUser.UserId?.ToString(),
            UserEmail = _currentUser.Email,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            Timestamp = DateTime.UtcNow
        };

        await _collection.InsertOneAsync(entry);
    }
}
