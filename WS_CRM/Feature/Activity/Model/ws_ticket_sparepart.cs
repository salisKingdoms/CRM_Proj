namespace WS_CRM.Feature.Activity.Model
{
    public class ws_ticket_sparepart
    {
        public string ticket_no { get; set; }
        public string? sparepart_code { get; set; }
        public string? sparepart_name { get; set; }
        public string? product_name { get; set; }
        public int? qty { get; set; }
        public int? unit_line_no { get; set; }
        public string? uom { get; set; }
        public string? created_by { get; set; }
        public DateTime? created_on { get; set; }
        public string? modified_by { get; set; }
        public DateTime? modified_on { get; set; }

    }
}
