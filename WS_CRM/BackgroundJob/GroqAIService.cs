using Dapper;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WS_CRM.Config;
using WS_CRM.Helper;

namespace WS_CRM.BackgroundJob
{
    public class GroqAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IDbConnection _db;
        private DataContext _context;
        private readonly AppConfig _appConfig;

        public GroqAIService(HttpClient httpClient, DataContext context, AppConfig appConfig)
        {
            _httpClient = httpClient;
            _context = context;
            _appConfig = appConfig;
        }


        public async Task AnalyzeAndSaveAsync(long unitId, string warrantyNo, string complaint)
        {
            using var connection = _context.CreateConnection();
            var aiResult = await CallGroqApi(complaint);

            var insertLOG = @"
            INSERT INTO ws_ticket_unit_ai
            (warranty_no, complaint_text, ai_category, ai_severity,
             ai_suggested_action, created_on,created_by, ticket_unit_id )
            VALUES (@WarrantyNo, @Complaint, @Category,
                    @Severity, @Action,@CreatedON, @CreatedBy, @UnitID )";

            var updateUnit = @" UPDATE ws_ticket_unit SET ai_category=@Category, ai_severity=@Severity, ai_last_processed=@CreatedON 
                                WHERE unit_line_no=@UnitID AND warranty_no=@WarrantyNo";

            var param = new Dictionary<string, object>
            {
                { "WarrantyNo", warrantyNo },
                { "Complaint", complaint ?? "" },
                { "UnitID", unitId },
                { "Category", aiResult.category },
                { "Severity", aiResult.severity ?? "" },
                { "Action", "" },
                { "CreatedBy", "job"},
                { "CreatedON", DateTime.UtcNow },

            };
            await connection.ExecuteAsync(insertLOG + ";" + updateUnit, param);

        }


        private async Task<AIResult> CallGroqApi(string complaint)
        {
            string aiContent = string.Empty;
            try
            {
                var apiKey = _appConfig.GroqAIKey;

                var requestBody = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = new[]
                    {
                new {
                    role = "system",
                    content = "You classify warranty complaints. Choose ONLY from the allowed categories and severity. Return ONLY JSON with two fields: category and severity No extra text."
                },
                new {
                    role = "user",
                    content = $"Allowed categories: Hardware Failure, Software Issue, Installation Problem, User Error, Cosmetic Damage. Allowed severity: Low, Medium, High, Critical. Complaint: {complaint}"
                }
            },
                    temperature = 0
                };

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://api.groq.com/openai/v1/chat/completions"
                );

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                request.Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();

                var groqResponse = JsonSerializer.Deserialize<GroqResponse>(json);

                var content = groqResponse.choices[0].message.content;
                var cleanJson = ExtractJson(content);
                return JsonSerializer.Deserialize<AIResult>(cleanJson);

            }
            catch (Exception ex)
            {
                string errors = ex.Message;
                aiContent = errors;

                // fallback biar sistem gak crash
                return new AIResult
                {
                    category = "Unknown",
                    severity = "Low"
                };
            }

        }

        private string ExtractJson(string text)
        {
            var start = text.IndexOf("{");
            var end = text.LastIndexOf("}");

            if (start >= 0 && end >= 0)
                return text.Substring(start, end - start + 1);

            throw new Exception("No valid JSON found in AI response");
        }
    }
}
