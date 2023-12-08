namespace WebMonAn.DTOs
{
    public class ProductReviewDTO
    {
        public int UserId { get; set; }
        public UserDTO? User { get; set; }
        public string? Content_rated { get; set; }
        public int? Point_evaluation { get; set; }

    }

}
