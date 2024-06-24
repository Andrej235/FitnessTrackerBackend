using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Update;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Services.Read.QueryBased;
using FitnessTracker.Services.Read.ExpressionBased;

namespace FitnessTracker.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public partial class ExerciseController(ICreateService<Exercise> createService,
                                            IReadSingleService<Exercise> readSingleService,
                                            IReadQueryService<Exercise> readQueryService,
                                            IUpdateService<Exercise> updateService,
                                            IDeleteService<Exercise> deleteService,
                                            ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService,
                                            ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService,
                                            ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService,
                                            ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService,
                                            ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService,
                                            IDeleteRangeService<EquipmentUsage> equipmetUsageDeleteRangeService,
                                            IDeleteRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteRangeService,
                                            IDeleteRangeService<PrimaryMuscleInExercise> primaryMuscleDeleteRangeService,
                                            IDeleteRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteRangeService,
                                            IDeleteRangeService<SecondaryMuscleInExercise> secondaryMuscleDeleteRangeService,
                                            IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper,
                                            IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Exercise> createService = createService;
        private readonly IReadSingleService<Exercise> readSingleService = readSingleService;
        private readonly IReadQueryService<Exercise> readQueryService = readQueryService;
        private readonly IUpdateService<Exercise> updateService = updateService;
        private readonly IDeleteService<Exercise> deleteService = deleteService;
        private readonly ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService = equipmetUsageCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService = primaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService = primaryMuscleCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService = secondaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService = secondaryMuscleCreateRangeService;
        private readonly IDeleteRangeService<EquipmentUsage> equipmetUsageDeleteRangeService = equipmetUsageDeleteRangeService;
        private readonly IDeleteRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteRangeService = primaryMuscleGroupDeleteRangeService;
        private readonly IDeleteRangeService<PrimaryMuscleInExercise> primaryMuscleDeleteRangeService = primaryMuscleDeleteRangeService;
        private readonly IDeleteRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteRangeService = secondaryMuscleGroupDeleteRangeService;
        private readonly IDeleteRangeService<SecondaryMuscleInExercise> secondaryMuscleDeleteRangeService = secondaryMuscleDeleteRangeService;
        private readonly IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper = createRequestMapper;
        private readonly IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper = detailedResponseMapper;
    }
}