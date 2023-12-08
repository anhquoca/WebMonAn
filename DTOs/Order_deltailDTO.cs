using WebMonAn.Entities;

namespace WebMonAn.DTOs
{
    public class Order_deltailDTO
    {
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public Double Price_total { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get; set; }
    }
}
