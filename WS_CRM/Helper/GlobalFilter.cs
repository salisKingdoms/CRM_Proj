namespace WS_CRM.Helper
{
    public class GlobalFilter
    {
        public string? filter { get; set; }
        public string? filter_column { get; set; }
        public int? limit { get; set; }
        public int? offset { get; set; }
        public string? order_by { get; set;}
        public SortDirection sort_by { get; set;}
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }

    
}
