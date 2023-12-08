using ApiHoaDon.Helper;
using WebMonAn.DTOs;
using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface IProductReviewServices
    {
        List<ProductReviewDTO> GetProductReviews(int ProductId);
        ErorType UserAddtProductReviews(ProductReviewModel model);
        ErorType KiemTraQuyenDanhGiaSanPham(UserProductModel model);
    }
}
