namespace WS_CRM.Feature.Activity.dto
{
    public class CreateTicket
    {
        public string ticket_no { get; set; }
        public string status { get; set; }
        public long customer_id { get; set; }
        public string service_center { get; set; }
        public string assign_to { get; set; }
        public string payment_method { get; set; }
        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
    }

    public class CreateTicketUnit
    {
        public string ticket_no { get; set; }
        public string sku_code { get; set; }
        public string product_name { get; set; }
        public int qty { get; set; }
        public int unit_line_no { get; set; }
        public string warranty_no { get; set; }
        public bool active { get; set; }
        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
    }

    public class CreateTicketSparepart
    {
        public string ticket_no { get; set; }
        public string sparepart_code { get; set; }
        public string sparepart_name { get; set; }
        public string? product_name { get; set; }
        public int? qty { get; set; }
        public int? unit_line_no { get; set; }
        public string? uom { get; set; }
        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
    }
    public class CreateTiketBase
    {
        public CreateTicket ticket_header { get; set; }
        public List<CreateTicketUnit> ticket_unit { get; set; }
        public List<CreateTicketSparepart> ticket_sparepart { get; set; }
    }

    public class TicketDetailRespon
    {
        public string ticket_no { get; set; }
        public string status { get; set; }
        public long customer_id { get; set; }
        public string service_center { get; set; }
        public string assign_to { get; set; }
        public string assign_name { get; set; }
        public string payment_method { get; set; }
        public List<CreateTicketUnit> ticket_unit { get; set; }
        public List<CreateTicketSparepart> ticket_sparepart { get; set; }
        public CustomerRespon customer { get; set; }
    }
}
