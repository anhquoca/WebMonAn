using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;
using WebMonAn.Service;

namespace WebMonAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public readonly ICartServices CartSV;
        public readonly AppDbContext DbContext;
        public CartController()
        {
            DbContext = new AppDbContext();
            CartSV = new CartServices();
        }
        [HttpGet("GetCartItems/{userId}")]
        public IActionResult GetListCartItem(int userId) { 
            var res= CartSV.GetCartItems(userId);
            if(res != null)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest("ERORR  ");
            }
        }
        [HttpPost("ThemSanPhamVaoCart")]
        public IActionResult PostCartItem(AddToCartModel model)
        {
            var res = CartSV.ThemSanPhamVaoGioHang(model);
            return Ok(res); 

        }
        [HttpDelete("XoaSanPhamKhoiGioHang/{cartItemId}")]
        public IActionResult DeleteCartItem(int cartItemId)
        {
            var carItem = DbContext.Cart_item.FirstOrDefault(x => x.Id == cartItemId);
            if(carItem != null)
            {
                DbContext.Cart_item.Remove(carItem);
                DbContext.SaveChanges();
                return Ok("Xoa thanh cong!");
            }
            else
            {
                return Ok("Cartitem khong ton tai!");
            }

        }
        [HttpDelete("XoaGioHang/{userId}")]
        public IActionResult DeleteCart(int userId)
        {
            var  cart = DbContext.Cart.FirstOrDefault(x=>x.UserId == userId);
            if(cart != null)
            {
                var listCatitem = DbContext.Cart_item.Where(x => x.CartId == cart.Id);
                DbContext.Cart_item.RemoveRange(listCatitem);
                DbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
           
        }

    }
}
