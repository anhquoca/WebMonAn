using ApiHoaDon.Helper;
using Microsoft.EntityFrameworkCore;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;

namespace WebMonAn.Service
{
    public class ProductReviewServices : IProductReviewServices
    {
        public readonly AppDbContext DbContext;
        public ProductReviewServices()
        {
            DbContext = new AppDbContext();
        }
        public List<ProductReviewDTO> GetProductReviews(int ProductId)
        {
            var listResult = DbContext.Product_review.Include(x=>x.User).Where(x=>x.ProductId == ProductId).
                Select(x=> new ProductReviewDTO
                {
                    User = new UserDTO
                    {
                        User_name = x.User.User_name,
                        ImgUser = x.User.ImgUser
                    },
                    Content_rated = x.Content_rated,
                    Point_evaluation = x.Point_evaluation
                }).ToList();
            if(listResult.Count > 0)
            {
                return listResult;
            }
            else
            {
                return new List<ProductReviewDTO>();
            }
        }

        public ErorType KiemTraQuyenDanhGiaSanPham(UserProductModel model)
        {
            var listOrderDetail = DbContext.Order_Detail.Include(x => x.Order).Where(x => x.ProductId == model.ProductId && x.Order.UserId == model.UserId && x.Order.Order_statusId ==3);
            if (listOrderDetail.Any())
            {
                return ErorType.ThanhCong;
            }
            else
            {
                return ErorType.ThatBai;
            }

        }

        public ErorType UserAddtProductReviews(ProductReviewModel model)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var user = DbContext.User.FirstOrDefault(x => x.Id == model.UserId);
                    var product = DbContext.Product.FirstOrDefault(x => x.Id == model.ProductId);
                    if (user != null && product != null)
                    {
                        var productReview = new Product_review
                        {
                            UserId = model.UserId,
                            ProductId = model.ProductId,
                            Content_rated = model.Content_rated,
                            Point_evaluation = model.Point_evaluation,
                            Created_at = DateTime.Now
                        };
                        DbContext.Add(productReview);
                        DbContext.SaveChanges();
                        transaction.Commit();
                        return ErorType.ThanhCong;
                    }
                    else
                    {
                        return ErorType.ThatBai;
                    }

                }
                catch
                {
                    transaction.Rollback();
                    return ErorType.ThatBai;
                }
            }



        }
    }
}
