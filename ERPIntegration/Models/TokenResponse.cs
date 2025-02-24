namespace ERPIntegration.Models
{
    public class TokenResponse
    {
        public string access_token { get; set; } = string.Empty;
        public int expires_in { get; set; }
    }
}
