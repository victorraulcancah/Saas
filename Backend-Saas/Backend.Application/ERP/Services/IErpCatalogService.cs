using Backend.Application.ERP.Models;

namespace Backend.Application.ERP.Services;

public interface IErpCatalogService
{
    Task<IEnumerable<CategoryResponse>> GetCategoriesAsync();
    Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request);
    Task<IEnumerable<ProductResponse>> GetProductsAsync();
    Task<ProductResponse> CreateProductAsync(ProductRequest request);
    Task<ProductResponse?> UpdateProductAsync(Guid id, ProductRequest request);
    Task<IEnumerable<SupplierResponse>> GetSuppliersAsync();
    Task<SupplierResponse> CreateSupplierAsync(SupplierRequest request);
    Task<IEnumerable<WarehouseResponse>> GetWarehousesAsync();
    Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request);
    Task<IEnumerable<WarehouseLocationResponse>> GetWarehouseLocationsAsync(Guid warehouseId);
    Task<WarehouseLocationResponse> CreateWarehouseLocationAsync(WarehouseLocationRequest request);
    Task<IEnumerable<ProductStockResponse>> GetProductStocksAsync(Guid? productId, Guid? warehouseId);
}
