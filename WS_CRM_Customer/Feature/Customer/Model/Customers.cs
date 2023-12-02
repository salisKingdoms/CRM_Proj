namespace WS_CRM_Customer.Feature.Customer.Model
{
    public class Customers
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public bool is_member { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_on { get; set; }
    }
}
