using WebMonAn.Entities;

namespace WebMonAn.Model
{
    public class ProductReviewModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Content_rated { get; set; }
        public int Point_evaluation { get; set; }
    }
}
