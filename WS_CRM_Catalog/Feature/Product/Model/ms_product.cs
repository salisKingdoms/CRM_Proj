namespace WS_CRM_Catalog.Feature.Product.Model
{
    public class ms_product
    {
        public long id { get; set; }
        public string sku_code { get; set; }
        public string product_name { get; set; }
        public int? qty { get; set;}
        public bool? is_trade_in { get; set; }
        public int unit_line_no { get; set; }
        public string created_by { get; set;}
        public DateTime created_on { get; set; }
        public string modified_by { get; set;}
        public DateTime modified_on { get; set; }
    }
}
