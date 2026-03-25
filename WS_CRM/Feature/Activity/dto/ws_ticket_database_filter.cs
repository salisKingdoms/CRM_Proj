using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using WS_CRM.Config;

namespace WS_CRM.Feature.Activity.dto
{
    public class ws_ticket_database_filter
    {
        [DisplayName("ticket_no")]
        [CustomAttribute("Like")]
        [Column("ticket_no", Order = 1)]
        public string ticket_no { get; set; }

        [DisplayName("status")]
        [CustomAttribute("Equal")]
        [Column("status", Order = 2)]
        public string status { get; set; }

        [DisplayName("customer_id")]
        [CustomAttribute("Equal")]
        [Column("customer_id", Order = 3)]
        public long customer_id { get; set; }

        [DisplayName("service_center")]
        [CustomAttribute("Like")]
        [Column("service_center", Order = 4)]
        public string service_center { get; set; }

        [DisplayName("assign_to")]
        [CustomAttribute("Like")]
        [Column("assign_to", Order = 5)]
        public string assign_to { get; set; }

        [DisplayName("payment_method")]
        [CustomAttribute("Like")]
        [Column("payment_method", Order = 6)]
        public string payment_method { get; set; }

        [DisplayName("created_by")]
        [CustomAttribute("Like")]
        [Column("created_by", Order = 7)]
        public string created_by { get; set; }

        [DisplayName("created_on")]
        [CustomAttribute("Equal")]
        [Column("created_on", Order = 8)]
        public DateTime? created_on { get; set; }

        [DisplayName("modified_by")]
        [CustomAttribute("Like")]
        [Column("modified_by", Order = 9)]
        public string modified_by { get; set; }

        [DisplayName("modified_on")]
        [CustomAttribute("Equal")]
        [Column("modified_on", Order = 10)]
        public DateTime? modified_on { get; set; }

        [DisplayName("active")]
        [CustomAttribute("Equal")]
        [Column("active", Order = 11)]
        public bool? active { get; set; }
    }

    public class ws_warranty_db_filter 
    {
        public string warranty_no { get; set; }
        public string? company_code { get; set; }
        public string? invoice_no { get; set; }
        public DateTime? invoice_date { get; set; }
        public string? article_code { get; set; }
        public string? article_name { get; set; }
        public string? serial_no { get; set; }
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
    }
}
