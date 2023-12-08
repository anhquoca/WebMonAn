namespace WebMonAn.Entities
{
    public class Cart_item
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        public int Quantity { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
