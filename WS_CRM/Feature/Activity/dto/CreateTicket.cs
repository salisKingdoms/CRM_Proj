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
}
