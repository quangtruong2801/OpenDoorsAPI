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
    public class JobsController : ControllerBase
    {
        private readonly JobService _service;

        public JobsController(JobService service)
        {
            _service = service;
        }

        // ---------------- GET ALL JOBS ----------------
        [Authorize] // ✅ Bắt buộc đăng nhập mới xem được danh sách
        [HttpGet]
        public async Task<ActionResult<List<Job>>> Get() =>
            Ok(await _service.GetAllAsync());

        // ---------------- GET JOB BY ID ----------------
        [Authorize] // ✅ Bắt buộc đăng nhập
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Job>> GetById(string id)
        {
            var job = await _service.GetByIdAsync(id);
            return job is null ? NotFound() : Ok(job);
        }

        // ---------------- CREATE JOB (Admin Only) ----------------
        [Authorize(Roles = "admin")] // ✅ Chỉ admin mới được tạo job
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Job job)
        {
            job.JobId = null;
            await _service.CreateAsync(job);
            return CreatedAtAction(nameof(GetById), new { id = job.JobId }, job);
        }

        // ---------------- UPDATE JOB (Admin Only) ----------------
        [Authorize(Roles = "admin")] // ✅ Chỉ admin mới được sửa job
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Job job)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null) return NotFound();

            job.JobId = existing.JobId; // giữ nguyên ID
            await _service.UpdateAsync(id, job);
            return NoContent();
        }

        // ---------------- DELETE JOB (Admin Only) ----------------
        [Authorize(Roles = "admin")] // ✅ Chỉ admin mới được xóa job
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
