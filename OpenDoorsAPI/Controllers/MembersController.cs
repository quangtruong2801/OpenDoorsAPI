using Microsoft.AspNetCore.Mvc;
using OpenDoorsAPI.Models;
using OpenDoorsAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenDoorsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly MemberService _service;

        public MembersController(MemberService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Member>>> Get() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Member>> GetById(string id)
        {
            var member = await _service.GetByIdAsync(id);
            return member == null ? NotFound() : Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Member member)
        {
            member.Id = null; // 🔹 Không cho client tự truyền Id
            await _service.CreateAsync(member);
            return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Member member)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            member.Id = existing.Id; // 🔹 Giữ nguyên Id cũ
            await _service.UpdateAsync(id, member);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.DeleteAsync(id); // 🔹 Xóa member + ảnh trên Cloudinary
            return NoContent();
        }
    }
}
