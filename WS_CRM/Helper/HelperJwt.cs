using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WS_CRM.Helper
{
    public class HelperJwt
    {
        public static string generateJwtToken(string secret, object claimdata, int expiredMinute)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(secret);
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

        public static string generateJwtToken(string secret, object claimdata, DateTime expiredAt)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(secret);
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
                NotBefore = DateTime.UtcNow.AddDays(-1),
                Expires = expiredAt,
                Claims = claims,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytes), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
            };
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public static T validateToken<T>(string secret, string token) where T : class, new()
        {
            if (HelperStr.isEmptyOrNull(token))
            {
                return null;
            }

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.ASCII.GetBytes(secret);
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

        public static T decodeContent<T>(string token) where T : class, new()
        {
            //new T();
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

        public static bool VerifySignature(string token, string secret)
        {
            string[] parts = token.Split(".".ToCharArray());
            var header = parts[0];
            var payload = parts[1];
            var signature = parts[2];

            byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Join(".", header, payload));
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret);

            var alg = new HMACSHA256(secretBytes);
            var hash = alg.ComputeHash(bytesToSign);
            var computedSignature = Base64UrlEncode(hash);

            var isValid = signature == computedSignature;
            return isValid;
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0];
            output = output.Replace('+', '-');
            output = output.Replace('/', '_');
            return output;
        }
    }
}
