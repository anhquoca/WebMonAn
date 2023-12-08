using ApiHoaDon.Helper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;


namespace WebMonAn.Service
{
    public class OrderServices : IOrderServices
    {
        public readonly AppDbContext DbContext;
        public readonly ICartServices CartServices;

        public IModelMetadataProvider ViewData { get; private set; }

        public OrderServices()
        {
            DbContext = new AppDbContext();
            CartServices = new CartServices();
        }
        public ErorType ThanhToan(OrderModel model)
        {
            List < CartItemDTO > listCartItem= CartServices.GetCartItems(model.UserId).ToList();
            var newOrder = new Order
            {
                PaymentId = model.PaymentId,
                UserId = model.UserId,
                Original_price = model.Original_price,
                Actual_price = model.Actual_price,
                Full_Name = model.Full_Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,
                Order_statusId=1,
                Created_at = DateTime.Now,
            };
            
            DbContext.Order.Add(newOrder);
            DbContext.SaveChanges();

            var newOrderId = newOrder.Id;
            CreaterOrderDetail(listCartItem, newOrderId);
            // removeLítCartItem
            RefresCart(listCartItem);
            //gui email
            SendEmailThongTinOrderToUser(newOrderId, model.UserId);

            return ErorType.ThanhCong;

        }
        public string ThanhToanOnline(OrderModel model)
        {
            List<CartItemDTO> listCartItem = CartServices.GetCartItems(model.UserId).ToList();
            var newOrder = new Order
            {
                PaymentId = model.PaymentId,
                UserId = model.UserId,
                Original_price = model.Original_price,
                Actual_price = model.Actual_price,
                Full_Name = model.Full_Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,
                Order_statusId = 1,
                Created_at = DateTime.Now,
            };

            DbContext.Order.Add(newOrder);
            DbContext.SaveChanges();

            var newOrderId = newOrder.Id;
            CreaterOrderDetail(listCartItem, newOrderId);
            // removeLítCartItem
            // chua reset cart
            //RefresCart(listCartItem);
            //gui email
            //SendEmailThongTinOrderToUser(newOrderId, model.UserId);
            return newOrderId.ToString();
        }
        public void XuLyDonDatHangThanhToanOnline(int orderId)
        {
            var order = DbContext.Order.FirstOrDefault(x => x.Id == orderId);
            if(order != null)
            {
                List<CartItemDTO> listCartItem = CartServices.GetCartItems(order.UserId).ToList();
                order.Order_statusId = 3;
                DbContext.SaveChanges();
                RefresCart(listCartItem);
                SendEmailThongTinOrderToUser(order.Id, order.UserId);
            }

        }
        private void SendEmailThongTinOrderToUser(int newOrderId, int user_id)
        {
            var user = DbContext.User.FirstOrDefault(x => x.Id == user_id);
            var listOrderDeltail = DbContext.Order_Detail.Where(x => x.OrderId == newOrderId).Include(x => x.Product).Select(x => new Order_deltailDTO
            {
                Product = new ProductDTO
                {
                    Name_product = x.Product.Name_product,
                    Price = x.Product.Price,
                    Discount = x.Product.Discount
                },
                Quantity = x.Quantity
            }).ToList();

            var sendEmail = new MimeMessage();
            sendEmail.From.Add(MailboxAddress.Parse("nguyenanhquoc3042612@gmail.com"));
            sendEmail.To.Add(MailboxAddress.Parse(user.Email));
            sendEmail.Subject = "Thông tin đơn hàng";
            string body = GenerateHtmlTableOrder(newOrderId, listOrderDeltail);
            sendEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("nguyenanhquoc3042612@gmail.com", "jrodmstwjyahwgqv");
            smtp.Send(sendEmail);
            smtp.Disconnect(true);

        }

        private string GenerateHtmlTableOrder(int orderID, List<Order_deltailDTO> listItems)
        {
            var order = DbContext.Order.Include(x => x.Payment).Include(x => x.Order_status).FirstOrDefault(x => x.Id == orderID);
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.AppendLine($"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n" +
                                $"   <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    " +
                                $"<style>\r\n        .table {{\r\n            border-collapse: collapse;\r\n            width: 100%;\r\n        " +
                                $"}}\r\n        .table2 {{\r\n            border-collapse: collapse;\r\n            " +
                                $"width: 50%;\r\n        }}\r\n\r\n        th, td {{\r\n            border: 1px solid black;\r\n          " +
                                $"  padding: 8px;\r\n            text-align: left;\r\n        }}\r\n\r\n        th {{\r\n           " +
                                $" background-color: #f2f2f2;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n\r\n    " +
                                $"<h2>Thông Tin Đơn Hàng</h2>\r\n\r\n    <table class=\"table\">\r\n        <thead>\r\n            <tr>\r\n    " +
                                $"            <th>Mã Đơn Hàng</th>\r\n                <th>Phương Thức Thanh Toán</th>\r\n                <th>Trạng Thái Đơn Hàng</th>\r\n                <th>Giá Gốc</th>\r\n                <th>Giá Thực Tế</th>\r\n                <th>Ngày Tạo</th>\r\n                <th>Tên Người Đặt</th>\r\n                <th>Số Điện Thoại</th>\r\n                <th>Địa Chỉ</th>\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n            " +
                                $"<tr>\r\n                <td>{order.Id}</td>\r\n     <td>{order.Payment.Payment_method}</td>\r\n    <td>{order.Order_status.Status_name}</td>\r\n   <td>{order.Original_price}</td>\r\n     <td>{order.Actual_price}</td>\r\n                <td>{order.Created_at}</td>\r\n                <td>{order.Full_Name}</td>\r\n                <td>{order.Phone}</td>\r\n                <td>{order.Address}</td>\r\n            </tr>\r\n        </tbody>\r\n   " +
                                $" </table>\r\n\r\n    <br>\r\n\r\n    <h2>Danh Sách Sản Phẩm</h2>\r\n\r\n    <table class=\"table2\">\r\n        <thead>\r\n            <tr>\r\n                <th>Tên Sản Phẩm</th>\r\n                <th>Số Lượng</th>\r\n                <th>Giá</th>\r\n " +
                                $"           </tr>\r\n        </thead>\r\n        <tbody>\r\n");
            double tongtien = 0;
            foreach (var item in listItems)
            {
              
                htmlTable.AppendLine($"            <tr>\r\n" +
                                    $"                <td>{item.Product.Name_product}</td>\r\n" +
                                    $"                <td>{item.Quantity}</td>\r\n" +
                                    $"                <td>{item.Product.Price}</td>\r\n" +
                                    $"            </tr>\r\n");
                double totalprice = item.Quantity * item.Product.Price * (100 - (double)item.Product.Discount) / 100;
                tongtien += totalprice;
            }

            htmlTable.AppendLine($"        </tbody>\r\n" +
                                $"        <tfoot>\r\n" +
                                $"            <tr>\r\n" +
                                $"                <td colspan=\"2\">Tổng Tiền</td>\r\n" +
                                $"                <td>{tongtien}</td>\r\n" +
                                $"            </tr>\r\n" +
                                $"        </tfoot>\r\n" +
                                $"    </table>\r\n\r\n</body>\r\n</html>\r\n");

            return htmlTable.ToString();
        }
        private void RefresCart(List<CartItemDTO> listCartItem)
        {
            foreach (var item in listCartItem)
            {
                var cartItem = DbContext.Cart_item.FirstOrDefault(x=>x.Id==item.Id);
                if (cartItem != null)
                {
                    DbContext.Cart_item.Remove(cartItem);
                }
                
            }
            DbContext.SaveChanges();
        }

