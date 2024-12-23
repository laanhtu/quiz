using System.Collections.Generic;
using System.Linq;

using CP.Api.Models;
using CP.Api.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;
using Elsa.Api.Helpers;

namespace CP.Api.Services
{
  public class RoleService : IRoleService
  {
    private readonly IMongoCollection<Role> _roles;
    private readonly IDatabaseSettings _dbSettings;

    public RoleService(IDatabaseSettings dbSettings)
    {
      _dbSettings = dbSettings;

      var client = new MongoClient(dbSettings.ConnectionString);
      var database = client.GetDatabase(dbSettings.DatabaseName);

      _roles = database.GetCollection<Role>(dbSettings.RoleCollectionName);
    }

    private List<Role> Get() => _roles.Find(Role => true).ToList();
    public IEnumerable<Role> GetAll()
    {
      return Get();
    }
    public async Task<Role> Create(Role role)
    {

      await _roles.InsertOneAsync(role);

      return role;
    }
    public void Update(Role role) => _roles.ReplaceOne(u => u.Id == role.Id, role);

    public async Task UpdateAsync(Role role)
    {
      await _roles.ReplaceOneAsync(u => u.Id == role.Id, role);
    }
  }
}