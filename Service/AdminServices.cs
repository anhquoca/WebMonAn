using ApiHoaDon.Helper;
using Microsoft.EntityFrameworkCore;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;

namespace WebMonAn.Service
{
    public class AdminServices : IAdminServices
    {
        public readonly AppDbContext DbContext;
        public AdminServices() {
            DbContext = new AppDbContext();
        }

        public List<OrderDTO> GetOrders()
        {
            var listOder = DbContext.Order.Include(x=>x.Payment).Include(x=>x.Order_status).AsQueryable();
            if (listOder.Count() > 0)
            {
                var listResult = listOder.Select(x => new OrderDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Original_price = x.Original_price,
                    Actual_price = x.Actual_price,
                    Full_Name = x.Full_Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    Payment_method = x.Payment.Payment_method,
                    Status_name = x.Order_status.Status_name,
                    Created_at = x.Created_at.Value.ToString("dd/MM/yyyy"),

                }).ToList();
                return listResult;
            }
            else
            {
                return new List<OrderDTO>();
            }
        }

        public ErorType ThemProduct(ProductModel product)
        {

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var newProduct = new Product
                    {
                        Product_typeId = product.Product_typeId,
                        Name_product = product.Name_product,
                        Price = product.Price,
                        Avartar_image_product = product.Avartar_image_product,
                        Title = product.Title,
                        Description = product.Description,
                        Discount = product.Discount,
                        Created_at = DateTime.Now
                    };
                    DbContext.Product.Add(newProduct);
                    DbContext.SaveChanges();
                    transaction.Commit();
                    return ErorType.ThanhCong;

                }
                catch
                {
                    transaction.Rollback();
                    return ErorType.ThatBai;
                }
            }
        }

        public ErorType ThemProductType(ProductTypeModel productType)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var newProductType = new Product_type
                    {
                        Name_product_type = productType.Name_product_type,
                        Created_at = DateTime.Now
                    };
                    DbContext.Product_type.Add(newProductType);
                    DbContext.SaveChanges();
                    transaction.Commit();
                    return ErorType.ThanhCong;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ErorType.ThatBai;
                }
            }

        }

        public ErorType UpdateProductType(int productTypeId, ProductTypeModel productType)
        {
            var product = DbContext.Product_type.FirstOrDefault(x => x.Id == productTypeId);
            if (product != null)
            {
                product.Name_product_type = productType.Name_product_type;
                product.Update_at = DateTime.Now;
                DbContext.Product_type.Update(product);
                DbContext.SaveChanges();
                return ErorType.ThanhCong;
            }
            else
            {
                return ErorType.ThatBai;
            }
        }
        public ErorType DeleteProductType(int productTypeId)
        {
            var product = DbContext.Product_type.FirstOrDefault(x => x.Id == productTypeId);
            if (product != null)
            {
                DbContext.Product_type.Remove(product);
                DbContext.SaveChanges();
                return ErorType.ThanhCong;
            }
            else
            {
                return ErorType.ThatBai;
            }
        }

        public List<Order_deltailDTO> GetOrder_deltail(int orderId)
        {
            var listOderdeltail = DbContext.Order_Detail.Where(x => x.OrderId == orderId).Include(x => x.Product).Include(x => x.Order).ThenInclude(or => or.Order_status).
                        Select(x => new Order_deltailDTO
                        {
                            ProductId = x.ProductId,
                            Price_total = x.Price_total,
                            Quantity = x.Quantity,
                            Product = new ProductDTO
                            {
                                Avartar_image_product = x.Product.Avartar_image_product,
                                Name_product = x.Product.Name_product,
                                Price = x.Product.Price,
                                Discount = x.Product.Discount
                            },
                            OrderStatus = x.Order.Order_status.Status_name
                        }
                        ).ToList();
            if(listOderdeltail != null)
            {
                return listOderdeltail;
            }
            else
            {
                return new List<Order_deltailDTO>();
            }
            
        }

        public List<ProductTypeDTO> GetProductTypes()
        {
            var listProductType= DbContext.Product_type.Select(x=> new ProductTypeDTO { 
                Id=x.Id,
                Name_product_type = x.Name_product_type,}
            ).ToList(); 
            if(listProductType != null)
            {
                return listProductType;
            }
            else
            {
                return new List<ProductTypeDTO>();
            }

        }
    }
}
