using EXE_BE.Controllers.ViewModel;
using EXE_BE.Models;
using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly ChallengeService _challengeService;
        public ChallengeController(ChallengeService challengeService)
        {
            _challengeService = challengeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChallenges()
        {
            var challenges = await _challengeService.GetAllChallengesAsync();
            return Ok(challenges);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChallengeById(int id)
        {
            var challenge = await _challengeService.GetChallengeByIdAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }
            return Ok(challenge);
        }
        [HttpPost]
        public async Task<IActionResult> CreateChallenge([FromBody] ChalengeRequest challengeRequest)
        {
            var challenge = new Challenge
            {
                Name = challengeRequest.Name,
                Description = challengeRequest.Description,
                StartDate = challengeRequest.StartDate,
                isComplete = false,
                EndDate = challengeRequest?.EndDate,
            };
            await _challengeService.CreateChallengeAsync(challenge);
            return CreatedAtAction(nameof(GetChallengeById), new { id = challenge.Id }, challenge);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChallenge(int id, [FromBody] Models.Challenge challenge)
        {
            if (id != challenge.Id)
            {
                return BadRequest();
            }
            await _challengeService.UpdateChallengeAsync(challenge);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            await _challengeService.DeleteChallengeAsync(id);
            return NoContent();
        }

    }
}
