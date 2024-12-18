using AspNetCoreRateLimit;
using FitnessTracker.Auth;
using FitnessTracker.Data;
using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.AuthTokens;
using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Emails;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.EmailSender;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Request.CompletedMappers;
using FitnessTracker.Services.Mapping.Request.EquipmentMappers;
using FitnessTracker.Services.Mapping.Request.ExerciseMappers;
using FitnessTracker.Services.Mapping.Request.MuscleGroupMappers;
using FitnessTracker.Services.Mapping.Request.MuscleMappers;
using FitnessTracker.Services.Mapping.Request.SetMappers;
using FitnessTracker.Services.Mapping.Request.SplitMappers;
using FitnessTracker.Services.Mapping.Request.UserMappers;
using FitnessTracker.Services.Mapping.Request.WorkoutMappers;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Mapping.Response.AuthTokens;
using FitnessTracker.Services.Mapping.Response.CompletedWorkoutMappers;
using FitnessTracker.Services.Mapping.Response.EquipmentMappers;
using FitnessTracker.Services.Mapping.Response.ExerciseMappers;
using FitnessTracker.Services.Mapping.Response.MuscleGroupMappers;
using FitnessTracker.Services.Mapping.Response.MuscleMappers;
using FitnessTracker.Services.Mapping.Response.SetMappers;
using FitnessTracker.Services.Mapping.Response.SplitMappers;
using FitnessTracker.Services.Mapping.Response.UserMappers;
using FitnessTracker.Services.Mapping.Response.WorkoutMappers;
using FitnessTracker.Services.ModelServices.EquipmentService;
using FitnessTracker.Services.ModelServices.ExerciseService;
using FitnessTracker.Services.ModelServices.MuscleGroupService;
using FitnessTracker.Services.ModelServices.MuscleService;
using FitnessTracker.Services.ModelServices.SplitService;
using FitnessTracker.Services.ModelServices.UserService;
using FitnessTracker.Services.ModelServices.UserService.EmailConfirmationSenderService;
using FitnessTracker.Services.ModelServices.UserService.EmailConfirmationService;
using FitnessTracker.Services.ModelServices.UserService.ResetPasswordSenderService;
using FitnessTracker.Services.ModelServices.UserService.ResetPasswordService;
using FitnessTracker.Services.ModelServices.WorkoutService;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FitnessTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;
            _ = builder.Services.AddSingleton(configuration);
            _ = builder.Services.AddControllers();
            _ = builder.Services.AddDbContext<DataContext>(x =>
            {
                _ = x.UseSqlServer(configuration.GetConnectionString("SQLConnectionString"));
                _ = x.EnableSensitiveDataLogging(); //TODO-PROD: remove in production
            });
            _ = builder.Services.AddSwaggerGen(options => options.SupportNonNullableReferenceTypes());
            _ = builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();
            _ = builder.Services.AddProblemDetails();

            #region Rate limiting
            _ = builder.Services.AddMemoryCache();
            _ = builder.Services.Configure<ClientRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-Ip";
                options.ClientIdHeader = "X-ClientId";
                options.GeneralRules = [
                        new() {
                            Endpoint = "*",
                            Limit = 10,
                            Period = "10s"
                        },
                        new() {
                            Endpoint = "*/login",
                            Limit = 10,
                            Period = "30s"
                        },
                        new() {
                            Endpoint = "*/register",
                            Limit = 5,
                            Period = "30s"
                        },
                        new() {
                            Endpoint = "*/forgotpassword",
                            Limit = 1,
                            Period = "30s"
                        },
                        new() {
                            Endpoint = "*/forgotpassword/*",
                            Limit = 7,
                            Period = "7d"
                        },
                        new() {
                            Endpoint = "*/chagepassword",
                            Limit = 5,
                            Period = "3d"
                        },
                        new() {
                            Endpoint = "*/confirm",
                            Limit = 1,
                            Period = "15s"
                        },
                        new() {
                            Endpoint = "*/resendconfirmationemail",
                            Limit = 1,
                            Period = "1m"
                        }
                    ];
            });

            _ = builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            _ = builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            _ = builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            _ = builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            _ = builder.Services.AddInMemoryRateLimiting();
            #endregion

            #region JWT / Auth
            _ = builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                ClockSkew = TimeSpan.Zero,
            })
            .AddScheme<AuthenticationSchemeOptions, AllowExpiredAuthenticationHandler>("AllowExpired", (p) => { });

            _ = builder.Services.AddScoped<ITokenManager, TokenManager>();
            _ = builder.Services.AddAuthorization();
            #endregion

            #region Cors
            _ = builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.SetIsOriginAllowed(origin => new Uri(origin).Host is "localhost" or "192.168.1.100")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));
            #endregion

            #region Emails
            EmailConfiguration? emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            if (emailConfig is not null)
                _ = builder.Services.AddSingleton(emailConfig);

            _ = builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
            _ = builder.Services.AddScoped<SmtpClient>();
            #endregion

            #region Email verification tokens
            _ = builder.Services.AddScoped<ICreateService<EmailConfirmation>, CreateService<EmailConfirmation>>();
            _ = builder.Services.AddScoped<IReadSingleService<EmailConfirmation>, ReadService<EmailConfirmation>>();
            _ = builder.Services.AddScoped<IReadRangeService<EmailConfirmation>, ReadService<EmailConfirmation>>();
            _ = builder.Services.AddScoped<IDeleteService<EmailConfirmation>, DeleteService<EmailConfirmation>>();
            _ = builder.Services.AddScoped<IDeleteService<EmailConfirmation>, DeleteService<EmailConfirmation>>();
            #endregion

            #region Exercise
            _ = builder.Services.AddScoped<IExerciseService, ExerciseService>();
            _ = builder.Services.AddScoped<ICreateService<Exercise>, CreateService<Exercise>>();
            _ = builder.Services.AddScoped<IReadSingleService<Exercise>, ReadService<Exercise>>();
            _ = builder.Services.AddScoped<IReadSingleSelectedService<Exercise>, ReadService<Exercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<Exercise>, ReadService<Exercise>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<Exercise>, UpdateService<Exercise>>();
            _ = builder.Services.AddScoped<IDeleteService<Exercise>, DeleteService<Exercise>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateExerciseRequestDTO, Exercise>, CreateExerciseRequestMapper>();
            _ = builder.Services.AddScoped<IRequestMapper<UpdateExerciseRequestDTO, Exercise>, UpdateExerciseRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Exercise, SimpleExerciseResponseDTO>, SimpleExerciseResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Exercise, DetailedExerciseResponseDTO>, DetailedExerciseResponseMapper>();
            #endregion

            #region Favorite exercise
            _ = builder.Services.AddScoped<ICreateService<FavoriteExercise>, CreateService<FavoriteExercise>>();
            _ = builder.Services.AddScoped<IReadSingleService<FavoriteExercise>, ReadService<FavoriteExercise>>();
            _ = builder.Services.AddScoped<IDeleteService<FavoriteExercise>, DeleteService<FavoriteExercise>>();
            #endregion

            #region User 
            _ = builder.Services.AddScoped<IUserService, UserService>();
            _ = builder.Services.AddScoped<ICreateService<User>, CreateService<User>>();
            _ = builder.Services.AddScoped<IReadSingleService<User>, ReadService<User>>();
            _ = builder.Services.AddScoped<IReadSingleSelectedService<User>, ReadService<User>>();
            _ = builder.Services.AddScoped<IReadRangeService<User>, ReadService<User>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<User>, UpdateService<User>>();
            _ = builder.Services.AddScoped<IExecuteUpdateService<User>, UpdateService<User>>();
            _ = builder.Services.AddScoped<IRequestMapper<RegisterUserRequestDTO, User>, RegisterUserRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<User, SimpleUserResponseDTO>, SimpleUserResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<User, DetailedUserResponseDTO>, DetailedPublicUserResponseMapper>();
            _ = builder.Services.AddScoped<IEmailConfirmationSenderService, EmailConfirmationSenderService>();
            _ = builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
            _ = builder.Services.AddScoped<IResetPasswordEmailSenderService, ResetPasswordEmailSender>();
            _ = builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
            #endregion

            #region User settings
            _ = builder.Services.AddScoped<ICreateService<UserSettings>, CreateService<UserSettings>>();
            _ = builder.Services.AddScoped<IReadSingleService<UserSettings>, ReadService<UserSettings>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<UserSettings>, UpdateService<UserSettings>>();
            _ = builder.Services.AddScoped<IResponseMapper<UserSettings, UserSettingsResponseDTO>, UserSettingsResponseMapper>();
            #endregion

            #region User follows
            _ = builder.Services.AddScoped<ICreateService<UserFollows>, CreateService<UserFollows>>();
            _ = builder.Services.AddScoped<IReadSingleService<UserFollows>, ReadService<UserFollows>>();
            _ = builder.Services.AddScoped<IReadRangeService<UserFollows>, ReadService<UserFollows>>();
            _ = builder.Services.AddScoped<IDeleteService<UserFollows>, DeleteService<UserFollows>>();
            #endregion

            #region Set
            _ = builder.Services.AddScoped<ICreateService<Set>, CreateService<Set>>();
            _ = builder.Services.AddScoped<ICreateRangeService<Set>, CreateService<Set>>();
            _ = builder.Services.AddScoped<IReadSingleService<Set>, ReadService<Set>>();
            _ = builder.Services.AddScoped<IReadSingleSelectedService<Set>, ReadService<Set>>();
            _ = builder.Services.AddScoped<IReadRangeService<Set>, ReadService<Set>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<Set>, UpdateService<Set>>();
            _ = builder.Services.AddScoped<IDeleteService<Set>, DeleteService<Set>>();
            _ = builder.Services.AddScoped<IDeleteService<Set>, DeleteService<Set>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSetRequestDTO, Set>, CreateSetRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Set, DetailedSetResponseDTO>, DetailedSetResponseMapper>();
            #endregion

            #region Workouts
            _ = builder.Services.AddScoped<IWorkoutService, WorkoutService>();
            _ = builder.Services.AddScoped<IReadSingleService<Workout>, ReadService<Workout>>();
            _ = builder.Services.AddScoped<IReadSingleSelectedService<Workout>, ReadService<Workout>>();
            _ = builder.Services.AddScoped<IReadRangeService<Workout>, ReadService<Workout>>();
            _ = builder.Services.AddScoped<IReadRangeSelectedService<Workout>, ReadService<Workout>>();
            _ = builder.Services.AddScoped<ICreateService<Workout>, CreateService<Workout>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<Workout>, UpdateService<Workout>>();
            _ = builder.Services.AddScoped<IDeleteService<Workout>, DeleteService<Workout>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateWorkoutRequestDTO, Workout>, CreateWorkoutRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Workout, NewWorkoutResponseDTO>, NewWorkoutResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Workout, SimpleWorkoutResponseDTO>, SimpleWorkoutResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Workout, SimpleWorkoutOptionResponseDTO>, SimpleWorkoutOptionResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Workout, DetailedWorkoutResponseDTO>, DetailedWorkoutResponseMapper>();
            #endregion

            #region Workout like
            _ = builder.Services.AddScoped<ICreateService<WorkoutLike>, CreateService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutLike>, ReadService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IReadRangeService<WorkoutLike>, ReadService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutLike>, ReadService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IReadRangeService<WorkoutLike>, ReadService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutLike>, DeleteService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutLike>, DeleteService<WorkoutLike>>();
            #endregion

            #region Workout comment
            _ = builder.Services.AddScoped<ICreateService<WorkoutComment>, CreateService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutComment>, ReadService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IReadSingleSelectedService<WorkoutComment>, ReadService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IReadRangeService<WorkoutComment>, ReadService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IReadRangeSelectedService<WorkoutComment>, ReadService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutComment>, DeleteService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutComment>, DeleteService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment>, CreateWorkoutCommentRequestMapper>();
            #endregion

            #region Workout comment like
            _ = builder.Services.AddScoped<ICreateService<WorkoutCommentLike>, CreateService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutCommentLike>, ReadService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IReadRangeService<WorkoutCommentLike>, ReadService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutCommentLike>, DeleteService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutCommentLike>, DeleteService<WorkoutCommentLike>>();
            #endregion

            #region Workout favorite
            _ = builder.Services.AddScoped<ICreateService<FavoriteWorkout>, CreateService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IReadSingleService<FavoriteWorkout>, ReadService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IReadRangeService<FavoriteWorkout>, ReadService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IReadSingleService<FavoriteWorkout>, ReadService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IReadRangeService<FavoriteWorkout>, ReadService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IDeleteService<FavoriteWorkout>, DeleteService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IDeleteService<FavoriteWorkout>, DeleteService<FavoriteWorkout>>();
            #endregion

            #region Muscle group
            _ = builder.Services.AddScoped<IMuscleGroupService, MuscleGroupService>();
            _ = builder.Services.AddScoped<ICreateService<MuscleGroup>, CreateService<MuscleGroup>>();
            _ = builder.Services.AddScoped<ICreateRangeService<MuscleGroup>, CreateService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IReadSingleService<MuscleGroup>, ReadService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IReadRangeService<MuscleGroup>, ReadService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<MuscleGroup>, UpdateService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IDeleteService<MuscleGroup>, DeleteService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup>, CreateMuscleGroupRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO>, SimpleMuscleGroupResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<MuscleGroup, DetailedMuscleGroupResponseDTO>, DetailedMuscleGroupResponseMapper>();
            #endregion

            #region Muscle
            _ = builder.Services.AddScoped<IMuscleService, MuscleService>();
            _ = builder.Services.AddScoped<ICreateService<Muscle>, CreateService<Muscle>>();
            _ = builder.Services.AddScoped<ICreateRangeService<Muscle>, CreateService<Muscle>>();
            _ = builder.Services.AddScoped<IReadSingleService<Muscle>, ReadService<Muscle>>();
            _ = builder.Services.AddScoped<IReadRangeService<Muscle>, ReadService<Muscle>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<Muscle>, UpdateService<Muscle>>();
            _ = builder.Services.AddScoped<IDeleteService<Muscle>, DeleteService<Muscle>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateMuscleRequestDTO, Muscle>, CreateMuscleRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Muscle, SimpleMuscleResponseDTO>, SimpleMuscleResponseMapper>();
            #endregion

            #region Equipment exercise usage
            _ = builder.Services.AddScoped<ICreateService<EquipmentUsage>, CreateService<EquipmentUsage>>();
            _ = builder.Services.AddScoped<ICreateRangeService<EquipmentUsage>, CreateService<EquipmentUsage>>();
            _ = builder.Services.AddScoped<IReadRangeService<EquipmentUsage>, ReadService<EquipmentUsage>>();
            _ = builder.Services.AddScoped<IDeleteService<EquipmentUsage>, DeleteService<EquipmentUsage>>();
            #endregion

            #region Equipment
            _ = builder.Services.AddScoped<IEquipmentService, EquipmentService>();
            _ = builder.Services.AddScoped<ICreateService<Equipment>, CreateService<Equipment>>();
            _ = builder.Services.AddScoped<ICreateRangeService<Equipment>, CreateService<Equipment>>();
            _ = builder.Services.AddScoped<IReadSingleService<Equipment>, ReadService<Equipment>>();
            _ = builder.Services.AddScoped<IReadRangeService<Equipment>, ReadService<Equipment>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<Equipment>, UpdateService<Equipment>>();
            _ = builder.Services.AddScoped<IDeleteService<Equipment>, DeleteService<Equipment>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateEquipmentRequestDTO, Equipment>, CreateEquipmentRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Equipment, SimpleEquipmentResponseDTO>, SimpleEquipmentResponseMapper>();
            #endregion

            #region Primary muscle group
            _ = builder.Services.AddScoped<ICreateService<PrimaryMuscleGroupInExercise>, CreateService<PrimaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<ICreateRangeService<PrimaryMuscleGroupInExercise>, CreateService<PrimaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<PrimaryMuscleGroupInExercise>, ReadService<PrimaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IDeleteService<PrimaryMuscleGroupInExercise>, DeleteService<PrimaryMuscleGroupInExercise>>();
            #endregion

            #region Secondary muscle group
            _ = builder.Services.AddScoped<ICreateService<SecondaryMuscleGroupInExercise>, CreateService<SecondaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<ICreateRangeService<SecondaryMuscleGroupInExercise>, CreateService<SecondaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<SecondaryMuscleGroupInExercise>, ReadService<SecondaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IDeleteService<SecondaryMuscleGroupInExercise>, DeleteService<SecondaryMuscleGroupInExercise>>();
            #endregion

            #region Primary muscle
            _ = builder.Services.AddScoped<ICreateRangeService<PrimaryMuscleInExercise>, CreateService<PrimaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<PrimaryMuscleInExercise>, ReadService<PrimaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IDeleteService<PrimaryMuscleInExercise>, DeleteService<PrimaryMuscleInExercise>>();
            #endregion

            #region Secondary muscle
            _ = builder.Services.AddScoped<ICreateRangeService<SecondaryMuscleInExercise>, CreateService<SecondaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<SecondaryMuscleInExercise>, ReadService<SecondaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IDeleteService<SecondaryMuscleInExercise>, DeleteService<SecondaryMuscleInExercise>>();
            #endregion

            #region Auth tokens
            _ = builder.Services.AddScoped<ICreateService<RefreshToken>, CreateService<RefreshToken>>();
            _ = builder.Services.AddScoped<IReadSingleSelectedService<RefreshToken>, ReadService<RefreshToken>>();
            _ = builder.Services.AddScoped<IExecuteUpdateService<RefreshToken>, UpdateService<RefreshToken>>();
            _ = builder.Services.AddScoped<IDeleteService<RefreshToken>, DeleteService<RefreshToken>>();
            _ = builder.Services.AddScoped<IDeleteService<RefreshToken>, DeleteService<RefreshToken>>();

            _ = builder.Services.AddScoped<IResponseMapper<string, SimpleJWTResponseDTO>, SimpleJWTResponseMapper>();
            #endregion

            #region Split
            _ = builder.Services.AddScoped<ISplitService, SplitService>();
            _ = builder.Services.AddScoped<ICreateService<Split>, CreateService<Split>>();
            _ = builder.Services.AddScoped<IReadSingleService<Split>, ReadService<Split>>();
            _ = builder.Services.AddScoped<IReadSingleSelectedService<Split>, ReadService<Split>>();
            _ = builder.Services.AddScoped<IReadRangeService<Split>, ReadService<Split>>();
            _ = builder.Services.AddScoped<IReadRangeSelectedService<Split>, ReadService<Split>>();
            _ = builder.Services.AddScoped<IUpdateSingleService<Split>, UpdateService<Split>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSplitRequestDTO, Split>, CreateSplitRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Split, SimpleSplitResponseDTO>, SimpleSplitResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Split, DetailedSplitResponseDTO>, DetailedSplitResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Split, DetailedUserSplitResponseDTO>, DetailedUserSplitResponseMapper>();
            #endregion

            #region Split workout
            _ = builder.Services.AddScoped<IUpdateSingleService<SplitWorkout>, UpdateService<SplitWorkout>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSplitWorkoutRequestDTO, SplitWorkout>, CreateSplitWorkoutRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO>, SimpleSplitWorkoutResponseMapper>();
            #endregion

            #region Split like
            _ = builder.Services.AddScoped<ICreateService<SplitLike>, CreateService<SplitLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<SplitLike>, ReadService<SplitLike>>();
            _ = builder.Services.AddScoped<IReadRangeService<SplitLike>, ReadService<SplitLike>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitLike>, DeleteService<SplitLike>>();
            #endregion

            #region Split comment
            _ = builder.Services.AddScoped<ICreateService<SplitComment>, CreateService<SplitComment>>();
            _ = builder.Services.AddScoped<IReadSingleService<SplitComment>, ReadService<SplitComment>>();
            _ = builder.Services.AddScoped<IReadRangeSelectedService<SplitComment>, ReadService<SplitComment>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitComment>, DeleteService<SplitComment>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitComment>, DeleteService<SplitComment>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSplitCommentRequestDTO, SplitComment>, CreateSplitCommentRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO>, SimpleSplitCommentResponseMapper>();
            #endregion

            #region Split comment like
            _ = builder.Services.AddScoped<ICreateService<SplitCommentLike>, CreateService<SplitCommentLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<SplitCommentLike>, ReadService<SplitCommentLike>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitCommentLike>, DeleteService<SplitCommentLike>>();
            #endregion

            #region Split favorite
            _ = builder.Services.AddScoped<ICreateService<FavoriteSplit>, CreateService<FavoriteSplit>>();
            _ = builder.Services.AddScoped<IReadSingleService<FavoriteSplit>, ReadService<FavoriteSplit>>();
            _ = builder.Services.AddScoped<IReadRangeService<FavoriteSplit>, ReadService<FavoriteSplit>>();
            _ = builder.Services.AddScoped<IDeleteService<FavoriteSplit>, DeleteService<FavoriteSplit>>();
            #endregion

            #region Completed workouts / sets
            _ = builder.Services.AddScoped<ICreateService<CompletedWorkout>, CreateService<CompletedWorkout>>();
            _ = builder.Services.AddScoped<IReadRangeService<CompletedWorkout>, ReadService<CompletedWorkout>>();
            _ = builder.Services.AddScoped<IReadRangeSelectedService<CompletedWorkout>, ReadService<CompletedWorkout>>();
            _ = builder.Services.AddScoped<IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO>, SimpleWeekOfCompletedWorkoutsResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO>, DetailedWeekOfCompletedWorkoutsResponseMapper>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateCompletedSetRequestDTO, CompletedSet>, CreateCompletedSetRequestMapper>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateCompletedWorkoutRequestDTO, CompletedWorkout>, CreateCompletedWorkoutRequestMapper>();
            _ = builder.Services.AddScoped<IReadRangeSelectedService<CompletedSet>, ReadService<CompletedSet>>();
            #endregion

            #region Pins
            _ = builder.Services.AddScoped<ICreateRangeService<WorkoutPin>, CreateService<WorkoutPin>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutPin>, ReadService<WorkoutPin>>();
            _ = builder.Services.AddScoped<IUpdateRangeService<WorkoutPin>, UpdateService<WorkoutPin>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutPin>, DeleteService<WorkoutPin>>();
            _ = builder.Services.AddScoped<ICreateRangeService<SplitPin>, CreateService<SplitPin>>();
            _ = builder.Services.AddScoped<IReadSingleService<SplitPin>, ReadService<SplitPin>>();
            _ = builder.Services.AddScoped<IUpdateRangeService<SplitPin>, UpdateService<SplitPin>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitPin>, DeleteService<SplitPin>>();
            _ = builder.Services.AddScoped<IResponseMapper<WorkoutPin, PinResponseDTO>, SimpleWorkoutPinResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<SplitPin, PinResponseDTO>, SimpleSplitPinResponseMapper>();
            #endregion

            WebApplication app = builder.Build();

            #region Middleware
            _ = app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            _ = app.UseCors();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseClientRateLimiting();
            #endregion

            _ = app.MapControllers();
            app.Run();
        }
    }
}