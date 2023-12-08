using WebMonAn.Entities;

namespace WebMonAn.Model
{
    public class AddToCartModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
