namespace WS_CRM.Helper
{
    public class JWTTokenData
    {
        public string userName { get; set; }
        public string password { get; set; }
        public long? exp { get; set; }
        public long? iat { get; set; }
        public string date { get; set; }
    }
}
