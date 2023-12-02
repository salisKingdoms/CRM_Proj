using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Security;
using System.Text;


namespace WS_CRM.Helper
{
    public class CallAPIHelper
    {
        public static T RunAPIServiceRequestGET<T>(T resultModel, string endPoint)
        {
            var strAPIResponse = GETResponse(endPoint);
            var result = JsonConvert.DeserializeObject<T>(strAPIResponse);
            return result;
        }

        public static T RunAPIServiceRequestGETWithToken<T>(T resultModel, string endPoint, string token)
        {
            var strAPIResponse = GETResponseWithToken(endPoint, token);
            var result = JsonConvert.DeserializeObject<T>(strAPIResponse);
            return result;
        }

        public static string GETResponse(string WEBSERVICE_URL)
        {
            string response = "";
            try
            {
                var webRequest = WebRequest.Create(WEBSERVICE_URL);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 180000;
                    webRequest.ContentType = "application/json";

                    using (StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        response = jsonResponse.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("API Err : " + ex.Message);
            }

            return response;
        }

        public static string GETResponseWithToken(string WEBSERVICE_URL, string token)
        {
            string response = "";
            try
            {
                var webRequest = WebRequest.Create(WEBSERVICE_URL);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 180000;
                    webRequest.ContentType = "application/json";

                    if (!string.IsNullOrEmpty(token))
                    {
                        webRequest.Headers.Add("token", token);
                    }

                    using (StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        response = jsonResponse.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("API Err : " + ex.Message);
            }

            return response;
        }

        public static T RunAPIServiceRequestGET<T>(T resultModel, string endPoint, object data)
        {
            var strAPIResponse = GETResponse(endPoint, data).Result;
            var result = JsonConvert.DeserializeObject<T>(strAPIResponse);
            return result;
        }

        public static T RunAPIServiceRequestGETWithToken<T>(T resultModel, string endPoint, object data, string token)
        {
            var strAPIResponse = GETResponseWithToken(endPoint, data, token).Result;
            var result = JsonConvert.DeserializeObject<T>(strAPIResponse);
            return result;
        }

        public static async Task<string> GETResponse(string WEBSERVICE_URL, object data)
        {
            string response = "";
            try
            {
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(WEBSERVICE_URL),
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                var responseClient = await client.SendAsync(request).ConfigureAwait(false);
                var responseBody = await responseClient.Content.ReadAsStringAsync();
                response = responseBody.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("API Err : " + ex.Message);
            }

            return response;
        }

        public static async Task<string> GETResponseWithToken(string WEBSERVICE_URL, object data, string token)
        {
            string response = "";
            try
            {
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(WEBSERVICE_URL),
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Add("token", token);
                }

                var responseClient = await client.SendAsync(request).ConfigureAwait(false);
                var responseBody = await responseClient.Content.ReadAsStringAsync();
                response = responseBody.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("API Err : " + ex.Message);
            }

            return response;
        }

        public static T RunAPIServiceRequestPOST<T>(T resultModel, string endPoint, object data, string website_key)
        {
            var strAPIResponse = POSTResponse(endPoint, data, website_key, null);
            var result = JsonConvert.DeserializeObject<T>(strAPIResponse);
            return result;
        }

        public static T RunAPIServiceRequestPOSTWithToken<T>(T resultModel, string endPoint, object data, string token)
        {
            var strAPIResponse = POSTResponseWithToken(endPoint, data, token, null);
            var result = JsonConvert.DeserializeObject<T>(strAPIResponse);
            return result;
        }

        public static T RunAPIServiceRequestPOST<T>(T resultModel, string endPoint, object data)
        {
            var strAPIResponse = POSTResponse(endPoint, data, null, null);
            var result = JsonConvert.DeserializeObject<T>(strAPIResponse);
            return result;
        }

        public static string POSTResponse(string WEBSERVICE_URL, object data, string website_key, NetworkCredential credential)
        {
            string response = "";
            try
            {
                var webRequest = WebRequest.Create(WEBSERVICE_URL);

                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 180000;
                    webRequest.ContentType = "application/json";

                    if (!string.IsNullOrEmpty(website_key))
                    {
                        webRequest.Headers.Add("website_key", website_key);
                    }

                    if (credential != null)
                    {
                        webRequest.Credentials = credential;
                    }

                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(data);
                        streamWriter.Write(json);
                    }

                    using (StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        response = jsonResponse.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("API Err : " + ex.Message);
            }

            return response;
        }

        public static string POSTResponseWithToken(string WEBSERVICE_URL, object data, string token, NetworkCredential credential)
        {
            string response = "";
            try
            {
                var webRequest = WebRequest.Create(WEBSERVICE_URL);
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 180000;
                    webRequest.ContentType = "application/json";

                    if (!string.IsNullOrEmpty(token))
                    {
                        webRequest.Headers.Add("token", token);
                    }

                    if (credential != null)
                    {
                        webRequest.Credentials = credential;
                    }

                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(data);
                        streamWriter.Write(json);
                    }

                    using (StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        response = jsonResponse.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("API Err : " + ex.Message);
            }

            return response;
        }
    }
}
