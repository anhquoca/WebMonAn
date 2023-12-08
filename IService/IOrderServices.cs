using ApiHoaDon.Helper;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface IOrderServices
    {
        ErorType ThanhToan(OrderModel orderModel);
        string ThanhToanOnline(OrderModel orderModel);
        List<Order_deltailDTO> GetOrder_deltail(int userId);
        ErorType XuLyDonDatHang(OrderProcessingModel model);
        void XuLyDonDatHangThanhToanOnline(int  orderId);
        List<OrderDTO> GetOrders(int userId);

    }
}
