using ApiHoaDon.Helper;
using Microsoft.EntityFrameworkCore;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;

namespace WebMonAn.Service
{
    public class ProductServices : IProductServices
    {
        public readonly AppDbContext DbContext;
        public ProductServices()
        {
            DbContext = new AppDbContext();
        }

        public List<ProductDTO> GetListProducts()
        {
            var product = DbContext.Product.Include(x=>x.Product_type).AsEnumerable();
            var listproduct = DbContext.Product.Select(x => new ProductDTO
            {
                Id = x.Id,
                Product_typeId = x.Product_typeId,
                Product_typeName = x.Product_type.Name_product_type,
                Name_product = x.Name_product,
                Price = x.Price,
                Discount = x.Discount,
                Avartar_image_product = x.Avartar_image_product
            }).ToList();
            if (listproduct.Any())
            {
                return listproduct;
            }
            else
            {
                return new List<ProductDTO>();
            }
        }

        public List<ProductDTO> GetListProductTheoType(int productId)
        {
            var product = DbContext.Product.FirstOrDefault(x=>x.Id == productId);
            var listproduct = DbContext.Product.Select(x => new ProductDTO
            {
                Id = x.Id,
                Product_typeId = x.Product_typeId,
                Name_product = x.Name_product,
                Price = x.Price,
                Discount = x.Discount,
                Avartar_image_product = x.Avartar_image_product
            }).Where(x=>x.Id != productId && x.Product_typeId == product.Product_typeId).ToList();
            if(listproduct.Any() )
            {
                return listproduct;
            }
            else
            {
                return new List<ProductDTO>();
            }
        }

        public ProductDetailDTO GetProduct(int productID)
        {
            var productReviews = DbContext.Product_review.Where(x => x.ProductId == productID).ToList();
            var productSolds = DbContext.Order_Detail.Include(x=>x.Order).Where(x=>x.ProductId == productID).ToList();
            var product = DbContext.Product.Include(x=>x.Product_type).Select(x => new ProductDetailDTO
            {
                Id = x.Id,
                Product_typeId = x.Product_typeId,
                Product_type = new ProductTypeDTO { Name_product_type = x.Product_type.Name_product_type},
                Name_product=x.Name_product,
                Price = x.Price,
                Discount = x.Discount,
                Avartar_image_product = x.Avartar_image_product,
                Title = x.Title,
                Description=x.Description ,
                Number_of_views = x.Number_of_views ?? 0,
                Point_evaluation = TinhTrungBinhPointEvaluation(productReviews),
                Number_of_solds= TinhSoSanPhamDaBan(productSolds) ?? 0
            }).FirstOrDefault(x => x.Id == productID);
            if(product != null)
            {
                TangSoLuotViews(product.Id);
                return product;

            }
            else
            {
                return new ProductDetailDTO();
            }

        }
        public List<ProductDTO> GetProductListBanChay()
        {
            var listProduct = DbContext.Product.AsEnumerable();
            foreach (var item in listProduct)
            {
                var productSolds = DbContext.Order_Detail.Include(x => x.Order).Where(x => x.ProductId == item.Id).ToList();
                var Number_of_solds = TinhSoSanPhamDaBan(productSolds);
                item.Number_of_solds = Number_of_solds;
                DbContext.Product.Update(item);
                DbContext.SaveChanges();
            }
            var listResult = DbContext.Product.AsEnumerable().OrderByDescending(x=>x.Number_of_solds).Take(8).Select(x => new ProductDTO
            {
                Id = x.Id,
                Product_typeId = x.Product_typeId,
                Name_product = x.Name_product,
                Avartar_image_product = x.Avartar_image_product,
                Price = x.Price,
                Discount = x.Discount

            }).ToList();
            return listResult;
        }

        private int? TinhSoSanPhamDaBan(List<Order_detail> productSolds)
        {
            if (productSolds.Any())
            {
                var tongsoluotmua = productSolds.Where(x => x.Order.Order_statusId == 3).Sum(x => x.Quantity);
                return tongsoluotmua;
            }
            else
            {
                return 0;
            }
        }

        private void TangSoLuotViews(int productId)
        {
            var product = DbContext.Product.FirstOrDefault(x=>x.Id== productId);
            product.Number_of_views += 1;
            DbContext.Product.Update(product);
            DbContext.SaveChanges();
        }

        private int? TinhTrungBinhPointEvaluation(List<Product_review> productReviews)
        {
            if (productReviews.Any())
            {
                var diemTrungBinhRating = Math.Round(productReviews.Average(x => x.Point_evaluation ?? 0));
                return (int)diemTrungBinhRating;
            }
            else
            {
                return 0;
            }
        }

        
    }
}
