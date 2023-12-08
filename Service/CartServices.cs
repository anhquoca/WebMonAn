using System.Linq;
using ApiHoaDon.Helper;
using Microsoft.EntityFrameworkCore;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;

namespace WebMonAn.Service
{
    public class CartServices : ICartServices
    {
        public readonly AppDbContext DbContext;
        public CartServices() {
            DbContext = new AppDbContext();
        }
        public ErorType ThemSanPhamVaoGioHang(AddToCartModel model)
        {
            try
            {
                CreateCart(model.UserId);
                var cart = DbContext.Cart.FirstOrDefault(x=>x.UserId == model.UserId);
                var cartItem = DbContext.Cart_item.FirstOrDefault(x=>x.CartId == cart.Id && x.ProductId==model.ProductId);
                if (cartItem != null)
                {
                    cartItem.Quantity = cartItem.Quantity + model.Quantity;
                    DbContext.Update(cartItem);
                    DbContext.SaveChanges();
                    return ErorType.ThanhCong;

                }
                else
                {
                    CreateCartItem(model.ProductId,cart.Id, model.Quantity);
                    return ErorType.ThanhCong;
                }

            }catch (Exception ex)
            {
                return ErorType.ThatBai;
            }
        }
        public IEnumerable<CartItemDTO> GetCartItems(int userId)
        {
            var cart = DbContext.Cart.FirstOrDefault(x => x.UserId == userId);
            if(cart!=null)
            {
                var listCartItem = DbContext.Cart_item.Where(x => x.CartId == cart.Id)
                .Include(x => x.Product)
                .Select(x => new CartItemDTO
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    CartId = x.CartId,
                    Quantity = x.Quantity,
                    Product = new ProductDTO
                    {
                        Avartar_image_product = x.Product.Avartar_image_product,
                        Name_product = x.Product.Name_product,
                        Price = x.Product.Price,
                        Discount = x.Product.Discount
                    }
                }).ToList();

                return listCartItem;
            }
            else
            {
                return new List<CartItemDTO>();
            }
        }

        private void CreateCartItem(int productId, int cartId, int soluong)
        {
            var cartItem = new Cart_item
            {
                ProductId = productId,
                CartId = cartId,
                Quantity = soluong
            };
            DbContext.Cart_item.Add(cartItem);
            DbContext.SaveChanges();
        }

        private void CreateCart(int userId)
        {
            var cart = DbContext.Cart.FirstOrDefault(x => x.UserId == userId);
            if (cart == null)
            {
                var newCart = new Cart
                {
                    UserId = userId,
                };
                DbContext.Cart.Add(newCart);
                DbContext.SaveChanges();
            }

        }

        
    }
}
