namespace Backend.SharedKernel.Common;

using Backend.SharedKernel.Common.Interfaces;

public abstract class BaseEntity : ISoftDelete
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
