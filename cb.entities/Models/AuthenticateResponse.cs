
namespace CP.Api.Models
{
  public class AuthenticateResponse
  {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
  }
}
