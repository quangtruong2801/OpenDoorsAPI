using MongoDB.Driver;
using OpenDoorsAPI.Models;

namespace OpenDoorsAPI.Services
{
    public class MemberService
    {
        private readonly IMongoCollection<Member> _members;

        public MemberService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _members = database.GetCollection<Member>(config["MongoDB:MembersCollection"]);
        }

        public async Task<List<Member>> GetAllAsync() => await _members.Find(_ => true).ToListAsync();
        public async Task<Member?> GetByIdAsync(string id) => await _members.Find(m => m.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Member member) => await _members.InsertOneAsync(member);
        public async Task UpdateAsync(string id, Member member) => await _members.ReplaceOneAsync(m => m.Id == id, member);
        public async Task DeleteAsync(string id) => await _members.DeleteOneAsync(m => m.Id == id);
    }
}
