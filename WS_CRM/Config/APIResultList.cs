namespace WS_CRM.Config
{
    public class APIResultList<T>: APIResult
    {
        public T data { get; set; }
        public int totalRow { get; set; }

    }
}
