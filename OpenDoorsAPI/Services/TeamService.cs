using MongoDB.Driver;
using OpenDoorsAPI.Models;

namespace OpenDoorsAPI.Services
{
    public class TeamService
    {
        private readonly IMongoCollection<Team> _teams;

        public TeamService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _teams = database.GetCollection<Team>(config["MongoDB:TeamsCollection"]);
        }

        public async Task<List<Team>> GetAllAsync() => await _teams.Find(_ => true).ToListAsync();
        public async Task<Team?> GetByIdAsync(string id) => await _teams.Find(t => t.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Team team) => await _teams.InsertOneAsync(team);
        public async Task UpdateAsync(string id, Team team) => await _teams.ReplaceOneAsync(t => t.Id == id, team);
        public async Task DeleteAsync(string id) => await _teams.DeleteOneAsync(t => t.Id == id);
    }
}
