﻿using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{splitId:guid}/comment")]
        public async Task<IActionResult> CreateComment(Guid splitId, [FromBody] CreateSplitCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                var mapped = createCommentRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.SplitId = splitId;

                var newId = await commentCreateService.Add(mapped);
                if (newId == default)
                    return BadRequest("Failed to create comment");

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{splitId:guid}/comment/{parentId:guid}/reply")]
        public async Task<IActionResult> CreateComment(Guid splitId, Guid parentId, [FromBody] CreateSplitCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                var mapped = createCommentRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.SplitId = splitId;
                mapped.ParentId = parentId;

                var newId = await commentCreateService.Add(mapped);
                if (newId == default)
                    return BadRequest("Failed to create comment");

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
            }
        }
    }
}
