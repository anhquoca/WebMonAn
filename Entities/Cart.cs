namespace WebMonAn.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public IEnumerable<Cart_item>? Cart_items { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
