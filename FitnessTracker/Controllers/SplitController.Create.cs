using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSplitRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            //TODO: If the split is public and one or more of the workouts are private, make the split private

            var mapped = createRequestMapper.Map(request);
            mapped.CreatorId = userId;

            await createService.Add(mapped);
            return Ok();
        }
    }
}
