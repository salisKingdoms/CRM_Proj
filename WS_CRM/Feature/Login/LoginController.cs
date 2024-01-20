using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WS_CRM.Feature.Login.Model;
using WS_CRM.Feature.Login.dto;
using WS_CRM.Helper;
using WS_CRM.Config;

namespace WS_CRM.Feature.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppConfig _appConfig;
        public LoginController(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        [HttpGet]
        [Route("API/GenerateToken")]
        public IActionResult GenerateToken(string user_name, string password)
        {
            var tokenData = new JWTTokenData()
            {
                userName = user_name,
                password = password,
                exp = 360,
                date = DateTime.Now.AddHours(1).ToString()
            };
            var token = HelperJwt.generateJwtToken(_appConfig.JwtSecret, tokenData, DateTime.Now.AddHours(1));
            return Ok(token);
        }
    }
}
