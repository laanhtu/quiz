using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CP.Api.Models;
using CP.Api.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Elsa.Api.Helpers;

namespace CP.Api.Services
{
  public class UserService : IUserService
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UserService));
    private readonly IMongoCollection<User> _users;

    private readonly IDatabaseSettings _dbSettings;

    public UserService(IDatabaseSettings dbSettings)
    {
      _dbSettings = dbSettings;

      var client = new MongoClient(dbSettings.ConnectionString);
      var database = client.GetDatabase(dbSettings.DatabaseName);

      _users = database.GetCollection<User>(dbSettings.UserCollectionName);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims, string secret)
    {
      // authentication successful so generate jwt token
      var tokenHandler = new JwtSecurityTokenHandler();

      var key = Encoding.UTF8.GetBytes(secret);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
        Issuer = "La Anh Tu",
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);

      var accessToken = tokenHandler.WriteToken(token);

      return accessToken;
    }

    public string GenerateRefreshToken()
    {
      var randomNumber = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
    }
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret)
    {
      try
      {
        var key = Encoding.UTF8.GetBytes(secret);
        var tokenValidationParameters = new TokenValidationParameters
        {
          ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
          ValidateIssuer = false,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),

          ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
          throw new SecurityTokenException("Invalid token");

        return principal;
      }
      catch (Microsoft.IdentityModel.Tokens.SecurityTokenInvalidSignatureException stise)
      {
        log.Error(stise.Message, stise);
      }
      catch (Exception ex)
      {
        log.Error(ex.Message);
      }

      return null;
    }

    public IEnumerable<User> GetAll()
    {
      // return users without passwords
      return Get().Select(x =>
      {
        x.Password = null;
        return x;
      });
    }

    public List<User> Get() => _users.Find(User => true).ToList();

    public User Get(string username) => _users.Find<User>(u => u.Username == username).FirstOrDefault();

    public async Task<User> Create(User user)
    {
      user.Password = PasswordHash.HashPassword(user.Password);

      await _users.InsertOneAsync(user);
      return user;
    }

    public void Update(string id, User user) => _users.ReplaceOne(u => u.Id == id, user);

    public async Task UpdateAsync(string id, User user)
    {
      await _users.ReplaceOneAsync(u => u.Id == id, user);
    }

    public void Remove(User UserIn) => _users.DeleteOne(User => User.Id == UserIn.Id);

    public void Remove(string id) =>
        _users.DeleteOne(u => u.Id == id);
  }
}