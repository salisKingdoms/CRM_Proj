namespace WS_CRM.Feature.Customer.Model
{
    public class Members
    {
        public long? id { get; set; }
        public long cust_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public string modofied_by { get; set; }
        public DateTime modified_on { get; set; }
        public bool is_active { get; set; }
    }
}
