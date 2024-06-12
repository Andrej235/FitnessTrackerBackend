using Microsoft.AspNetCore.Mvc;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryController<TEntity, TDTO>(ICreateService<TEntity> createService,
                                                     IUpdateService<TEntity> updateService,
                                                     IDeleteService<TEntity> deleteService,
                                                     IReadService<TEntity> readService,
                                                     IEntityMapper<TEntity, TDTO> mapper)
        : ControllerBase,
        ICreateController<TEntity, TDTO>,
        IReadController<TEntity, TDTO>,
        IUpdateController<TEntity, TDTO>,
        IDeleteController<TEntity>
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
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
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
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }
    }
}
