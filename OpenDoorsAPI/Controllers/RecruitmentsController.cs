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
    public class RecruitmentsController : ControllerBase
    {
        private readonly RecruitmentService _recruitmentService;

        public RecruitmentsController(RecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
        }

        // ---------------- GET LIST (mọi người xem được) ----------------
        [HttpGet]
        [AllowAnonymous] //Cho phép truy cập không cần đăng nhập
        public async Task<ActionResult<List<Recruitment>>> GetList()
        {
            var recruitments = await _recruitmentService.GetListAsync();
            return Ok(recruitments);
        }

        // ---------------- GET BY ID (mọi người xem được) ----------------
        [HttpGet("{id:length(24)}")]
        [AllowAnonymous]
        public async Task<ActionResult<Recruitment>> GetById(string id)
        {
            var recruitment = await _recruitmentService.GetByIdAsync(id);
            if (recruitment == null)
                return NotFound();

            return Ok(recruitment);
        }

        // ---------------- CREATE (Admin Only) ----------------
        [HttpPost]
        [Authorize(Roles = "admin")] 
        public async Task<ActionResult<Recruitment>> Create([FromBody] Recruitment recruitment)
        {
            recruitment.Id = null;
            await _recruitmentService.CreateAsync(recruitment);
            return CreatedAtAction(nameof(GetById), new { id = recruitment.Id }, recruitment);
        }

        // ---------------- UPDATE (Admin Only) ----------------
        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(string id, [FromBody] Recruitment recruitment)
        {
            var existing = await _recruitmentService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            recruitment.Id = existing.Id;
            await _recruitmentService.UpdateAsync(id, recruitment);
            return NoContent();
        }

        // ---------------- DELETE (Admin Only) ----------------
        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _recruitmentService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _recruitmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
