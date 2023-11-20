﻿namespace WS_CRM.Feature.Activity.Model
{
    public class ws_warranty
    {
        public long? id { get; set; }
        public string warranty_no { get; set; }
        public string company_code { get; set; }
        public string receipt_no { get; set; }
        public DateTime? receipt_date { get; set; }
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
        public string modified_by { get; set; }
        public DateTime? modified_on { get; set; }
        public string warranty_code { get; set; }
        public DateTime? replace_expired_date { get; set; }
        public DateTime? spare_expired_date { get; set; }
        public DateTime? service_expired_date { get; set; }

    }
}
