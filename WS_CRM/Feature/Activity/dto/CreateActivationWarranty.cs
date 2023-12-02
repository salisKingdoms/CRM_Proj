namespace WS_CRM.Feature.Activity.dto
{
    public class CreateActivationWarranty
    {
        public string warranty_no { get; set; }
        public string company_code { get; set; }
        public string invoice_no { get; set; }
        public DateTime? invoice_date { get; set; }
        public string article_code { get; set; }
        public string article_name { get; set; }
        public string serial_no { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string activate_by { get; set; }
        public DateTime? activate_on { get; set; }
        public bool active { get; set; }
        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
        public string warranty_code { get; set; }
    }
    public class UpdateWarrantyRequest
    {
        public long? id { get; set; }
        public string? company_code { get; set; }
        public string? invoice_no { get; set; }
        public DateTime? invoice_date { get; set; }
        public string? article_code { get; set; }
        public string? article_name { get; set; }
        public string? serial_no { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}
