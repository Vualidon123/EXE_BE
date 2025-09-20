using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeProgressController : ControllerBase
    {
        private readonly ChallengeProgressService _challengeProgressService;
        public ChallengeProgressController(ChallengeProgressService challengeProgressService)
        {
            _challengeProgressService = challengeProgressService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChallengeProgress()
        {
            var progressList = await _challengeProgressService.GetAllChallengeProgressAsync();
            return Ok(progressList);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChallengeProgressById(int id)
        {
            var progress = await _challengeProgressService.GetChallengeProgressByIdAsync(id);
            if (progress == null)
            {
                return NotFound();
            }
            return Ok(progress);
        }
        [HttpPost]
        public async Task<IActionResult> CreateChallengeProgress([FromBody] Models.ChallengeProgress progress)
        {
            await _challengeProgressService.CreateChallengeProgressAsync(progress);
            return CreatedAtAction(nameof(GetChallengeProgressById), new { id = progress.Id }, progress);
        }
        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdateChallengeProgress(int id, [FromBody] Models.ChallengeProgress progress)
        {
            if (id != progress.Id)
            {
                return BadRequest();
            }
            await _challengeProgressService.UpdateChallengeProgressAsync(progress);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallengeProgress(int id)
        {
            await _challengeProgressService.DeleteChallengeProgressAsync(id);
            return NoContent();
        }*/
    }

}
