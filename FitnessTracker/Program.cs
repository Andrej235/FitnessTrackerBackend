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
using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Emails;
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
using FitnessTracker.Services.Read.Count;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Services.Read.QueryBased;
using FitnessTracker.Services.Update;
using FitnessTracker.Services.UserServices.EmailConfirmationSenderService;
using FitnessTracker.Services.UserServices.EmailConfirmationService;
using FitnessTracker.Services.UserServices.ResetPasswordSenderService;
using FitnessTracker.Services.UserServices.ResetPasswordService;
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
            _ = builder.Services.AddScoped<IReadSingleService<EmailConfirmation>, ReadExpressionService<EmailConfirmation>>();
            _ = builder.Services.AddScoped<IReadRangeService<EmailConfirmation>, ReadExpressionService<EmailConfirmation>>();
            _ = builder.Services.AddScoped<IDeleteService<EmailConfirmation>, DeleteService<EmailConfirmation>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<EmailConfirmation>, DeleteRangeService<EmailConfirmation>>();
            #endregion

            #region Exercise
            _ = builder.Services.AddScoped<ICreateService<Exercise>, CreateService<Exercise>>();
            _ = builder.Services.AddScoped<IReadSingleService<Exercise>, ReadExpressionService<Exercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<Exercise>, ReadExpressionService<Exercise>>();
            _ = builder.Services.AddScoped<IReadQueryService<Exercise>, ExerciseReadQueryService>();
            _ = builder.Services.AddScoped<IUpdateService<Exercise>, UpdateService<Exercise>>();
            _ = builder.Services.AddScoped<IDeleteService<Exercise>, DeleteService<Exercise>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateExerciseRequestDTO, Exercise>, CreateExerciseRequestMapper>();
            _ = builder.Services.AddScoped<IRequestMapper<UpdateExerciseRequestDTO, Exercise>, UpdateExerciseRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Exercise, SimpleExerciseResponseDTO>, SimpleExerciseResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Exercise, DetailedExerciseResponseDTO>, DetailedExerciseResponseMapper>();
            #endregion

            #region Favorite exercise
            _ = builder.Services.AddScoped<ICreateService<FavoriteExercise>, CreateService<FavoriteExercise>>();
            _ = builder.Services.AddScoped<IReadSingleService<FavoriteExercise>, ReadExpressionService<FavoriteExercise>>();
            _ = builder.Services.AddScoped<IDeleteService<FavoriteExercise>, DeleteService<FavoriteExercise>>();
            #endregion

            #region User
            _ = builder.Services.AddScoped<ICreateService<User>, UserCreateService>();
            _ = builder.Services.AddScoped<IReadSingleService<User>, UserReadExpressionService>();
            _ = builder.Services.AddScoped<IReadRangeService<User>, UserReadExpressionService>();
            _ = builder.Services.AddScoped<IUpdateService<User>, UpdateService<User>>();
            _ = builder.Services.AddScoped<IRequestMapper<RegisterUserRequestDTO, User>, RegisterUserRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<User, SimpleUserResponseDTO>, SimpleUserResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<User, DetailedUserResponseDTO>, DetailedUserResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<User, DetailedPublicUserResponseDTO>, DetailedPublicUserResponseMapper>();
            _ = builder.Services.AddScoped<IEmailConfirmationSenderService, EmailConfirmationSenderService>();
            _ = builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
            _ = builder.Services.AddScoped<IResetPasswordEmailSenderService, ResetPasswordEmailSender>();
            _ = builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
            #endregion

            #region User follows
            _ = builder.Services.AddScoped<ICreateService<UserFollows>, CreateService<UserFollows>>();
            _ = builder.Services.AddScoped<IReadSingleService<UserFollows>, ReadExpressionService<UserFollows>>();
            _ = builder.Services.AddScoped<ICountService<UserFollows>, CountService<UserFollows>>();
            _ = builder.Services.AddScoped<IReadRangeService<UserFollows>, ReadExpressionService<UserFollows>>();
            _ = builder.Services.AddScoped<IDeleteService<UserFollows>, DeleteService<UserFollows>>();
            #endregion

            #region Set
            _ = builder.Services.AddScoped<ICreateService<Set>, CreateService<Set>>();
            _ = builder.Services.AddScoped<ICreateRangeService<Set>, CreateService<Set>>();
            _ = builder.Services.AddScoped<IReadSingleService<Set>, ReadExpressionService<Set>>();
            _ = builder.Services.AddScoped<IReadRangeService<Set>, ReadExpressionService<Set>>();
            _ = builder.Services.AddScoped<IUpdateService<Set>, UpdateService<Set>>();
            _ = builder.Services.AddScoped<IDeleteService<Set>, DeleteService<Set>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<Set>, DeleteRangeService<Set>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSetRequestDTO, Set>, CreateSetRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Set, DetailedSetResponseDTO>, DetailedSetResponseMapper>();
            #endregion

            #region Workouts
            _ = builder.Services.AddScoped<IReadSingleService<Workout>, WorkoutReadExpressionService>();
            _ = builder.Services.AddScoped<IReadRangeService<Workout>, WorkoutReadExpressionService>();
            _ = builder.Services.AddScoped<ICreateService<Workout>, CreateService<Workout>>();
            _ = builder.Services.AddScoped<IUpdateService<Workout>, WorkoutUpdateService>();
            _ = builder.Services.AddScoped<IDeleteService<Workout>, DeleteService<Workout>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateWorkoutRequestDTO, Workout>, CreateWorkoutRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Workout, NewWorkoutResponseDTO>, NewWorkoutResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Workout, SimpleWorkoutResponseDTO>, SimpleWorkoutResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Workout, DetailedWorkoutResponseDTO>, DetailedWorkoutResponseMapper>();
            #endregion

            #region Workout like
            _ = builder.Services.AddScoped<ICreateService<WorkoutLike>, CreateService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutLike>, WorkoutLikeReadExpressionService>();
            _ = builder.Services.AddScoped<IReadRangeService<WorkoutLike>, WorkoutLikeReadExpressionService>();
            _ = builder.Services.AddScoped<ICountService<WorkoutLike>, CountService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutLike>, DeleteService<WorkoutLike>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<WorkoutLike>, DeleteRangeService<WorkoutLike>>();
            #endregion

            #region Workout comment
            _ = builder.Services.AddScoped<ICreateService<WorkoutComment>, CreateService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutComment>, ReadExpressionService<WorkoutComment>>();
            _ = builder.Services.AddScoped<ICountService<WorkoutComment>, CountService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IReadRangeService<WorkoutComment>, ReadExpressionService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutComment>, DeleteService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<WorkoutComment>, DeleteRangeService<WorkoutComment>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment>, CreateWorkoutCommentRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<WorkoutComment, SimpleWorkoutCommentResponseDTO>, SimpleWorkoutCommentResponseMapper>();
            #endregion

            #region Workout comment like
            _ = builder.Services.AddScoped<ICreateService<WorkoutCommentLike>, CreateService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<WorkoutCommentLike>, ReadExpressionService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IReadRangeService<WorkoutCommentLike>, ReadExpressionService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IDeleteService<WorkoutCommentLike>, DeleteService<WorkoutCommentLike>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<WorkoutCommentLike>, DeleteRangeService<WorkoutCommentLike>>();
            #endregion

            #region Workout favorite
            _ = builder.Services.AddScoped<ICreateService<FavoriteWorkout>, CreateService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IReadSingleService<FavoriteWorkout>, FavoriteWorkoutReadExpressionService>();
            _ = builder.Services.AddScoped<IReadRangeService<FavoriteWorkout>, FavoriteWorkoutReadExpressionService>();
            _ = builder.Services.AddScoped<ICountService<FavoriteWorkout>, CountService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IDeleteService<FavoriteWorkout>, DeleteService<FavoriteWorkout>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<FavoriteWorkout>, DeleteRangeService<FavoriteWorkout>>();
            #endregion

            #region Muscle group
            _ = builder.Services.AddScoped<ICreateService<MuscleGroup>, CreateService<MuscleGroup>>();
            _ = builder.Services.AddScoped<ICreateRangeService<MuscleGroup>, CreateService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IReadSingleService<MuscleGroup>, ReadExpressionService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IReadRangeService<MuscleGroup>, ReadExpressionService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IUpdateService<MuscleGroup>, UpdateService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IDeleteService<MuscleGroup>, DeleteService<MuscleGroup>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup>, CreateMuscleGroupRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO>, SimpleMuscleGroupResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<MuscleGroup, DetailedMuscleGroupResponseDTO>, DetailedMuscleGroupResponseMapper>();
            #endregion

            #region Muscle
            _ = builder.Services.AddScoped<ICreateService<Muscle>, CreateService<Muscle>>();
            _ = builder.Services.AddScoped<ICreateRangeService<Muscle>, CreateService<Muscle>>();
            _ = builder.Services.AddScoped<IReadSingleService<Muscle>, ReadExpressionService<Muscle>>();
            _ = builder.Services.AddScoped<IReadRangeService<Muscle>, ReadExpressionService<Muscle>>();
            _ = builder.Services.AddScoped<IUpdateService<Muscle>, UpdateService<Muscle>>();
            _ = builder.Services.AddScoped<IDeleteService<Muscle>, DeleteService<Muscle>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateMuscleRequestDTO, Muscle>, CreateMuscleRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Muscle, SimpleMuscleResponseDTO>, SimpleMuscleResponseMapper>();
            #endregion

            #region Equipment exercise usage
            _ = builder.Services.AddScoped<ICreateService<EquipmentUsage>, CreateService<EquipmentUsage>>();
            _ = builder.Services.AddScoped<ICreateRangeService<EquipmentUsage>, CreateService<EquipmentUsage>>();
            _ = builder.Services.AddScoped<IReadRangeService<EquipmentUsage>, ReadExpressionService<EquipmentUsage>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<EquipmentUsage>, DeleteRangeService<EquipmentUsage>>();
            #endregion

            #region Equipment
            _ = builder.Services.AddScoped<ICreateService<Equipment>, CreateService<Equipment>>();
            _ = builder.Services.AddScoped<ICreateRangeService<Equipment>, CreateService<Equipment>>();
            _ = builder.Services.AddScoped<IReadSingleService<Equipment>, ReadExpressionService<Equipment>>();
            _ = builder.Services.AddScoped<IReadRangeService<Equipment>, ReadExpressionService<Equipment>>();
            _ = builder.Services.AddScoped<IUpdateService<Equipment>, UpdateService<Equipment>>();
            _ = builder.Services.AddScoped<IDeleteService<Equipment>, DeleteService<Equipment>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateEquipmentRequestDTO, Equipment>, CreateEquipmentRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Equipment, SimpleEquipmentResponseDTO>, SimpleEquipmentResponseMapper>();
            #endregion

            #region Primary muscle group
            _ = builder.Services.AddScoped<ICreateService<PrimaryMuscleGroupInExercise>, CreateService<PrimaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<ICreateRangeService<PrimaryMuscleGroupInExercise>, CreateService<PrimaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<PrimaryMuscleGroupInExercise>, ReadExpressionService<PrimaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<PrimaryMuscleGroupInExercise>, DeleteRangeService<PrimaryMuscleGroupInExercise>>();
            #endregion

            #region Secondary muscle group
            _ = builder.Services.AddScoped<ICreateService<SecondaryMuscleGroupInExercise>, CreateService<SecondaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<ICreateRangeService<SecondaryMuscleGroupInExercise>, CreateService<SecondaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<SecondaryMuscleGroupInExercise>, ReadExpressionService<SecondaryMuscleGroupInExercise>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<SecondaryMuscleGroupInExercise>, DeleteRangeService<SecondaryMuscleGroupInExercise>>();
            #endregion

            #region Primary muscle
            _ = builder.Services.AddScoped<ICreateRangeService<PrimaryMuscleInExercise>, CreateService<PrimaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<PrimaryMuscleInExercise>, ReadExpressionService<PrimaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<PrimaryMuscleInExercise>, DeleteRangeService<PrimaryMuscleInExercise>>();
            #endregion

            #region Secondary muscle
            _ = builder.Services.AddScoped<ICreateRangeService<SecondaryMuscleInExercise>, CreateService<SecondaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IReadRangeService<SecondaryMuscleInExercise>, ReadExpressionService<SecondaryMuscleInExercise>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<SecondaryMuscleInExercise>, DeleteRangeService<SecondaryMuscleInExercise>>();
            #endregion

            #region Auth tokens
            _ = builder.Services.AddScoped<ICreateService<RefreshToken>, RefreshTokenCreateService>();
            _ = builder.Services.AddScoped<IReadSingleService<RefreshToken>, ReadExpressionService<RefreshToken>>();
            _ = builder.Services.AddScoped<IReadRangeService<RefreshToken>, ReadExpressionService<RefreshToken>>();
            _ = builder.Services.AddScoped<IUpdateService<RefreshToken>, UpdateService<RefreshToken>>();
            _ = builder.Services.AddScoped<IDeleteService<RefreshToken>, DeleteService<RefreshToken>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<RefreshToken>, DeleteRangeService<RefreshToken>>();

            _ = builder.Services.AddScoped<IResponseMapper<string, SimpleJWTResponseDTO>, SimpleJWTResponseMapper>();
            #endregion

            #region Split
            _ = builder.Services.AddScoped<ICreateService<Split>, CreateService<Split>>();
            _ = builder.Services.AddScoped<IReadRangeService<Split>, SplitReadExpressionService>();
            _ = builder.Services.AddScoped<IReadSingleService<Split>, SplitReadExpressionService>();
            _ = builder.Services.AddScoped<IUpdateService<Split>, UpdateService<Split>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSplitRequestDTO, Split>, CreateSplitRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Split, SimpleSplitResponseDTO>, SimpleSplitResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Split, DetailedSplitResponseDTO>, DetailedSplitResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<Split, DetailedUserSplitResponseDTO>, DetailedUserSplitResponseMapper>();
            #endregion

            #region Split workout
            _ = builder.Services.AddScoped<IUpdateService<SplitWorkout>, UpdateService<SplitWorkout>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSplitWorkoutRequestDTO, SplitWorkout>, CreateSplitWorkoutRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO>, SimpleSplitWorkoutResponseMapper>();
            #endregion

            #region Split like
            _ = builder.Services.AddScoped<ICreateService<SplitLike>, CreateService<SplitLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<SplitLike>, ReadExpressionService<SplitLike>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitLike>, DeleteService<SplitLike>>();
            #endregion

            #region Split comment
            _ = builder.Services.AddScoped<ICreateService<SplitComment>, CreateService<SplitComment>>();
            _ = builder.Services.AddScoped<IReadSingleService<SplitComment>, ReadExpressionService<SplitComment>>();
            _ = builder.Services.AddScoped<IReadRangeService<SplitComment>, ReadExpressionService<SplitComment>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitComment>, DeleteService<SplitComment>>();
            _ = builder.Services.AddScoped<IDeleteRangeService<SplitComment>, DeleteRangeService<SplitComment>>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateSplitCommentRequestDTO, SplitComment>, CreateSplitCommentRequestMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO>, SimpleSplitCommentResponseMapper>();
            #endregion

            #region Split comment like
            _ = builder.Services.AddScoped<ICreateService<SplitCommentLike>, CreateService<SplitCommentLike>>();
            _ = builder.Services.AddScoped<IReadSingleService<SplitCommentLike>, ReadExpressionService<SplitCommentLike>>();
            _ = builder.Services.AddScoped<IDeleteService<SplitCommentLike>, DeleteService<SplitCommentLike>>();
            #endregion

            #region Split favorite
            _ = builder.Services.AddScoped<ICreateService<FavoriteSplit>, CreateService<FavoriteSplit>>();
            _ = builder.Services.AddScoped<IReadSingleService<FavoriteSplit>, ReadExpressionService<FavoriteSplit>>();
            _ = builder.Services.AddScoped<IDeleteService<FavoriteSplit>, DeleteService<FavoriteSplit>>();
            #endregion

            #region Completed workouts / sets
            _ = builder.Services.AddScoped<ICreateService<CompletedWorkout>, CreateService<CompletedWorkout>>();
            _ = builder.Services.AddScoped<IReadRangeService<CompletedWorkout>, CompletedWorkoutReadExpressionService>();
            _ = builder.Services.AddScoped<ICountService<CompletedWorkout>, CountService<CompletedWorkout>>();
            _ = builder.Services.AddScoped<IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO>, SimpleWeekOfCompletedWorkoutsResponseMapper>();
            _ = builder.Services.AddScoped<IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO>, DetailedWeekOfCompletedWorkoutsResponseMapper>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateCompletedSetRequestDTO, CompletedSet>, CreateCompletedSetRequestMapper>();
            _ = builder.Services.AddScoped<IRequestMapper<CreateCompletedWorkoutRequestDTO, CompletedWorkout>, CreateCompletedWorkoutRequestMapper>();
            #endregion

            WebApplication app = builder.Build();

            #region Middleware
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