using MongoDB.Driver;
using OpenDoorsAPI.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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

        // ---------------- CRUD ----------------
        public async Task<List<Member>> GetAllAsync() =>
            await _members.Find(_ => true).ToListAsync();

        public async Task<Member> GetByIdAsync(string id) =>
            await _members.Find(m => m.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Member member)
        {
            // 🔹 Hash password trước khi lưu
            if (!string.IsNullOrEmpty(member.Password))
                member.Password = HashPassword(member.Password);

            await _members.InsertOneAsync(member);
        }

        public async Task UpdateAsync(string id, Member member)
        {
            var existing = await _members.Find(m => m.Id == id).FirstOrDefaultAsync();
            if (existing == null) return;

            // Giữ password cũ nếu không có password mới
            if (string.IsNullOrWhiteSpace(member.Password))
                member.Password = existing.Password;
            else
                member.Password = HashPassword(member.Password);

            await _members.ReplaceOneAsync(m => m.Id == id, member);
        }

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

        // ---------------- LOGIN SUPPORT ----------------
        public async Task<Member> GetByEmailAsync(string email) =>
            await _members.Find(m => m.Email == email).FirstOrDefaultAsync();

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return false;
            return HashPassword(enteredPassword) == storedHash;
        }

        // ---------------- HELPER ----------------
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
