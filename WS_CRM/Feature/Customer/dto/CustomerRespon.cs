using System.ComponentModel.DataAnnotations;
using WS_CRM.Feature.Customer.Model;
namespace WS_CRM.Feature.Customer.dto
{
    public class CustomerRespon
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

    public class CreateCustomerRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public bool is_member { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
    }
    public class UpdateCustomerRequest
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public bool is_member { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_on { get; set; }
    }
}
