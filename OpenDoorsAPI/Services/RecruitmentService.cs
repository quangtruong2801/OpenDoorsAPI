using MongoDB.Driver;
using OpenDoorsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDoorsAPI.Services
{
    public class RecruitmentService
    {
        private readonly IMongoCollection<Recruitment> _recruitments;

        public RecruitmentService(IMongoDatabase database)
        {
            _recruitments = database.GetCollection<Recruitment>("Recruitments"); // collection cố định
        }

        // ---------------- GET LIST ----------------
        public async Task<List<Recruitment>> GetListAsync() =>
            await _recruitments.Find(_ => true).ToListAsync();

        // ---------------- GET BY ID ----------------
        public async Task<Recruitment?> GetByIdAsync(string id) =>
            await _recruitments.Find(r => r.Id == id).FirstOrDefaultAsync();

        // ---------------- CREATE ----------------
        public async Task CreateAsync(Recruitment recruitment)
        {
            recruitment.Id = null; // không cho client set Id
            await _recruitments.InsertOneAsync(recruitment);
        }

        // ---------------- UPDATE ----------------
        public async Task UpdateAsync(string id, Recruitment recruitment) =>
            await _recruitments.ReplaceOneAsync(r => r.Id == id, recruitment);

        // ---------------- DELETE ----------------
        public async Task DeleteAsync(string id) =>
            await _recruitments.DeleteOneAsync(r => r.Id == id);
    }
}
