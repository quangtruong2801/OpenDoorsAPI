using MongoDB.Driver;
using OpenDoorsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDoorsAPI.Services
{
    public class MemberService
    {
        private readonly IMongoCollection<Member> _members;
        private readonly CloudinaryService _cloudinaryService;

        public MemberService(IMongoDatabase database, CloudinaryService cloudinaryService)
        {
            _members = database.GetCollection<Member>("Members");
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<Member>> GetAllAsync() =>
            await _members.Find(_ => true).ToListAsync();

        public async Task<Member> GetByIdAsync(string id) =>
            await _members.Find(m => m.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Member member) =>
            await _members.InsertOneAsync(member);

        public async Task UpdateAsync(string id, Member member) =>
            await _members.ReplaceOneAsync(m => m.Id == id, member);

        public async Task DeleteAsync(string id)
        {
            var member = await _members.Find(m => m.Id == id).FirstOrDefaultAsync();
            if (member == null) return;

            // 🔹 Xóa ảnh trên Cloudinary nếu có
            if (!string.IsNullOrEmpty(member.AvatarPublicId))
            {
                await _cloudinaryService.DeleteImageAsync(member.AvatarPublicId);
            }

            // 🔹 Xóa member khỏi MongoDB
            await _members.DeleteOneAsync(m => m.Id == id);
        }
    }
}
