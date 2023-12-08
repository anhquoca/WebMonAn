using ApiHoaDon.Helper;
using WebMonAn.DTOs;
using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface IAdminServices
    {
        List<OrderDTO> GetOrders();
        List<Order_deltailDTO> GetOrder_deltail(int orderId);
        List<ProductTypeDTO> GetProductTypes();
        ErorType ThemProduct(ProductModel product);
        ErorType ThemProductType(ProductTypeModel productType);
        ErorType UpdateProductType(int productTypeId, ProductTypeModel productType);
        ErorType DeleteProductType(int productTypeId);
    }
}
