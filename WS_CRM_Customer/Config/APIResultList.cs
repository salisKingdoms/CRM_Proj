namespace WS_CRM_Customer.Config
{
    public class APIResultList<T> : APIResult
    {
        public T data { get; set; }
        public int totalRow { get; set; }
    }
}
