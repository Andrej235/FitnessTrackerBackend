using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Models;
using FitnessTracker.Services.Count;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class ExerciseController(ICreateService<Exercise> createService,
                                            IReadSingleService<Exercise> readSingleService,
                                            IReadRangeService<Exercise> readRangeService,
                                            IReadSingleService<FavoriteExercise> favoriteReadSingleService,
                                            ICountService<FavoriteExercise> favoriteCountService,
                                            IUpdateService<Exercise> updateService,
                                            IExecuteDeleteService<Exercise> deleteService,
                                            ICreateService<FavoriteExercise> favoriteExerciseCreateService,
                                            ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService,
                                            ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService,
                                            ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService,
                                            ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService,
                                            ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService,
                                            IExecuteDeleteService<FavoriteExercise> favoriteExerciseDeleteService,
                                            IExecuteDeleteService<EquipmentUsage> equipmetUsageExecuteDeleteService,
                                            IExecuteDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupExecuteDeleteService,
                                            IExecuteDeleteService<PrimaryMuscleInExercise> primaryMuscleExecuteDeleteService,
                                            IExecuteDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupExecuteDeleteService,
                                            IExecuteDeleteService<SecondaryMuscleInExercise> secondaryMuscleExecuteDeleteService,
                                            IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper,
                                            IResponseMapper<Exercise, SimpleExerciseResponseDTO> simpleResponseMapper,
                                            IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Exercise> createService = createService;
        private readonly IReadSingleService<Exercise> readSingleService = readSingleService;
        private readonly IReadRangeService<Exercise> readRangeService = readRangeService;
        private readonly IReadSingleService<FavoriteExercise> favoriteReadSingleService = favoriteReadSingleService;
        private readonly ICountService<FavoriteExercise> favoriteCountService = favoriteCountService;
        private readonly IUpdateService<Exercise> updateService = updateService;
        private readonly IExecuteDeleteService<Exercise> deleteService = deleteService;
        private readonly ICreateService<FavoriteExercise> favoriteExerciseCreateService = favoriteExerciseCreateService;
        private readonly IExecuteDeleteService<FavoriteExercise> favoriteExerciseDeleteService = favoriteExerciseDeleteService;
        private readonly IExecuteDeleteService<EquipmentUsage> equipmetUsageExecuteDeleteService = equipmetUsageExecuteDeleteService;
        private readonly IExecuteDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupExecuteDeleteService = primaryMuscleGroupExecuteDeleteService;
        private readonly IExecuteDeleteService<PrimaryMuscleInExercise> primaryMuscleExecuteDeleteService = primaryMuscleExecuteDeleteService;
        private readonly IExecuteDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupExecuteDeleteService = secondaryMuscleGroupExecuteDeleteService;
        private readonly IExecuteDeleteService<SecondaryMuscleInExercise> secondaryMuscleExecuteDeleteService = secondaryMuscleExecuteDeleteService;
        private readonly ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService = equipmetUsageCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService = primaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService = primaryMuscleCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService = secondaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService = secondaryMuscleCreateRangeService;
        private readonly IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper = createRequestMapper;
        private readonly IResponseMapper<Exercise, SimpleExerciseResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper = detailedResponseMapper;
    }
}