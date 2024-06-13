using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Controllers.Admin
{
    [Authorize(Roles = Role.Admin)]
    [Route("api/unsafe/narrow")]
    [ApiController]
    public class UNSAFE_NarrowController(DataContext context, IReadService<Exercise> readService, IEntityMapper<Exercise, ExerciseDTO> mapper, IEntityMapper<Exercise, object> fullMapper) : ControllerBase
    {
        [HttpGet("fullexercise/singlequery")]
        public IActionResult GetFullExercise_SingleQuery([FromQuery] string include)
        {
            var result = context.Exercises.AsSingleQuery().Select(x => new
            {
                id = include.Contains("id") ? (int?)x.Id : null,
                name = include.Contains("name") ? x.Name : null,
                description = include.Contains("description") ? x.Description : null,
                image = include.Contains("image") ? x.Image : null,
                equipment = include.Contains("equipment") ? x.Equipment.Select(x => x) : null,
                primaryMuscleGroups = include.Contains("primarymusclegroups") ? x.PrimaryMuscleGroups.Select(x => x) : null,
                secondaryMuscleGroups = include.Contains("secondarymusclegroups") ? x.SecondaryMuscleGroups.Select(x => x) : null,
                primaryMuscles = include.Contains("primarymuscles") ? x.PrimaryMuscles.Select(x => x) : null,
                secondaryMuscles = include.Contains("secondarymuscles") ? x.SecondaryMuscles.Select(x => x) : null,
            });

            var a = ApplyOffsetAndLimit(result);
            var b = a.Select(x => fullMapper.Map(new Exercise()
            {
                Id = x.id ?? 0,
                Name = x.name ?? "",
                Description = x.description ?? "",
                Image = x.image ?? "",
                Equipment = x.equipment ?? [],
                PrimaryMuscleGroups = x.primaryMuscleGroups ?? [],
                SecondaryMuscleGroups = x.secondaryMuscleGroups ?? [],
                PrimaryMuscles = x.primaryMuscles ?? [],
                SecondaryMuscles = x.secondaryMuscles ?? [],
            }));

            return Ok(b);
        }

        [HttpGet("fullexercise/splitquery")]
        public IActionResult GetFullExercise_SplitQuery([FromQuery] string include)
        {
            var result = context.Exercises.AsSplitQuery().Select(x => new
            {
                id = include.Contains("id") ? (int?)x.Id : null,
                name = include.Contains("name") ? x.Name : null,
                description = include.Contains("description") ? x.Description : null,
                image = include.Contains("image") ? x.Image : null,
                equipment = include.Contains("equipment") ? x.Equipment.Select(x => x) : null,
                primaryMuscleGroups = include.Contains("primarymusclegroups") ? x.PrimaryMuscleGroups.Select(x => x) : null,
                secondaryMuscleGroups = include.Contains("secondarymusclegroups") ? x.SecondaryMuscleGroups.Select(x => x) : null,
                primaryMuscles = include.Contains("primarymuscles") ? x.PrimaryMuscles.Select(x => x) : null,
                secondaryMuscles = include.Contains("secondarymuscles") ? x.SecondaryMuscles.Select(x => x) : null,
            });

            var a = ApplyOffsetAndLimit(result);
            var b = a.Select(x => fullMapper.Map(new Exercise()
            {
                Id = x.id ?? 0,
                Name = x.name ?? "",
                Description = x.description ?? "",
                Image = x.image ?? "",
                Equipment = x.equipment ?? [],
                PrimaryMuscleGroups = x.primaryMuscleGroups ?? [],
                SecondaryMuscleGroups = x.secondaryMuscleGroups ?? [],
                PrimaryMuscles = x.primaryMuscles ?? [],
                SecondaryMuscles = x.secondaryMuscles ?? [],
            }));

            return Ok(b);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string include)
        {
            var result = context.Exercises.Select(x => new
            {
                id = include.Contains("id") ? (int?)x.Id : null,
                name = include.Contains("name") ? x.Name : null,
                description = include.Contains("description") ? x.Description : null,
                image = include.Contains("image") ? x.Image : null,
                equipment = include.Contains("equipment") ? x.Equipment.Select(x => x.Id) : null,
                primaryMuscleGroups = include.Contains("primarymusclegroups") ? x.PrimaryMuscleGroups.Select(x => x.Id) : null,
                secondaryMuscleGroups = include.Contains("secondarymusclegroups") ? x.SecondaryMuscleGroups.Select(x => x.Id) : null,
                primaryMuscles = include.Contains("primarymuscles") ? x.PrimaryMuscles.Select(x => x.Id) : null,
                secondaryMuscles = include.Contains("secondarymuscles") ? x.SecondaryMuscles.Select(x => x.Id) : null,
            });

            return Ok(ApplyOffsetAndLimit(result));
        }

        [HttpGet("manual")]
        public IActionResult GetManual([FromQuery] string include)
        {
            var result = context.Exercises.Select(x => new
            {
                id = include.Contains("id") ? (int?)x.Id : null,
                name = include.Contains("name") ? x.Name : null,
                description = include.Contains("description") ? x.Description : null,
                image = include.Contains("image") ? x.Image : null,
                equipment = include.Contains("equipment") ? x.Equipment.Select(x => x) : null,
                primaryMuscleGroups = include.Contains("primarymusclegroups") ? x.PrimaryMuscleGroups.Select(x => x) : null,
                secondaryMuscleGroups = include.Contains("secondarymusclegroups") ? x.SecondaryMuscleGroups.Select(x => x) : null,
                primaryMuscles = include.Contains("primarymuscles") ? x.PrimaryMuscles.Select(x => x) : null,
                secondaryMuscles = include.Contains("secondarymuscles") ? x.SecondaryMuscles.Select(x => x) : null,
            });

            var a = ApplyOffsetAndLimit(result);
            var b = a.Select(x => mapper.Map(new Exercise()
            {
                Id = x.id ?? 0,
                Name = x.name ?? "",
                Description = x.description ?? "",
                Image = x.image ?? "",
                Equipment = x.equipment ?? [],
                PrimaryMuscleGroups = x.primaryMuscleGroups ?? [],
                SecondaryMuscleGroups = x.secondaryMuscleGroups ?? [],
                PrimaryMuscles = x.primaryMuscles ?? [],
                SecondaryMuscles = x.secondaryMuscles ?? [],
            }));

            return Ok(b);
        }

        [HttpGet("basic/inline")]
        public IActionResult Get()
        {
            var mappedExercises = context.Exercises.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Image,
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
                x.Image,
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
                x.Image,
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
                x.Image,
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
