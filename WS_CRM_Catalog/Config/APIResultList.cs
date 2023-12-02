namespace WS_CRM_Catalog.Config
{
    public class APIResultList<T> : APIResult
    {
        public T data { get; set; }
        public int totalRow { get; set; }
    }
}
