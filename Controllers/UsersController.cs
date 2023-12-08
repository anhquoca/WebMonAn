using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMonAn.Entities;
using WebMonAn.IService;
using WebMonAn.Model;
using WebMonAn.Service;

namespace WebMonAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IUserServices UserSV;
        public readonly AppDbContext DbContext;
        public UsersController()
        {
            DbContext = new AppDbContext();
            UserSV = new UserServices();
        }
        [HttpPost("updatePassword")]
        public IActionResult UpdatePass(UpdatePassword model)
        {
            var res = UserSV.UpdatePassword(model);
            return Ok(res);
        }
        [HttpPut("updateProfile")]
        public IActionResult UpdateProfile(ProfileModel model)
        {
            var res = UserSV.UpdateProfile(model);
            return Ok(res);
        }

        [HttpGet("thongtinUser/{userId}")]
        public IActionResult GetThongtin(int userId)
        {
            var user = DbContext.User.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return Ok("Khong tim thay nguoi dung");
            }
            var cart = DbContext.Cart.FirstOrDefault(x=>x.UserId == userId);
            int listcartitem = 0;
            if (cart != null)
            {
                listcartitem = DbContext.Cart_item.Where(x => x.CartId == cart.Id).Count();
            }
            
            
            return Ok(new
            {
                userImage = user.ImgUser,
                slCartItem = listcartitem,
                email = user.Email,
                userName= user.User_name,
                address = user.Address,
                phone = user.Phone
            });
        }
        


    }
}
