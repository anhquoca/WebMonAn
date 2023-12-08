namespace WebMonAn.Model
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpiry {  get; set; }
    }
}
