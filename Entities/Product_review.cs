namespace WebMonAn.Entities
{
    public class Product_review
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }    
        public Product? Product { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Content_rated { get; set; }
        public int? Point_evaluation {  get; set; }
        public string?  Content_seen {  get; set; }
        public int? Status { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