        private void CreaterOrderDetail(List<CartItemDTO> listCartItem,int orderId)
        {
            List<Order_detail> listResult = new List<Order_detail>();
            foreach (var item in listCartItem)
            {
                Order_detail newOrderDetail = new Order_detail
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Price_total = item.Product.Price * item.Quantity,
                    Quantity = item.Quantity,
                    Created_at = DateTime.UtcNow
                };
                listResult.Add(newOrderDetail); 
            }
            DbContext.Order_Detail.AddRange(listResult);
            DbContext.SaveChanges();
        }

        public List<Order_deltailDTO> GetOrder_deltail(int userId)
        {
            var listoder = DbContext.Order.Where(x => x.UserId == userId);
            if (listoder.Count() != 0)
            {
                var listresult = new List<Order_deltailDTO>();
                foreach (var item in listoder)
                {
                    var listOderdeltail = DbContext.Order_Detail.Where(x => x.OrderId == item.Id).Include(x => x.Product).Include(x=>x.Order).ThenInclude(or=>or.Order_status).
                        Select(x => new Order_deltailDTO{
                            ProductId = x.ProductId,
                            Price_total = x.Price_total,
                            Quantity = x.Quantity,
                            Product = new ProductDTO
                            {
                                Avartar_image_product = "",
                                Name_product = x.Product.Name_product,
                                Price = x.Product.Price,
                                Discount = x.Product.Discount
                            },
                            OrderStatus = x.Order.Order_status.Status_name
                        }
                        );
                    listresult.AddRange(listOderdeltail);
                }
                return listresult;
            }
            else
            {
                return new List<Order_deltailDTO>();
            }   
        }

        public ErorType XuLyDonDatHang(OrderProcessingModel model)
        {
            var Order = DbContext.Order.FirstOrDefault(x=>x.Id == model.OrderID);
            if (Order != null) {
                Order.Order_statusId = model.OrderStatusId;
                DbContext.Update(Order);
                DbContext.SaveChanges();
                if( model.OrderStatusId ==3 || model.OrderStatusId == 4)
                {
                    SendProcessingOrderEmail(Order.UserId, model.OrderStatusId);
                }
                return ErorType.ThanhCong;
            }
            else
            {
                return ErorType.ThatBai;
            }
        }
        public List<OrderDTO> GetOrders(int userId)
        {
            var listOder = DbContext.Order.Where(x => x.UserId == userId).Include(x=>x.Payment).Include(x=>x.Order_status);
            if (listOder.Count() > 0)
            {
                var listResult = listOder.Select(x => new OrderDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Original_price = x.Original_price,
                    Actual_price = x.Actual_price,
                    Full_Name=x.Full_Name,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    Payment_method = x.Payment.Payment_method,
                    Status_name = x.Order_status.Status_name
                    
                }).ToList();
                return listResult;
            }
            else
            {
                return new List<OrderDTO>();
            }
        }
        public void SendProcessingOrderEmail(int userId,int orderStatusId )
        {
            var user = DbContext.User.FirstOrDefault(x=>x.Id == userId);

            var sendEmail = new MimeMessage();
            sendEmail.From.Add(MailboxAddress.Parse("nguyenanhquoc3042612@gmail.com"));
            sendEmail.To.Add(MailboxAddress.Parse(user.Email));
            sendEmail.Subject = "Thong báo đơn hàng";
            string body="";
            if(orderStatusId == 3)
            {
                 body = $"Đơn hàng đã thanh toán thanh công ";

            }
            else
            {
                body = $"Đơn hàng đã xảy ra lỗi trong quá trình xử lý, đơn hàng của bạn sẽ được hủy!";
            }
           
            sendEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("nguyenanhquoc3042612@gmail.com", "jrodmstwjyahwgqv");
            smtp.Send(sendEmail);
            smtp.Disconnect(true);
        }

      
    }
}
