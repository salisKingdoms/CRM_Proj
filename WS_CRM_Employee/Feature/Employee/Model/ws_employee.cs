namespace WS_CRM_Employee.Feature.Employee.Model
{
    public class ws_employee
    {
        public string nip { get; set; }
        public string name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public bool? active { get; set; }
        public DateTime? created_on { get; set; }
        public string? created_by { get; set; }
        public DateTime? modified_on { get; set; }
        public string? modified_by { get; set; }
    }
}
