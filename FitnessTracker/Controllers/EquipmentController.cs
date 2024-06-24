using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/equipment")]
    public class EquipmentController(ICreateService<Equipment> createService,
                                     ICreateRangeService<Equipment> createRangeService,
                                     IReadRangeService<Equipment> readRangeService,
                                     IRequestMapper<CreateEquipmentRequestDTO, Equipment> requestMapper,
                                     IResponseMapper<Equipment, SimpleEquipmentResponseDTO> responseMapper) : ControllerBase
    {
        private readonly ICreateService<Equipment> createService = createService;
        private readonly ICreateRangeService<Equipment> createRangeService = createRangeService;
        private readonly IReadRangeService<Equipment> readRangeService = readRangeService;
        private readonly IRequestMapper<CreateEquipmentRequestDTO, Equipment> requestMapper = requestMapper;
        private readonly IResponseMapper<Equipment, SimpleEquipmentResponseDTO> responseMapper = responseMapper;

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEquipmentRequestDTO request)
        {
            await createService.Add(requestMapper.Map(request));
            return Ok();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateEquipmentRequestDTO> request)
        {
            await createRangeService.Add(request.Select(requestMapper.Map));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            var muscleGroups = await readRangeService.Get(x => true, offset, limit, include);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }
    }
}
