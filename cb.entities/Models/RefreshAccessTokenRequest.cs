
namespace CP.Api.Models
{
    public class RefreshAccessTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
