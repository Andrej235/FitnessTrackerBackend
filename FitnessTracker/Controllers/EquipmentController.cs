using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.Full;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/equipment")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class EquipmentController(ICreateService<Equipment> createService,
                                             ICreateRangeService<Equipment> createRangeService,
                                             IFullReadRangeService<Equipment> readRangeService,
                                             IDeleteService<Equipment> deleteService,
                                             IRequestMapper<CreateEquipmentRequestDTO, Equipment> requestMapper,
                                             IResponseMapper<Equipment, SimpleEquipmentResponseDTO> responseMapper) : ControllerBase
    {
        private readonly ICreateService<Equipment> createService = createService;
        private readonly ICreateRangeService<Equipment> createRangeService = createRangeService;
        private readonly IFullReadRangeService<Equipment> readRangeService = readRangeService;
        private readonly IDeleteService<Equipment> deleteService = deleteService;
        private readonly IRequestMapper<CreateEquipmentRequestDTO, Equipment> requestMapper = requestMapper;
        private readonly IResponseMapper<Equipment, SimpleEquipmentResponseDTO> responseMapper = responseMapper;
    }
}
