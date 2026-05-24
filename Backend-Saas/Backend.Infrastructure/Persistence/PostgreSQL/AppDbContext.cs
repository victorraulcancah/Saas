namespace Backend.Infrastructure.Persistence.PostgreSQL;

using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Domain.Common.Interfaces;
using Backend.Domain.CRM.Entities;
using Backend.Domain.ERP.Entities;
using Backend.Domain.HR.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Module = Backend.Domain.Common.Module;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly ICurrentUserService _currentUser;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<TenantModule> TenantModules => Set<TenantModule>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Payroll> Payrolls => Set<Payroll>();

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
                var tenantId = _currentUser?.TenantId;
                var body = tenantId.HasValue
                    ? (Expression)Expression.Equal(property, Expression.Constant(tenantId.Value, typeof(Guid?)))
                    : Expression.Constant(true);
                var lambda = Expression.Lambda(body, parameter);
                entityType.SetQueryFilter(lambda);
            }

            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var body = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(body, parameter);
                entityType.SetQueryFilter(lambda);
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.TenantId == null)
            {
                entry.Entity.TenantId = _currentUser.TenantId;
            }
        }

        foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
