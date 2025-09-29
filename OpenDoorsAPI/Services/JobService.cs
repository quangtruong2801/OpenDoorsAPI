using MongoDB.Driver;
using OpenDoorsAPI.Models;

namespace OpenDoorsAPI.Services
{
    public class JobService
    {
        private readonly IMongoCollection<Job> _jobs;

        public JobService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _jobs = database.GetCollection<Job>(config["MongoDB:JobsCollection"]);
        }

        public async Task<List<Job>> GetAllAsync() =>
            await _jobs.Find(_ => true).ToListAsync();

        public async Task<Job?> GetByIdAsync(string id) =>
            await _jobs.Find(j => j.JobId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Job job) =>
            await _jobs.InsertOneAsync(job);

        public async Task UpdateAsync(string id, Job job) =>
            await _jobs.ReplaceOneAsync(j => j.JobId == id, job);

        public async Task DeleteAsync(string id) =>
            await _jobs.DeleteOneAsync(j => j.JobId == id);
    }
}
