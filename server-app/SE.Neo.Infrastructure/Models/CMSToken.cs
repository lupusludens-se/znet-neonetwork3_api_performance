namespace SE.Neo.Infrastructure.Models
{
    public class CMSToken
    {
        public string token_type { get; set; }
        public int iat { get; set; }
        public int expires_in { get; set; }
        public string jwt_token { get; set; }
    }
}
