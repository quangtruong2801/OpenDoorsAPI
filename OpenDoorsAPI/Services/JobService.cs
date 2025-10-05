using MongoDB.Driver;
using OpenDoorsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDoorsAPI.Services
{
    public class JobService
    {
        private readonly IMongoCollection<Job> _jobs;

        public JobService(IMongoDatabase database)
        {
            _jobs = database.GetCollection<Job>("Jobs"); // collection cố định
        }

        // ---------------- GET ALL ----------------
        public async Task<List<Job>> GetAllAsync() =>
            await _jobs.Find(_ => true).ToListAsync();

        // ---------------- GET BY ID ----------------
        public async Task<Job?> GetByIdAsync(string id) =>
            await _jobs.Find(j => j.JobId == id).FirstOrDefaultAsync();

        // ---------------- CREATE ----------------
        public async Task CreateAsync(Job job)
        {
            job.JobId = null; // đảm bảo client không truyền Id
            await _jobs.InsertOneAsync(job);
        }

        // ---------------- UPDATE ----------------
        public async Task UpdateAsync(string id, Job job) =>
            await _jobs.ReplaceOneAsync(j => j.JobId == id, job);

        // ---------------- DELETE ----------------
        public async Task DeleteAsync(string id) =>
            await _jobs.DeleteOneAsync(j => j.JobId == id);
    }
}
