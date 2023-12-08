namespace WebMonAn.Model
{
    public class UpdatePassword
    {
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        public int UserId { get; set; }
    }
}
