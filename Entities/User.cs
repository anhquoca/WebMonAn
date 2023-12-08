namespace WebMonAn.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string? User_name { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set;}
        public string? ImgUser { get; set; }    
        public string? VerifyEmailToken { get; set; }
        public string? Address { get; set;}
        public Account? Account { get; set;}
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Cart>? Carts { get; set;}
        public IEnumerable<Product_review>? Product_Reviews { get; set;}
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }

    }
}
