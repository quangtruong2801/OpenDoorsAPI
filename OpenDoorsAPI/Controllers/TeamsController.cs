using Microsoft.AspNetCore.Mvc;
using OpenDoorsAPI.Models;
using OpenDoorsAPI.Services;

namespace OpenDoorsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService _teamService;
        private readonly MemberService _memberService;

        public TeamsController(TeamService teamService, MemberService memberService)
        {
            _teamService = teamService;
            _memberService = memberService;
        }

        // GET: api/teams
        [HttpGet]
        public async Task<IActionResult> GetTeamsWithMemberCount()
        {
            var teams = await _teamService.GetAllAsync();
            var members = await _memberService.GetAllAsync();

            var result = teams.Select(team => new
            {
                Id = team.Id,
                TeamName = team.TeamName,
                CreatedDate = team.CreatedDate,
                Members = members.Count(m => m.TeamId == team.Id) // ✅ số lượng member theo teamID
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var team = await _teamService.GetByIdAsync(id);
            return team is null ? NotFound() : Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Team team)
        {
            team.CreatedDate = DateTime.UtcNow;
            team.Id = null;
            await _teamService.CreateAsync(team);
            return CreatedAtAction(nameof(GetById), new { id = team.Id }, team);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Team team)
        {
            var existing = await _teamService.GetByIdAsync(id);
            if (existing is null) return NotFound();

            team.Id = existing.Id;
            team.CreatedDate = existing.CreatedDate;
            await _teamService.UpdateAsync(id, team);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _teamService.GetByIdAsync(id);
            if (existing is null) return NotFound();

            await _teamService.DeleteAsync(id);
            return NoContent();
        }
    }
}
