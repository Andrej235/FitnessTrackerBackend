﻿using FitnessTracker.DTOs.Responses.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [Authorize]
        [HttpGet("{creator}/{name}/exercise-chart-data/{exerciseId:int}")]
        [ProducesResponseType(typeof(WorkoutExerciseChartDataResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetChartData(string creator, string name, int exerciseId, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await workoutService.GetChartDataForExercise(userId, creator, name, exerciseId, offset, limit));
        }

        [Authorize]
        [HttpGet("{creator}/{name}/mock-exercise-chart-data/{exerciseId:int}")]
        [ProducesResponseType(typeof(WorkoutExerciseChartDataResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Mock(string creator, string name, int exerciseId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await workoutService.MockChartDataForExercise(userId, creator, name, exerciseId));
        }
    }
}