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
                                            IDeleteService<Exercise> deleteService,
                                            ICreateService<FavoriteExercise> favoriteExerciseCreateService,
                                            ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService,
                                            ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService,
                                            ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService,
                                            ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService,
                                            ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService,
                                            IDeleteService<FavoriteExercise> favoriteExerciseDeleteService,
                                            IDeleteService<EquipmentUsage> equipmetUsageExecuteDeleteService,
                                            IDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupExecuteDeleteService,
                                            IDeleteService<PrimaryMuscleInExercise> primaryMuscleExecuteDeleteService,
                                            IDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupExecuteDeleteService,
                                            IDeleteService<SecondaryMuscleInExercise> secondaryMuscleExecuteDeleteService,
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
        private readonly IDeleteService<Exercise> deleteService = deleteService;
        private readonly ICreateService<FavoriteExercise> favoriteExerciseCreateService = favoriteExerciseCreateService;
        private readonly IDeleteService<FavoriteExercise> favoriteExerciseDeleteService = favoriteExerciseDeleteService;
        private readonly IDeleteService<EquipmentUsage> equipmetUsageExecuteDeleteService = equipmetUsageExecuteDeleteService;
        private readonly IDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupExecuteDeleteService = primaryMuscleGroupExecuteDeleteService;
        private readonly IDeleteService<PrimaryMuscleInExercise> primaryMuscleExecuteDeleteService = primaryMuscleExecuteDeleteService;
        private readonly IDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupExecuteDeleteService = secondaryMuscleGroupExecuteDeleteService;
        private readonly IDeleteService<SecondaryMuscleInExercise> secondaryMuscleExecuteDeleteService = secondaryMuscleExecuteDeleteService;
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