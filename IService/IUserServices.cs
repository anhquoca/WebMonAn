
using ApiHoaDon.Helper;
using WebMonAn.DTOs;
using WebMonAn.Entities;
using WebMonAn.Model;

namespace WebMonAn.IService
{
    public interface IUserServices
    {
        ErorType UpdateProfile(ProfileModel user);
        ErorType UpdatePassword(UpdatePassword model);
        ThongKeDoanhThuDTO ThongKeDoanhThu(int? thang, int? quy, int? nam);
    }
}
