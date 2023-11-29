using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using WS_CRM.Config;

namespace WS_CRM.Feature.Catalog.dto
{
    public class ms_sparepart_database_filter
    {
        [DisplayName("id")]
        [CustomAttribute("Equal")]
        [Column("id", Order = 1)]
        public long id { get; set; }

        [DisplayName("sparepart_code")]
        [CustomAttribute("Like")]
        [Column("sparepart_code", Order = 2)]
        public string sparepart_code { get; set; }

        [DisplayName("sparepart_name")]
        [CustomAttribute("Like")]
        [Column("sparepart_name", Order = 3)]
        public string sparepart_name { get; set; }

        [DisplayName("sku_code")]
        [CustomAttribute("Like")]
        [Column("sku_code", Order = 4)]
        public string sku_code { get; set; }

        [DisplayName("sku_name")]
        [CustomAttribute("Like")]
        [Column("sku_name", Order = 5)]
        public string sku_name { get; set; }

        [DisplayName("uom")]
        [CustomAttribute("Like")]
        [Column("uom", Order = 6)]
        public string uom { get; set; }

        [DisplayName("qty")]
        [CustomAttribute("Equal")]
        [Column("qty", Order = 7)]
        public int qty { get; set; }

        [DisplayName("warranty_duration")]
        [CustomAttribute("Equal")]
        [Column("warranty_duration", Order = 8)]
        public int warranty_duration { get; set; }

        [DisplayName("created_by")]
        [CustomAttribute("Like")]
        [Column("created_by", Order = 9)]
        public string created_by { get; set; }

        [DisplayName("created_on")]
        [CustomAttribute("Equal")]
        [Column("created_on", Order = 10)]
        public DateTime created_on { get; set; }

        [DisplayName("modified_by")]
        [CustomAttribute("Like")]
        [Column("modified_by", Order = 11)]
        public string modified_by { get; set; }

        [DisplayName("modified_on")]
        [CustomAttribute("Equal")]
        [Column("modified_on", Order = 12)]
        public DateTime modified_on { get; set; }
    }
}
