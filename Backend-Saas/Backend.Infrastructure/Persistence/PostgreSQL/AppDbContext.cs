namespace Backend.Infrastructure.Persistence.PostgreSQL;

using Backend.Application.Common.Interfaces;
using Backend.Domain.BI.Entities;
using Backend.Domain.Common;
using Backend.Domain.CRM.Entities;
using Backend.Domain.ERP.Entities;
using Backend.Domain.HelpDesk.Entities;
using Backend.Domain.HR.Entities;
using Backend.Domain.LossPrevention.Entities;
using Backend.Domain.OMS.Entities;
using Backend.Domain.PIM.Entities;
using Backend.Domain.POS.Entities;
using Backend.Domain.RetailAnalytics.Entities;
using Backend.Domain.Saas.Entities;
using Backend.Domain.SFA.Entities;
using Backend.Domain.TMS.Entities;
using Backend.Domain.WMS.Entities;
using Backend.SharedKernel.Common.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly ICurrentUserService _currentUser;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<SaasSystem> SaasSystems => Set<SaasSystem>();
    public DbSet<SaasModule> SaasModules => Set<SaasModule>();
    public DbSet<SaasSubModule> SaasSubModules => Set<SaasSubModule>();
    public DbSet<SaasPlan> SaasPlans => Set<SaasPlan>();
    public DbSet<PlanSystem> PlanSystems => Set<PlanSystem>();
    public DbSet<PlanModule> PlanModules => Set<PlanModule>();
    public DbSet<PlanSubModule> PlanSubModules => Set<PlanSubModule>();
    public DbSet<TenantSubscription> TenantSubscriptions => Set<TenantSubscription>();
    public DbSet<TenantSystemLicense> TenantSystemLicenses => Set<TenantSystemLicense>();
    public DbSet<TenantModuleLicense> TenantModuleLicenses => Set<TenantModuleLicense>();
    public DbSet<TenantSubModuleLicense> TenantSubModuleLicenses => Set<TenantSubModuleLicense>();
    public DbSet<TenantCompanyProfile> TenantCompanyProfiles => Set<TenantCompanyProfile>();
    public DbSet<TenantBranch> TenantBranches => Set<TenantBranch>();
    public DbSet<TenantSunatCredential> TenantSunatCredentials => Set<TenantSunatCredential>();
    public DbSet<TenantCertificate> TenantCertificates => Set<TenantCertificate>();
    public DbSet<InvoiceSeries> InvoiceSeries => Set<InvoiceSeries>();

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();
    public DbSet<ProductStock> ProductStocks => Set<ProductStock>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<GoodsReceipt> GoodsReceipts => Set<GoodsReceipt>();
    public DbSet<GoodsReceiptItem> GoodsReceiptItems => Set<GoodsReceiptItem>();
    public DbSet<WarehouseTransfer> WarehouseTransfers => Set<WarehouseTransfer>();
    public DbSet<WarehouseTransferItem> WarehouseTransferItems => Set<WarehouseTransferItem>();
    public DbSet<StockAdjustment> StockAdjustments => Set<StockAdjustment>();
    public DbSet<DispatchGuide> DispatchGuides => Set<DispatchGuide>();
    public DbSet<DispatchGuideItem> DispatchGuideItems => Set<DispatchGuideItem>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Payroll> Payrolls => Set<Payroll>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<EmployeeShift> EmployeeShifts => Set<EmployeeShift>();
    public DbSet<Commission> Commissions => Set<Commission>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<Vacation> Vacations => Set<Vacation>();

    public DbSet<OmnichannelOrder> OmnichannelOrders => Set<OmnichannelOrder>();
    public DbSet<OmnichannelOrderItem> OmnichannelOrderItems => Set<OmnichannelOrderItem>();
    public DbSet<FulfillmentAssignment> FulfillmentAssignments => Set<FulfillmentAssignment>();
    public DbSet<SalesChannel> SalesChannels => Set<SalesChannel>();
    public DbSet<ChannelSyncLog> ChannelSyncLogs => Set<ChannelSyncLog>();
    public DbSet<OrderRoute> OrderRoutes => Set<OrderRoute>();

    public DbSet<WarehouseTask> WarehouseTasks => Set<WarehouseTask>();
    public DbSet<PickingTask> PickingTasks => Set<PickingTask>();
    public DbSet<PickingTaskItem> PickingTaskItems => Set<PickingTaskItem>();
    public DbSet<PackingTask> PackingTasks => Set<PackingTask>();
    public DbSet<ReplenishmentOrder> ReplenishmentOrders => Set<ReplenishmentOrder>();
    public DbSet<ReplenishmentOrderItem> ReplenishmentOrderItems => Set<ReplenishmentOrderItem>();
    public DbSet<WarehouseZone> WarehouseZones => Set<WarehouseZone>();
    public DbSet<StorageBin> StorageBins => Set<StorageBin>();
    public DbSet<DeliveryRoute> DeliveryRoutes => Set<DeliveryRoute>();
    public DbSet<RoutePlan> RoutePlans => Set<RoutePlan>();
    public DbSet<RouteStop> RouteStops => Set<RouteStop>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<ProofOfDelivery> ProofOfDeliveries => Set<ProofOfDelivery>();
    public DbSet<GpsTrackPoint> GpsTrackPoints => Set<GpsTrackPoint>();
    public DbSet<ProductContent> ProductContents => Set<ProductContent>();
    public DbSet<ProductMedia> ProductMedias => Set<ProductMedia>();
    public DbSet<ProductAttributeSet> ProductAttributeSets => Set<ProductAttributeSet>();
    public DbSet<ChannelPublication> ChannelPublications => Set<ChannelPublication>();
    public DbSet<FieldOrder> FieldOrders => Set<FieldOrder>();
    public DbSet<FieldVisit> FieldVisits => Set<FieldVisit>();
    public DbSet<SalesRoute> SalesRoutes => Set<SalesRoute>();
    public DbSet<RouteCustomer> RouteCustomers => Set<RouteCustomer>();
    public DbSet<PreOrder> PreOrders => Set<PreOrder>();
    public DbSet<PreOrderItem> PreOrderItems => Set<PreOrderItem>();
    public DbSet<FieldCollection> FieldCollections => Set<FieldCollection>();
    public DbSet<HelpDeskTicket> HelpDeskTickets => Set<HelpDeskTicket>();
    public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
    public DbSet<TicketAssignment> TicketAssignments => Set<TicketAssignment>();
    public DbSet<TicketQueue> TicketQueues => Set<TicketQueue>();
    public DbSet<SlaPolicy> SlaPolicies => Set<SlaPolicy>();
    public DbSet<StoreTrafficReading> StoreTrafficReadings => Set<StoreTrafficReading>();
    public DbSet<HeatmapReading> HeatmapReadings => Set<HeatmapReading>();
    public DbSet<ConversionMetric> ConversionMetrics => Set<ConversionMetric>();
    public DbSet<ProductPerformance> ProductPerformances => Set<ProductPerformance>();
    public DbSet<CustomerBehavior> CustomerBehaviors => Set<CustomerBehavior>();
    public DbSet<CycleCount> CycleCounts => Set<CycleCount>();
    public DbSet<CycleCountItem> CycleCountItems => Set<CycleCountItem>();
    public DbSet<InventoryAudit> InventoryAudits => Set<InventoryAudit>();
    public DbSet<ShrinkageCase> ShrinkageCases => Set<ShrinkageCase>();
    public DbSet<SuspiciousTransactionAlert> SuspiciousTransactionAlerts => Set<SuspiciousTransactionAlert>();
    public DbSet<MetricSnapshot> MetricSnapshots => Set<MetricSnapshot>();
    public DbSet<KpiDefinition> KpiDefinitions => Set<KpiDefinition>();
    public DbSet<ReportDefinition> ReportDefinitions => Set<ReportDefinition>();
    public DbSet<Dashboard> Dashboards => Set<Dashboard>();
    public DbSet<DashboardWidget> DashboardWidgets => Set<DashboardWidget>();
    public DbSet<DataExportJob> DataExportJobs => Set<DataExportJob>();

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    public DbSet<PosSale> PosSales => Set<PosSale>();
    public DbSet<PosSaleItem> PosSaleItems => Set<PosSaleItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            Expression? filterBody = null;

            if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var property = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
                var tenantId = _currentUser?.TenantId;
                filterBody = tenantId.HasValue
                    ? (Expression)Expression.Equal(property, Expression.Constant(tenantId.Value, typeof(Guid?)))
                    : Expression.Constant(true);
            }

            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var softDeleteBody = Expression.Equal(property, Expression.Constant(false));
                filterBody = filterBody is null ? softDeleteBody : Expression.AndAlso(filterBody, softDeleteBody);
            }

            if (filterBody is not null)
            {
                var lambda = Expression.Lambda(filterBody, parameter);
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
