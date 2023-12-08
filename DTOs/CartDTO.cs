using WebMonAn.Entities;

namespace WebMonAn.DTOs
{
    public class CartDTO
    {
        public IEnumerable<CartItemDTO>? Cart_Items { get; set; }

    }
}
