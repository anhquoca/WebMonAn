namespace WebMonAn.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string? User_name { get; set; }
        public string? Avatar { get; set; }
        public string Password { get; set; }
        public int? Status { get; set; }
        public string? Verify {  get; set; }    
        public int DecentralizationId { get; set;}
        public Decentralization? Decentralization { get; set; }
        public string? Token {  get; set; }
        public string? ResetPasswordToken {  get; set; } 
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
