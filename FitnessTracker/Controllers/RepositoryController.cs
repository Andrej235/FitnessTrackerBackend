using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryController<TEntity, TDTO>(ICreateService<TEntity> createService,
                                                     IUpdateService<TEntity> updateService,
                                                     IDeleteService<TEntity> deleteService,
                                                     IReadService<TEntity> readService,
                                                     IEntityMapper<TEntity, TDTO> mapper)
        : ControllerBase
        where TEntity : class
        where TDTO : class
    {
        public ICreateService<TEntity> CreateService { get; } = createService;
        public IUpdateService<TEntity> UpdateService { get; } = updateService;
        public IDeleteService<TEntity> DeleteService { get; } = deleteService;
        public IReadService<TEntity> ReadService { get; } = readService;
        public IEntityMapper<TEntity, TDTO> Mapper { get; } = mapper;

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TDTO entityDTO)
        {
            var newEntityId = await CreateService.Add(Mapper.Map(entityDTO));
            return Ok(newEntityId);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(string id, [FromQuery] string? include)
        {
            var exercise = await ReadService.Get(id, include);
            if (exercise is null)
                return NotFound($"Entity with id {id} was not found.");

            return Ok(Mapper.Map(exercise));
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var entities = (await ReadService.Get(q, offset, limit, include)).Select(Mapper.Map);
            return Ok(entities);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update([FromBody] TDTO updatedEntity)
        {
            try
            {
                await UpdateService.Update(Mapper.Map(updatedEntity));
                return Ok("Successfully updated entity");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [HttpDelete("{primaryKey}")]
        public virtual async Task<IActionResult> Delete(string primaryKey)
        {
            try
            {
                await DeleteService.Delete(primaryKey);
                return Ok("Successfully deleted entity.");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }
    }
}
