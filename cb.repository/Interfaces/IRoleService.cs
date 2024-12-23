using System.Collections.Generic;
using CP.Api.Models;
using System.Threading.Tasks;
using System.Security.Claims;


namespace CP.Api.Services
{
  public interface IRoleService
  {
    IEnumerable<Role> GetAll();
    Task<Role> Create(Role role);
    void Update(Role role);
    Task UpdateAsync(Role role);
  }
}