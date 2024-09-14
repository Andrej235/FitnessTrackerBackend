using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.MuscleGroupService
{
    public partial class MuscleGroupService(ICreateService<MuscleGroup> createService,
                                            ICreateRangeService<MuscleGroup> createRangeService,
                                            IReadRangeService<MuscleGroup> readRangeService,
                                            IDeleteService<MuscleGroup> deleteService,
                                            IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup> requestMapper,
                                            IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO> responseMapper,
                                            IResponseMapper<MuscleGroup, DetailedMuscleGroupResponseDTO> detailedResponseMapper) : IMuscleGroupService
    {
        private readonly ICreateService<MuscleGroup> createService = createService;
        private readonly ICreateRangeService<MuscleGroup> createRangeService = createRangeService;
        private readonly IReadRangeService<MuscleGroup> readRangeService = readRangeService;
        private readonly IDeleteService<MuscleGroup> deleteService = deleteService;
        private readonly IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup> requestMapper = requestMapper;
        private readonly IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO> responseMapper = responseMapper;
        private readonly IResponseMapper<MuscleGroup, DetailedMuscleGroupResponseDTO> detailedResponseMapper = detailedResponseMapper;
    }
}
