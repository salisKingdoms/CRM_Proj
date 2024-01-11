namespace WS_CRM.Helper
{
    public class HelperLog
    {
        public static string GetRequestLog(string functionName, string message)
        {
            return functionName + " - Request: " + message;
        }

        public static string GetResponseSuccessLog(string functionName, string message)
        {
            return functionName + " - Response (Success): " + message;
        }

        public static string GetResponseErrorLog(string functionName, string message)
        {
            return functionName + " - Response (Error): " + message;
        }
    }
}
