namespace WebMonAn.Entities
{
    public class Decentralization
    {
        public int Id { get; set; }
        public string Authority_name { get; set; } = string.Empty;
        public IEnumerable<Account>? Accounts { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Update_at { get; set; }
    }
}
