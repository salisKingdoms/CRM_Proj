using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using WS_CRM.Config;

namespace WS_CRM.Helper
{
    public interface IJwtFunction
    {
        public APIResult TokenVerification(HttpRequest request);
        public string generateJwtToken(object claimdata, int expiredMinute);
        public string generateJwtToken(object claimdata, DateTime expiredAt);
        public T validateToken<T>(HttpRequest request) where T : class, new();
    }

    public class JwtFunction : IJwtFunction
    {
        private readonly AppConfig _appConfig;
        public JwtFunction(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public string GetToken(HttpRequest request)
        {
            try
            {
                var headers = request.Headers;
                return headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault();
            }
            catch
            {
                return "";
            }
        }

        public APIResult TokenVerification(HttpRequest request)
        {
            var result = new APIResult();
            try
            {
                var token = GetToken(request);
                if (string.IsNullOrEmpty(token)) throw new Exception("Token Required");

                var signatureVerified = VerifySignature(token);
                if (!signatureVerified) throw new Exception("Token Invalid");

                var decodedToken = decodeContent<JWTTokenData>(token);
                var expiredToken = new DateTime(1970, 1, 1).AddHours(7).AddSeconds(decodedToken.exp.Value);
                var tokenIsValid = expiredToken >= DateTime.Now;
                if (!tokenIsValid) throw new Exception("Token Expired");

                result.is_ok = true;
                result.message = "Success";
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = ex.Message;
            }
            return result;
        }

        public string generateJwtToken(object claimdata, int expiredMinute)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(_appConfig.JwtSecret);
            Dictionary<string, object> claims = new Dictionary<string, object>();
            new List<PropertyInfo>(claimdata.GetType().GetTypeInfo().DeclaredProperties).ForEach(delegate (PropertyInfo e)
            {
                if (e.GetValue(claimdata) != null)
                {
                    Type type = e.PropertyType;
                    if (type.IsGenericType && type.Name.Contains("Nullable"))
                    {
                        type = type.GenericTypeArguments[0];
                    }

                    if (type == typeof(DateTime))
                    {
                        double totalSeconds = ((DateTime)e.GetValue(claimdata)).ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        claims.Add(e.Name, Convert.ToInt64(Math.Floor(totalSeconds)));
                    }
                    else
                    {
                        claims.Add(e.Name, e.GetValue(claimdata));
                    }
                }
            });
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(expiredMinute),
                NotBefore = DateTime.UtcNow.AddSeconds(-60.0),
                Claims = claims,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytes), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
            };
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public string generateJwtToken(object claimdata, DateTime expiredAt)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(_appConfig.JwtSecret);
            Dictionary<string, object> claims = new Dictionary<string, object>();
            new List<PropertyInfo>(claimdata.GetType().GetTypeInfo().DeclaredProperties).ForEach(delegate (PropertyInfo e)
            {
                if (e.GetValue(claimdata) != null)
                {
                    Type type = e.PropertyType;
                    if (type.IsGenericType && type.Name.Contains("Nullable"))
                    {
                        type = type.GenericTypeArguments[0];
                    }

                    if (type == typeof(DateTime))
                    {
                        double totalSeconds = ((DateTime)e.GetValue(claimdata)).ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        claims.Add(e.Name, Convert.ToInt64(Math.Floor(totalSeconds)));
                    }
                    else
                    {
                        claims.Add(e.Name, e.GetValue(claimdata));
                    }
                }
            });
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = DateTime.UtcNow.AddSeconds(-10.0),
                Expires = expiredAt,
                Claims = claims,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytes), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
            };
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public T validateToken<T>(HttpRequest request) where T : class, new()
        {
            var token = GetToken(request);
            if (HelperStr.isEmptyOrNull(token))
            {
                return null;
            }

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(_appConfig.JwtSecret);
            try
            {
                token = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
                jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(bytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);
                JwtSecurityToken obj = (JwtSecurityToken)validatedToken;
                T val = new T();
                List<PropertyInfo> source = new List<PropertyInfo>(typeof(T).GetTypeInfo().DeclaredProperties);
                foreach (Claim cl in obj.Claims)
                {
                    if (HelperStr.isEmptyOrNull(cl.Value))
                    {
                        continue;
                    }

                    PropertyInfo propertyInfo = source.FirstOrDefault((PropertyInfo e) => e.Name.Equals(cl.Type, StringComparison.OrdinalIgnoreCase));
                    if (!(propertyInfo != null))
                    {
                        continue;
                    }

                    Type propertyType = propertyInfo.PropertyType;
                    if (propertyType.IsGenericType && propertyType.Name.Contains("Nullable"))
                    {
                        propertyType = propertyType.GenericTypeArguments[0];
                        if (propertyType == typeof(string))
                        {
                            propertyInfo.SetValue(val, cl.Value);
                        }
                        else if (propertyType == typeof(int))
                        {
                            propertyInfo.SetValue(val, Convert.ToInt32(cl.Value));
                        }
                        else if (propertyType == typeof(long))
                        {
                            propertyInfo.SetValue(val, Convert.ToInt64(cl.Value));
                        }
                        else if (propertyType == typeof(bool))
                        {
                            propertyInfo.SetValue(val, Convert.ToBoolean(cl.Value));
                        }
                        else if (propertyType == typeof(double))
                        {
                            propertyInfo.SetValue(val, Convert.ToDouble(cl.Value));
                        }
                        else if (propertyType == typeof(DateTime))
                        {
                            propertyInfo.SetValue(val, dateTime.AddSeconds((double)Convert.ToInt64(cl.Value) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds));
                        }
                    }
                    else if (propertyType == typeof(string))
                    {
                        propertyInfo.SetValue(val, cl.Value);
                    }
                    else if (propertyType == typeof(int))
                    {
                        propertyInfo.SetValue(val, Convert.ToInt32(cl.Value));
                    }
                    else if (propertyType == typeof(long))
                    {
                        propertyInfo.SetValue(val, Convert.ToInt64(cl.Value));
                    }
                    else if (propertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(val, Convert.ToBoolean(cl.Value));
                    }
                    else if (propertyType == typeof(double))
                    {
                        propertyInfo.SetValue(val, Convert.ToDouble(cl.Value));
                    }
                    else if (propertyType == typeof(DateTime))
                    {
                        propertyInfo.SetValue(val, dateTime.AddSeconds((double)Convert.ToInt64(cl.Value) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds));
                    }
                }

                return val;
            }
            catch (SecurityTokenExpiredException)
            {
                throw;
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.StackTrace);
                return null;
            }
        }

        public T decodeContent<T>(string token) where T : class, new()
        {
            new T();
            token = token.Replace("Bearer", "").Trim();
            string text = new Regex("(\\w*).(\\w*).(\\w*)", RegexOptions.IgnoreCase).Match(token).Groups[2].Value.Replace('_', '/').Replace('-', '+');
            switch (text.Length % 4)
            {
                case 2:
                    text += "==";
                    break;
                case 3:
                    text += "=";
                    break;
            }

            byte[] bytes = Convert.FromBase64String(text);
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
        }

        public bool VerifySignature(string token)
        {
            string[] parts = token.Split(".".ToCharArray());
            var header = parts[0];
            var payload = parts[1];
            var signature = parts[2];

            byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Join(".", header, payload));
            byte[] secretBytes = Encoding.UTF8.GetBytes(_appConfig.JwtSecret);

            var alg = new HMACSHA256(secretBytes);
            var hash = alg.ComputeHash(bytesToSign);
            var computedSignature = Base64UrlEncode(hash);

            var isValid = signature == computedSignature;
            return isValid;
        }

        private string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0];
            output = output.Replace('+', '-');
            output = output.Replace('/', '_');
            return output;
        }
    }
}
