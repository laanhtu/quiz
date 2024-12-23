using System.Collections.Generic;
using CP.Api.Models;
using System.Threading.Tasks;
using System.Security.Claims;


namespace CP.Api.Services
{
    public interface IUserService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret);

        string GenerateAccessToken(IEnumerable<Claim> claims, string secret);

        string GenerateRefreshToken();

        IEnumerable<User> GetAll();

        Task<User> Create(User User);
        void Update(string id, User user);
        Task UpdateAsync(string id, User user);

        User Get(string username);
    }
}