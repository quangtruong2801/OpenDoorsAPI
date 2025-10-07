using MongoDB.Driver;
using OpenDoorsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDoorsAPI.Services
{
    public class TeamService
    {
        private readonly IMongoCollection<Team> _teams;

        public TeamService(IMongoDatabase database)
        {
            _teams = database.GetCollection<Team>("Teams");
        }

        // ---------------- GET ALL ----------------
        public async Task<List<Team>> GetAllAsync() =>
            await _teams.Find(_ => true).ToListAsync();

        // ---------------- GET BY ID ----------------
        public async Task<Team?> GetByIdAsync(string id) =>
            await _teams.Find(t => t.Id == id).FirstOrDefaultAsync();

        // ---------------- CREATE ----------------
        public async Task CreateAsync(Team team)
        {
            if (team.CreatedDate == default)
                team.CreatedDate = DateTime.UtcNow;

            await _teams.InsertOneAsync(team);
        }

        // ---------------- UPDATE ----------------
        public async Task UpdateAsync(string id, Team team)
        {
            await _teams.ReplaceOneAsync(t => t.Id == id, team);
        }

        // ---------------- DELETE ----------------
        public async Task DeleteAsync(string id)
        {
            await _teams.DeleteOneAsync(t => t.Id == id);
        }
    }
}
