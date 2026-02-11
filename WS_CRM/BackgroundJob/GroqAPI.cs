namespace WS_CRM.BackgroundJob
{
    public class GroqAPI
    {
    }

    public class AIResult
    {
        public string category { get; set; }
        public string severity { get; set; }
    }

    public class GroqResponse
    {
        public Choice[] choices { get; set; }

        public class Choice
        {
            public Message message { get; set; }
        }

        public class Message
        {
            public string content { get; set; }
        }
    }

    public class AIJob
    {
        public long UnitId { get; set; }
        public string ComplaintText { get; set; }
        public string WarrantyNo { get; set; }
    }
}
