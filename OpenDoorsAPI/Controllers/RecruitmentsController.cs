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

        [HttpGet]
        public async Task<ActionResult<List<Recruitment>>> GetList()
        {
            var recruitments = await _recruitmentService.GetListAsync();
            return Ok(recruitments);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Recruitment>> GetById(string id)
        {
            var recruitment = await _recruitmentService.GetByIdAsync(id);
            if (recruitment == null)
                return NotFound();

            return Ok(recruitment);
        }

        [HttpPost]
        public async Task<ActionResult<Recruitment>> Create(Recruitment recruitment)
        {
            await _recruitmentService.CreateAsync(recruitment);
            return CreatedAtAction(nameof(GetById), new { id = recruitment.Id }, recruitment);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Recruitment recruitment)
        {
            var existing = await _recruitmentService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            recruitment.Id = id;
            await _recruitmentService.UpdateAsync(id, recruitment);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
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
