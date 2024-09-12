using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/equipment")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class EquipmentController(ICreateService<Equipment> createService,
                                             ICreateRangeService<Equipment> createRangeService,
                                             IReadRangeService<Equipment> readRangeService,
                                             IExecuteDeleteService<Equipment> deleteService,
                                             IRequestMapper<CreateEquipmentRequestDTO, Equipment> requestMapper,
                                             IResponseMapper<Equipment, SimpleEquipmentResponseDTO> responseMapper) : ControllerBase
    {
        private readonly ICreateService<Equipment> createService = createService;
        private readonly ICreateRangeService<Equipment> createRangeService = createRangeService;
        private readonly IReadRangeService<Equipment> readRangeService = readRangeService;
        private readonly IExecuteDeleteService<Equipment> deleteService = deleteService;
        private readonly IRequestMapper<CreateEquipmentRequestDTO, Equipment> requestMapper = requestMapper;
        private readonly IResponseMapper<Equipment, SimpleEquipmentResponseDTO> responseMapper = responseMapper;
    }
}
