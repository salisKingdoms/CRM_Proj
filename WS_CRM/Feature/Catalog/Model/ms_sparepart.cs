namespace WS_CRM.Feature.Catalog.Model
{
    public class ms_sparepart
    {
        public long id { get; set; }
        public string sparepart_code { get; set; }
        public string sparepart_name { get; set; }
        public string sku_code { get; set; }
        public string sku_name { get; set; }
        public string uom { get; set; }
        public int? qty { get; set; }
        public int? warranty_duration  { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_on { get; set; }
    }
}
