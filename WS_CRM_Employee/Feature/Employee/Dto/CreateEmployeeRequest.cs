namespace WS_CRM_Employee.Feature.Employee.Dto
{
    public class CreateEmployeeRequest
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool active { get; set; }
    }

    public class UpdateEmployeeRequest
    {
        public string nip { get; set; }
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public bool? active { get; set; }

    }
}
