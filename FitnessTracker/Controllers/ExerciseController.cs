using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using System.Collections.Generic;

namespace FitnessTracker.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController(ICreateService<Exercise> createService,
                                    IReadService<Exercise> readService,
                                    IUpdateService<Exercise> updateService,
                                    IDeleteService<Exercise> deleteService,
                                    IEntityMapperAsync<Exercise, ExerciseDTO> mapper) : ControllerBase
    {
        public IReadService<Exercise> readService = readService;
        public IDeleteService<Exercise> deleteService = deleteService;
        public ICreateService<Exercise> createService = createService;
        public IUpdateService<Exercise> updateService = updateService;
        public IEntityMapperAsync<Exercise, ExerciseDTO> mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var exercises = await readService.Get(q, offset, limit, include);

            return Ok(exercises.Select(mapper.Map).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery] string? include)
        {
            if (!int.TryParse(id, out var parsedId))
                return NotFound($"Entity with id {id} was not found.");

            var exercise = await readService.Get(x => x.Id == parsedId, include);
            if (exercise is null)
                return NotFound($"Entity with id {id} was not found.");

            return Ok(mapper.Map(exercise));
        }

        [HttpDelete("{primaryKey}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(string primaryKey)
        {
            try
            {
                await deleteService.Delete(primaryKey);
                return Ok("Successfully deleted entity.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Create([FromBody] ExerciseDTO entityDTO)
        {
            var entity = await mapper.MapAsync(entityDTO);
            var newId = await createService.Add(entity);
            return newId != default ? Ok(newId) : BadRequest("Entity already exists");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ExerciseDTO updatedEntity)
        {
            try
            {
                await updateService.Update(await mapper.MapAsync(updatedEntity));
                return Ok("Successfully updated entity");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}