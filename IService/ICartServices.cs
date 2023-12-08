using ApiHoaDon.Helper;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface ICartServices
    {
        ErorType ThemSanPhamVaoGioHang(AddToCartModel model);
        IEnumerable<CartItemDTO> GetCartItems(int userId);
    }
}
