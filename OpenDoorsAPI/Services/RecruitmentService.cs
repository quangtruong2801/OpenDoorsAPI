using MongoDB.Driver;
using OpenDoorsAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDoorsAPI.Services
{
    public class RecruitmentService
    {
        private readonly IMongoCollection<Recruitment> _recruitments;

        public RecruitmentService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _recruitments = database.GetCollection<Recruitment>(config["MongoDB:RecruitmentsCollection"]);
        }

        public async Task<List<Recruitment>> GetListAsync()
        {
            return await _recruitments.Find(_ => true).ToListAsync();
        }

        public async Task<Recruitment> GetByIdAsync(string id)
        {
            return await _recruitments.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Recruitment> CreateAsync(Recruitment recruitment)
        {
            await _recruitments.InsertOneAsync(recruitment);
            return recruitment;
        }

        public async Task UpdateAsync(string id, Recruitment recruitment)
        {
            await _recruitments.ReplaceOneAsync(r => r.Id == id, recruitment);
        }

        public async Task DeleteAsync(string id)
        {
            await _recruitments.DeleteOneAsync(r => r.Id == id);
        }
    }
}
