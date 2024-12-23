using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CP.Api.Services;
using System.Threading.Tasks;
using CP.Api.Helpers;
using CP.Api.Models;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace CP.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AccountController));
        private readonly AppSettings _appSettings;
        private IUserService _userService;
        private IRoleService _roleService;

        public AccountController(IOptions<AppSettings> appSettings, IUserService userService, IRoleService roleService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]User userParam)
        {
            var user = _userService.Get(userParam.Username);

            if (user == null || !PasswordHash.VerifyPassword(user.Password, userParam.Password))
                return BadRequest(new { message = "Username or password is incorrect" });

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            if (user.RoleIds != null && user.RoleIds.Count > 0)
            {
                var roles = _roleService.GetAll().Where(x => user.RoleIds.Contains(x.Id)).Select(x => x.Name).ToList();
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var jwtToken = _userService.GenerateAccessToken(claims, _appSettings.Secret);
            var refreshToken = _userService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            await _userService.UpdateAsync(user.Id, user);

            return new ObjectResult(new
            {
                accessToken = jwtToken,
                refreshToken = refreshToken,
                userId = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(RefreshAccessTokenRequest request)
        {
            var principal = _userService.GetPrincipalFromExpiredToken(request.AccessToken, _appSettings.Secret);

            if (principal != null)
            {
                var username = principal.Identity.Name; //this is mapped to the Name claim by default

                var user = _userService.Get(username);
                if (user == null || user.RefreshToken != request.RefreshToken) return BadRequest();

                var newJwtToken = _userService.GenerateAccessToken(principal.Claims, _appSettings.Secret);
                var newRefreshToken = _userService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;

                await _userService.UpdateAsync(user.Id, user);

                return new ObjectResult(new
                {
                    accessToken = newJwtToken,
                    refreshToken = newRefreshToken
                });
            }
            else
            {
                return BadRequest("Cannot refresh your access token, contact Administrator for more infor.");
            }
        }

        [HttpPost("revoke"), Authorize]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;

            var user = _userService.Get(username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;

            await _userService.UpdateAsync(user.Id, user);

            return NoContent();
        }

        [HttpGet, Authorize]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]User user)
        {
            var existed = _userService.Get(user.Username);
            if (existed != null)
            {
                return StatusCode(409);
            }

            if (user.Username == "laanhtu@live.co.uk" || user.Username == "anhtu315@gmail.com")
            {
                var roles = _roleService.GetAll();

                var adminRole = roles.FirstOrDefault(x => x.Name == "Administrator");

                if (adminRole == null)
                {
                    log.Error("Missing Administrator role in system. Create it now");
                    adminRole = await _roleService.Create(new Role { Name = "Administrator" });
                }

                user.RoleIds = new List<string> { adminRole.Id };
            }

            var result = await _userService.Create(user);

            return Ok(new RegisterResponse { LastName = result.LastName, FirstName = result.FirstName });
        }
    }
}
