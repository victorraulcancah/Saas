using Backend.Application.ERP.Models;
using Backend.Application.ERP.Services;
using Backend.Domain.ERP.Entities;
using Backend.Infrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class ErpCatalogService : IErpCatalogService
{
    private readonly AppDbContext _db;

    public ErpCatalogService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync() =>
        (await _db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync()).Select(MapCategory);

    public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request)
    {
        var category = new Category { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description ?? string.Empty, ParentId = request.ParentId };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return MapCategory(category);
    }

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync() =>
        (await _db.Products.AsNoTracking().Include(p => p.Category).OrderBy(p => p.Name).ToListAsync()).Select(MapProduct);

    public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description ?? string.Empty,
            Sku = request.Sku,
            Barcode = request.Barcode ?? string.Empty,
            UnitPrice = request.UnitPrice,
            CostPrice = request.CostPrice,
            CategoryId = request.CategoryId,
            Stock = request.Stock,
            MinStock = request.MinStock,
            MaxStock = request.MaxStock,
            IsActive = true
        };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return MapProduct(product);
    }

    public async Task<ProductResponse?> UpdateProductAsync(Guid id, ProductRequest request)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return null;

        product.Name = request.Name;
        product.Description = request.Description ?? string.Empty;
        product.Sku = request.Sku;
        product.Barcode = request.Barcode ?? string.Empty;
        product.UnitPrice = request.UnitPrice;
        product.CostPrice = request.CostPrice;
        product.CategoryId = request.CategoryId;
        product.Stock = request.Stock;
        product.MinStock = request.MinStock;
        product.MaxStock = request.MaxStock;
        product.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return MapProduct(product);
    }

    public async Task<IEnumerable<SupplierResponse>> GetSuppliersAsync() =>
        (await _db.Suppliers.AsNoTracking().OrderBy(s => s.Name).ToListAsync()).Select(MapSupplier);

    public async Task<SupplierResponse> CreateSupplierAsync(SupplierRequest request)
    {
        var supplier = new Supplier { Id = Guid.NewGuid(), Name = request.Name, TaxId = request.TaxId, Email = request.Email ?? string.Empty, Phone = request.Phone ?? string.Empty, Address = request.Address ?? string.Empty, IsActive = true };
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();
        return MapSupplier(supplier);
    }

    public async Task<IEnumerable<WarehouseResponse>> GetWarehousesAsync() =>
        (await _db.Warehouses.AsNoTracking().OrderBy(w => w.Name).ToListAsync()).Select(MapWarehouse);

    public async Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request)
    {
        var warehouse = new Warehouse { Id = Guid.NewGuid(), Name = request.Name, Code = request.Code, Address = request.Address ?? string.Empty, City = request.City ?? string.Empty, IsActive = true };
        _db.Warehouses.Add(warehouse);
        await _db.SaveChangesAsync();
        return MapWarehouse(warehouse);
    }

    public async Task<IEnumerable<WarehouseLocationResponse>> GetWarehouseLocationsAsync(Guid warehouseId) =>
        (await _db.WarehouseLocations.AsNoTracking().Where(l => l.WarehouseId == warehouseId).OrderBy(l => l.Code).ToListAsync()).Select(MapLocation);

    public async Task<WarehouseLocationResponse> CreateWarehouseLocationAsync(WarehouseLocationRequest request)
    {
        var location = new WarehouseLocation { Id = Guid.NewGuid(), WarehouseId = request.WarehouseId, Code = request.Code, Aisle = request.Aisle ?? string.Empty, Rack = request.Rack ?? string.Empty, Level = request.Level ?? string.Empty };
        _db.WarehouseLocations.Add(location);
        await _db.SaveChangesAsync();
        return MapLocation(location);
    }

    public async Task<IEnumerable<ProductStockResponse>> GetProductStocksAsync(Guid? productId, Guid? warehouseId)
    {
        var query = _db.ProductStocks.AsNoTracking().Include(s => s.Product).Include(s => s.Warehouse).Include(s => s.WarehouseLocation).AsQueryable();
        if (productId.HasValue) query = query.Where(s => s.ProductId == productId.Value);
        if (warehouseId.HasValue) query = query.Where(s => s.WarehouseId == warehouseId.Value);
        var stocks = await query.OrderBy(s => s.Product.Name).ThenBy(s => s.Warehouse.Name).ToListAsync();
        return stocks.Select(s => new ProductStockResponse(s.Id, s.ProductId, s.Product.Name, s.WarehouseId, s.Warehouse.Name, s.WarehouseLocationId, s.WarehouseLocation?.Code, s.LotNumber, s.ExpirationDate, s.Quantity, s.Condition));
    }

    private static CategoryResponse MapCategory(Category c) => new(c.Id, c.Name, c.Description, c.ParentId);
    private static ProductResponse MapProduct(Product p) => new(p.Id, p.Name, p.Description, p.Sku, p.Barcode, p.UnitPrice, p.CostPrice, p.CategoryId, p.Category?.Name, p.Stock, p.MinStock, p.MaxStock, p.IsActive);
    private static SupplierResponse MapSupplier(Supplier s) => new(s.Id, s.Name, s.TaxId, s.Email, s.Phone, s.Address, s.IsActive);
    private static WarehouseResponse MapWarehouse(Warehouse w) => new(w.Id, w.Name, w.Code, w.Address, w.City, w.IsActive);
    private static WarehouseLocationResponse MapLocation(WarehouseLocation l) => new(l.Id, l.WarehouseId, l.Code, l.Aisle, l.Rack, l.Level);
}
