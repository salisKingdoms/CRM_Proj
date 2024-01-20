using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WS_CRM.Feature.Login.Model
{
    public class LoginRequest
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
