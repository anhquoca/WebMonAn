using ApiHoaDon.Helper;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;

namespace WebMonAn.Service
{
    public class UserServices : IUserServices
    {
        public readonly AppDbContext DbContext;
        public UserServices()
        {
            DbContext = new AppDbContext();
        }

        public ThongKeDoanhThuDTO ThongKeDoanhThu(int? thang, int? quy, int? nam)
        {
            Double tongDoanhThu = 0;
            int sohoadon = 0;
            var listOrder = DbContext.Order.AsQueryable();
            
            if (nam.HasValue)
            {
                listOrder = listOrder.Where(x => x.Created_at.Value.Year == nam);

            }
            if (quy.HasValue)
            {
                listOrder = listOrder.Where(x => ((x.Created_at.Value.Month - 1) / 3 + 1) == quy);         
            }
            if (thang.HasValue)
            {
                listOrder = listOrder.Where(x => x.Created_at.Value.Month == thang);
            }
            if(nam.HasValue || quy.HasValue || thang.HasValue) {
                tongDoanhThu = listOrder.Sum(x => x.Original_price);
                sohoadon = listOrder.Count();
            }
            return new ThongKeDoanhThuDTO { TongDoanhThu= tongDoanhThu,SoHoaDon = sohoadon};
        }

        public ErorType UpdatePassword(UpdatePassword model)
        {
            try
            {
                if (model.PasswordNew != null && model.UserId != null)
                {
                    var account = DbContext.Account.FirstOrDefault(x => x.UserId == model.UserId);
                    if (account != null)
                    {
                        if (account.Password != model.PasswordOld)
                        {
                            return ErorType.MatKhauCuKhongChinhXac;
                        }
                        if (account.Password != model.PasswordNew)
                        {
                            account.Password = model.PasswordNew;
                            DbContext.Update(account);
                            DbContext.SaveChanges();
                            return ErorType.ThanhCong;
                        }
                        else
                        {
                            return ErorType.TrungVoiMatKhauCu;
                        }
                    }
                    return ErorType.ThatBai;

                }
                else
                {
                    return ErorType.ThatBai;
                }
            }
            catch
            {
                return ErorType.ThatBai;
            }
        }

        public ErorType UpdateProfile(ProfileModel model)
        {
            var user = DbContext.User.FirstOrDefault(x => x.Email == model.Email);
            if (user != null)
            {
                user.Address = model.Address ;
                user.Phone = model.Phone;
                user.User_name = model.User_name;
                user.ImgUser = model.ImgUser;
                user.Update_at = DateTime.Now;
                DbContext.Update(user);
                DbContext.SaveChanges();
                return ErorType.ThanhCong;
            }
            else
            {
                return ErorType.UserKhongTonTai;
            }
        }
    }
}
