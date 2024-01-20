namespace WS_CRM.Feature.Login.dto
{
    public class UserLogin
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
