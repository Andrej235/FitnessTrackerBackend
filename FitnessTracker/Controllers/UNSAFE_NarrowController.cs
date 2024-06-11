using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace FitnessTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/unsafe/narrow")]
    [ApiController]
    public class UNSAFE_NarrowController(ExerciseContext context, IReadService<Exercise> readService, IEntityMapper<Exercise, ExerciseDTO> mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] string include)
        {
            var result = context.Exercises.Select(x => new
            {
                id = include.Contains("id") ? (int?)x.Id : null,
                name = include.Contains("name") ? x.Name : null,
                description = include.Contains("description") ? x.Description : null,
                image = include.Contains("image") ? x.EncodedImage : null,
                equipment = include.Contains("equipment") ? x.Equipment.Select(x => x.Id) : null,
                primaryMuscleGroups = include.Contains("primarymusclegroups") ? x.PrimaryMuscleGroups.Select(x => x.Id) : null,
                secondaryMuscleGroups = include.Contains("secondarymusclegroups") ? x.SecondaryMuscleGroups.Select(x => x.Id) : null,
                primaryMuscles = include.Contains("primarymuscles") ? x.PrimaryMuscles.Select(x => x.Id) : null,
                secondaryMuscles = include.Contains("secondarymuscles") ? x.SecondaryMuscles.Select(x => x.Id) : null,
            });

            return Ok(ApplyOffsetAndLimit(result));
        }

        [HttpGet("basic/inline")]
        public IActionResult Get()
        {
            var mappedExercises = context.Exercises.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.EncodedImage,
            });
            List<object> result = [.. mappedExercises.Skip(0)];// ApplyOffsetAndLimit(mappedExercises);

            return Ok(result);
        }

        [HttpGet("basic/function/ienumerable")]
        public IActionResult GetIEnumerable()
        {
            var mappedExercises = context.Exercises.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.EncodedImage,
            });
            var result = ApplyOffsetAndLimit(mappedExercises);

            return Ok(result);
        }

        [HttpGet("basic/function/list")]
        public IActionResult GetList()
        {
            var mappedExercises = context.Exercises.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.EncodedImage,
            });
            var result = ApplyOffsetAndLimitList(mappedExercises);

            return Ok(result);
        }

        [HttpGet("basic/function/q")]
        public IActionResult GetQ()
        {
            var mappedExercises = context.Exercises.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.EncodedImage,
            });
            var result = ApplyOffsetAndLimitQ(mappedExercises);

            return Ok(result);
        }

        protected virtual IEnumerable<T> ApplyOffsetAndLimit<T>(IQueryable<T> queryable, int? offset = 0, int? limit = -1)
        {
            queryable = queryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                queryable = queryable.Take(limit ?? 0);

            return [.. queryable];
        }

        protected virtual List<T> ApplyOffsetAndLimitList<T>(IQueryable<T> queryable, int? offset = 0, int? limit = -1)
        {
            queryable = queryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                queryable = queryable.Take(limit ?? 0);

            return [.. queryable];
        }

        protected virtual IQueryable<T> ApplyOffsetAndLimitQ<T>(IQueryable<T> queryable, int? offset = 0, int? limit = -1)
        {
            queryable = queryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                queryable = queryable.Take(limit ?? 0);

            return queryable;
        }

        [HttpGet("readservice")]
        public async Task<IActionResult> GetRS([FromQuery] string include)
        {
            var result = await readService.Get((string?)null, 0, -1, include);
            return Ok(result.Select(mapper.Map));
        }
    }
}
