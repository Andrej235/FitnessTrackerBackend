using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.Data;
using ProjectGym.Services.DatabaseSerialization;
using ProjectGym.Utilities;
using System.Text;

namespace FitnessTracker.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    [Route("api/database")]
    public class DatabaseController(ExerciseContext context) : ControllerBase
    {
        struct OldNewIdPairs(object oldId, object newId)
        {
            public object oldId = oldId;
            public object newId = newId;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(DatabaseSerializationService.Serialize(context));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [HttpPut]
        public async Task<IActionResult> Load()
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                StreamReader reader = new(Request.Body, Encoding.UTF8);
                await DatabaseDeserializationService.LoadDatabase(context, await reader.ReadToEndAsync());

                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDB()
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDB()
        {
            try
            {
                await context.Database.EnsureDeletedAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        public class StringDTO
        {
            public string Value { get; set; } = "";
        }
    }
}