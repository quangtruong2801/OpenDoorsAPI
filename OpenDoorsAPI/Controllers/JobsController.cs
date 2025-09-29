using Microsoft.AspNetCore.Mvc;
using OpenDoorsAPI.Models;
using OpenDoorsAPI.Services;

namespace OpenDoorsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly JobService _service;

        public JobsController(JobService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Job>>> Get() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Job>> GetById(string id)
        {
            var job = await _service.GetByIdAsync(id);
            return job is null ? NotFound() : Ok(job);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Job job)
        {
            // ✅ Không cho client tự truyền Id
            job.JobId = null;
            await _service.CreateAsync(job);
            return CreatedAtAction(nameof(GetById), new { id = job.JobId }, job);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Job job)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null) return NotFound();

            job.JobId = existing.JobId; // ✅ giữ nguyên Id
            await _service.UpdateAsync(id, job);
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
