using Microsoft.AspNetCore.Mvc;
using OpenDoorsAPI.Models;
using OpenDoorsAPI.Services;

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
        public async Task<ActionResult<List<Member>>> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Member>> GetById(string id)
        {
            var member = await _service.GetByIdAsync(id);
            return member is null ? NotFound() : Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Member member)
        {
            // 🔥 Không cho phép client tự truyền Id
            member.Id = null;

            await _service.CreateAsync(member);
            return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Member member)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null) return NotFound();

            // 🔥 Giữ nguyên Id cũ
            member.Id = existing.Id;

            await _service.UpdateAsync(id, member);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
