namespace WS_CRM_Customer.Feature.Customer.Dto
{
    public class CreateMembersRequest
    {
        public long customer_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
