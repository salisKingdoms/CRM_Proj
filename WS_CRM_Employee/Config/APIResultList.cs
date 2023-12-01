namespace WS_CRM_Employee.Config
{
    public class APIResultList<T> : APIResult
    {
        public T data { get; set; }
        public int totalRow { get; set; }
    }
}
