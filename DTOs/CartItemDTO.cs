using WebMonAn.Entities;

namespace WebMonAn.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Quantity { get; set; }

    }
}
