using Microsoft.AspNetCore.Authorization;
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

        // ---------------- GET ALL MEMBERS ----------------
        [Authorize] 
        [HttpGet]
        public async Task<ActionResult<List<Member>>> Get() =>
            Ok(await _service.GetAllAsync());

        // ---------------- GET MEMBER BY ID ----------------
        [Authorize] 
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Member>> GetById(string id)
        {
            var member = await _service.GetByIdAsync(id);
            return member == null ? NotFound() : Ok(member);
        }

        // ---------------- CREATE MEMBER (Admin Only) ----------------
        [Authorize(Roles = "admin")] 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Member member)
        {
            member.Id = null;
            await _service.CreateAsync(member);
            return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
        }

        // ---------------- UPDATE MEMBER (Admin Only) ----------------
        [Authorize(Roles = "admin")] 
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Member member)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            member.Id = existing.Id;
            await _service.UpdateAsync(id, member);
            return NoContent();
        }

        // ---------------- DELETE MEMBER (Admin Only) ----------------
        [Authorize(Roles = "admin")] 
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
