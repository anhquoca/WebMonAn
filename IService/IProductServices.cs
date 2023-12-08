using ApiHoaDon.Helper;
using WebMonAn.DTOs;
using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface IProductServices
    {
        ProductDetailDTO GetProduct(int productID);
        List<ProductDTO> GetProductListBanChay();
        List<ProductDTO> GetListProducts();
        List<ProductDTO> GetListProductTheoType(int productId);

    }
}
