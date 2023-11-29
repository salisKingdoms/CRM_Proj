using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using WS_CRM.Config;


namespace WS_CRM.Feature.Catalog.dto
{
    public class ms_product_database_filter
    {
        [DisplayName("id")]
        [CustomAttribute("Equal")]
        [Column("id",Order =1)]
        public long id { get; set; }

        [DisplayName("sku_code")]
        [CustomAttribute("Like")]
        [Column("sku_code", Order = 2)]
        public string sku_code { get; set; }

        [DisplayName("product_name")]
        [CustomAttribute("Like")]
        [Column("product_name", Order = 3)]
        public string product_name { get; set; }

        [DisplayName("qty")]
        [CustomAttribute("Equal")]
        [Column("qty", Order = 4)]
        public int? qty { get; set; }

        [DisplayName("is_trade_in")]
        [CustomAttribute("Equal")]
        [Column("is_trade_in", Order = 5)]
        public bool? is_trade_in { get; set; }

        [DisplayName("unit_line_no")]
        [CustomAttribute("Equal")]
        [Column("unit_line_no", Order = 6)]
        public int unit_line_no { get; set; }

        [DisplayName("created_by")]
        [CustomAttribute("Like")]
        [Column("created_by", Order = 7)]
        public string created_by { get; set; }

        [DisplayName("created_on")]
        [CustomAttribute("Equal")]
        [Column("created_on", Order = 8)]
        public DateTime created_on { get; set; }

        [DisplayName("modified_by")]
        [CustomAttribute("Like")]
        [Column("modified_by", Order = 9)]
        public string modified_by { get; set; }

        [DisplayName("modified_on")]
        [CustomAttribute("Equal")]
        [Column("modified_on", Order = 10)]
        public DateTime modified_on { get; set; }

    }
}
