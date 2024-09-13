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

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService(ICreateService<Exercise> createService,
                                         IReadSingleService<Exercise> readSingleService,
                                         IReadRangeService<Exercise> readRangeService,
                                         IReadSingleService<FavoriteExercise> favoriteReadSingleService,
                                         ICountService<FavoriteExercise> favoriteCountService,
                                         IUpdateService<Exercise> updateService,
                                         ICreateService<FavoriteExercise> favoriteExerciseCreateService,
                                         ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService,
                                         ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService,
                                         ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService,
                                         ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService,
                                         ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService,
                                         IDeleteService<Exercise> deleteService,
                                         IDeleteService<FavoriteExercise> favoriteExerciseDeleteService,
                                         IDeleteService<EquipmentUsage> equipmetUsageDeleteService,
                                         IDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteService,
                                         IDeleteService<PrimaryMuscleInExercise> primaryMuscleDeleteService,
                                         IDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteService,
                                         IDeleteService<SecondaryMuscleInExercise> secondaryMuscleDeleteService,
                                         IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper,
                                         IResponseMapper<Exercise, SimpleExerciseResponseDTO> simpleResponseMapper,
                                         IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper
        ) : IExerciseService
    {
        private readonly ICreateService<Exercise> createService = createService;
        private readonly IReadSingleService<Exercise> readSingleService = readSingleService;
        private readonly IReadRangeService<Exercise> readRangeService = readRangeService;
        private readonly IReadSingleService<FavoriteExercise> favoriteReadSingleService = favoriteReadSingleService;
        private readonly ICountService<FavoriteExercise> favoriteCountService = favoriteCountService;
        private readonly IUpdateService<Exercise> updateService = updateService;
        private readonly ICreateService<FavoriteExercise> favoriteExerciseCreateService = favoriteExerciseCreateService;
        private readonly ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService = equipmetUsageCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService = primaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService = primaryMuscleCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService = secondaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService = secondaryMuscleCreateRangeService;
        private readonly IDeleteService<Exercise> deleteService = deleteService;
        private readonly IDeleteService<FavoriteExercise> favoriteExerciseDeleteService = favoriteExerciseDeleteService;
        private readonly IDeleteService<EquipmentUsage> equipmetUsageDeleteService = equipmetUsageDeleteService;
        private readonly IDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteService = primaryMuscleGroupDeleteService;
        private readonly IDeleteService<PrimaryMuscleInExercise> primaryMuscleDeleteService = primaryMuscleDeleteService;
        private readonly IDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteService = secondaryMuscleGroupDeleteService;
        private readonly IDeleteService<SecondaryMuscleInExercise> secondaryMuscleDeleteService = secondaryMuscleDeleteService;
        private readonly IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper = createRequestMapper;
        private readonly IResponseMapper<Exercise, SimpleExerciseResponseDTO> simpleResponseMapper = simpleResponseMapper;
        private readonly IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper = detailedResponseMapper;
    }
}
