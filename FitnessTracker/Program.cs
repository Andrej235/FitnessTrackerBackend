using System.Text;
using MailKit.Net.Smtp;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Auth;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Data;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Update;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Emails;
using FitnessTracker.Services.UserServices.EmailConfirmationSenderService;
using FitnessTracker.Services.UserServices.EmailConfirmationService;
using FitnessTracker.Services.UserServices.ResetPasswordSenderService;
using FitnessTracker.Services.UserServices.ResetPasswordService;
using FitnessTracker.Services.EmailSender;
using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Services.Mapping.Request.EquipmentMappers;
using FitnessTracker.Services.Mapping.Request.ExerciseMappers;
using FitnessTracker.Services.Mapping.Request.MuscleGroupMappers;
using FitnessTracker.Services.Mapping.Request.MuscleMappers;
using FitnessTracker.Services.Mapping.Request.UserMappers;
using FitnessTracker.Services.Mapping.Response.ExerciseMappers;
using FitnessTracker.Services.Mapping.Response.UserMappers;
using FitnessTracker.Services.Mapping.Response.EquipmentMappers;
using FitnessTracker.Services.Mapping.Response.MuscleGroupMappers;
using FitnessTracker.Services.Mapping.Response.MuscleMappers;
using FitnessTracker.Services.Mapping.Request.SetMappers;
using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.Services.Mapping.Request.WorkoutMappers;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Services.Mapping.Response.WorkoutMappers;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.Services.Mapping.Response.SetMappers;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Services.Read.QueryBased;
using FitnessTracker.Services.Mapping.Response.SplitMappers;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Services.Mapping.Request.SplitMappers;
using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.Services.Mapping.Response.CompletedWorkoutMappers;
using FitnessTracker.DTOs.Responses.AuthTokens;
using FitnessTracker.Services.Mapping.Response.AuthTokens;

namespace FitnessTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Services.AddSingleton(configuration);
            builder.Services.AddControllers();
            builder.Services.AddDbContext<DataContext>(x =>
            {
                x.UseSqlServer(configuration.GetConnectionString("SQLConnectionString"));
                x.EnableSensitiveDataLogging(); //TODO-PROD: remove in production
            });
            builder.Services.AddSwaggerGen(options =>
            {
                options.SupportNonNullableReferenceTypes();
            });


            #region Rate limiting
            builder.Services.AddMemoryCache();
            builder.Services.Configure<ClientRateLimitOptions>(options =>
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

            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddInMemoryRateLimiting();
            #endregion

            #region JWT / Auth
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                    ClockSkew = TimeSpan.Zero,
                };
            })
            .AddScheme<AuthenticationSchemeOptions, AllowExpiredAuthenticationHandler>("AllowExpired", (p) => { });

            builder.Services.AddScoped<ITokenManager, TokenManager>();
            builder.Services.AddAuthorization();
            #endregion

            #region Cors
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "192.168.1.100")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
            #endregion

            #region Emails
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            if (emailConfig is not null)
                builder.Services.AddSingleton(emailConfig);

            builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
            builder.Services.AddScoped<SmtpClient>();
            #endregion

            #region Email verification tokens
            builder.Services.AddScoped<ICreateService<EmailConfirmation>, CreateService<EmailConfirmation>>();
            builder.Services.AddScoped<IReadSingleService<EmailConfirmation>, ReadExpressionService<EmailConfirmation>>();
            builder.Services.AddScoped<IReadRangeService<EmailConfirmation>, ReadExpressionService<EmailConfirmation>>();
            builder.Services.AddScoped<IDeleteService<EmailConfirmation>, DeleteService<EmailConfirmation>>();
            builder.Services.AddScoped<IDeleteRangeService<EmailConfirmation>, DeleteRangeService<EmailConfirmation>>();
            #endregion

            #region Exercise
            builder.Services.AddScoped<ICreateService<Exercise>, CreateService<Exercise>>();
            builder.Services.AddScoped<IReadSingleService<Exercise>, ReadExpressionService<Exercise>>();
            builder.Services.AddScoped<IReadRangeService<Exercise>, ReadExpressionService<Exercise>>();
            builder.Services.AddScoped<IReadQueryService<Exercise>, ExerciseReadQueryService>();
            builder.Services.AddScoped<IUpdateService<Exercise>, UpdateService<Exercise>>();
            builder.Services.AddScoped<IDeleteService<Exercise>, DeleteService<Exercise>>();
            builder.Services.AddScoped<IRequestMapper<CreateExerciseRequestDTO, Exercise>, CreateExerciseRequestMapper>();
            builder.Services.AddScoped<IRequestMapper<UpdateExerciseRequestDTO, Exercise>, UpdateExerciseRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<Exercise, SimpleExerciseResponseDTO>, SimpleExerciseResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<Exercise, DetailedExerciseResponseDTO>, DetailedExerciseResponseMapper>();
            #endregion

            #region User
            builder.Services.AddScoped<ICreateService<User>, UserCreateService>();
            builder.Services.AddScoped<IReadSingleService<User>, UserReadExpressionService>();
            builder.Services.AddScoped<IReadRangeService<User>, UserReadExpressionService>();
            builder.Services.AddScoped<IUpdateService<User>, UpdateService<User>>();
            builder.Services.AddScoped<IRequestMapper<RegisterUserRequestDTO, User>, RegisterUserRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<User, SimpleUserResponseDTO>, SimpleUserResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<User, DetailedUserResponseDTO>, DetailedUserResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<User, DetailedPublicUserResponseDTO>, DetailedPublicUserResponseMapper>();
            builder.Services.AddScoped<IEmailConfirmationSenderService, EmailConfirmationSenderService>();
            builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
            builder.Services.AddScoped<IResetPasswordEmailSenderService, ResetPasswordEmailSender>();
            builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
            #endregion

            #region User follows
            builder.Services.AddScoped<ICreateService<UserFollows>, CreateService<UserFollows>>();
            builder.Services.AddScoped<IReadSingleService<UserFollows>, ReadExpressionService<UserFollows>>();
            builder.Services.AddScoped<IDeleteService<UserFollows>, DeleteService<UserFollows>>();
            #endregion

            #region Set
            builder.Services.AddScoped<ICreateService<Set>, CreateService<Set>>();
            builder.Services.AddScoped<IReadSingleService<Set>, ReadExpressionService<Set>>();
            builder.Services.AddScoped<IUpdateService<Set>, UpdateService<Set>>();
            builder.Services.AddScoped<IDeleteService<Set>, DeleteService<Set>>();
            builder.Services.AddScoped<IRequestMapper<CreateSetRequestDTO, Set>, CreateSetRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<Set, DetailedSetResponseDTO>, DetailedSetResponseMapper>();
            #endregion

            #region Workouts
            builder.Services.AddScoped<IReadSingleService<Workout>, WorkoutReadExpressionService>();
            builder.Services.AddScoped<IReadRangeService<Workout>, WorkoutReadExpressionService>();
            builder.Services.AddScoped<ICreateService<Workout>, CreateService<Workout>>();
            builder.Services.AddScoped<IUpdateService<Workout>, WorkoutUpdateService>();
            builder.Services.AddScoped<IDeleteService<Workout>, DeleteService<Workout>>();
            builder.Services.AddScoped<IRequestMapper<CreateWorkoutRequestDTO, Workout>, CreateWorkoutRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<Workout, SimpleWorkoutResponseDTO>, SimpleWorkoutResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<Workout, DetailedWorkoutResponseDTO>, DetailedWorkoutResponseMapper>();
            #endregion

            #region Workout like
            builder.Services.AddScoped<ICreateService<WorkoutLike>, CreateService<WorkoutLike>>();
            builder.Services.AddScoped<IReadSingleService<WorkoutLike>, ReadExpressionService<WorkoutLike>>();
            builder.Services.AddScoped<IDeleteService<WorkoutLike>, DeleteService<WorkoutLike>>();
            #endregion

            #region Workout comment
            builder.Services.AddScoped<ICreateService<WorkoutComment>, CreateService<WorkoutComment>>();
            builder.Services.AddScoped<IReadSingleService<WorkoutComment>, ReadExpressionService<WorkoutComment>>();
            builder.Services.AddScoped<IReadRangeService<WorkoutComment>, ReadExpressionService<WorkoutComment>>();
            builder.Services.AddScoped<IDeleteService<WorkoutComment>, DeleteService<WorkoutComment>>();
            builder.Services.AddScoped<IDeleteRangeService<WorkoutComment>, DeleteRangeService<WorkoutComment>>();
            builder.Services.AddScoped<IRequestMapper<CreateWorkoutCommentRequestDTO, WorkoutComment>, CreateWorkoutCommentRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<WorkoutComment, SimpleWorkoutCommentResponseDTO>, SimpleWorkoutCommentResponseMapper>();
            #endregion

            #region Workout comment like
            builder.Services.AddScoped<ICreateService<WorkoutCommentLike>, CreateService<WorkoutCommentLike>>();
            builder.Services.AddScoped<IReadSingleService<WorkoutCommentLike>, ReadExpressionService<WorkoutCommentLike>>();
            builder.Services.AddScoped<IDeleteService<WorkoutCommentLike>, DeleteService<WorkoutCommentLike>>();
            #endregion

            #region Workout favorite
            builder.Services.AddScoped<ICreateService<FavoriteWorkout>, CreateService<FavoriteWorkout>>();
            builder.Services.AddScoped<IReadSingleService<FavoriteWorkout>, ReadExpressionService<FavoriteWorkout>>();
            builder.Services.AddScoped<IDeleteService<FavoriteWorkout>, DeleteService<FavoriteWorkout>>();
            #endregion

            #region Muscle group
            builder.Services.AddScoped<ICreateService<MuscleGroup>, CreateService<MuscleGroup>>();
            builder.Services.AddScoped<ICreateRangeService<MuscleGroup>, CreateService<MuscleGroup>>();
            builder.Services.AddScoped<IReadSingleService<MuscleGroup>, ReadExpressionService<MuscleGroup>>();
            builder.Services.AddScoped<IReadRangeService<MuscleGroup>, ReadExpressionService<MuscleGroup>>();
            builder.Services.AddScoped<IUpdateService<MuscleGroup>, UpdateService<MuscleGroup>>();
            builder.Services.AddScoped<IDeleteService<MuscleGroup>, DeleteService<MuscleGroup>>();
            builder.Services.AddScoped<IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup>, CreateMuscleGroupRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO>, SimpleMuscleGroupResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<MuscleGroup, DetailedMuscleGroupResponseDTO>, DetailedMuscleGroupResponseMapper>();
            #endregion

            #region Muscle
            builder.Services.AddScoped<ICreateService<Muscle>, CreateService<Muscle>>();
            builder.Services.AddScoped<ICreateRangeService<Muscle>, CreateService<Muscle>>();
            builder.Services.AddScoped<IReadSingleService<Muscle>, ReadExpressionService<Muscle>>();
            builder.Services.AddScoped<IReadRangeService<Muscle>, ReadExpressionService<Muscle>>();
            builder.Services.AddScoped<IUpdateService<Muscle>, UpdateService<Muscle>>();
            builder.Services.AddScoped<IDeleteService<Muscle>, DeleteService<Muscle>>();
            builder.Services.AddScoped<IRequestMapper<CreateMuscleRequestDTO, Muscle>, CreateMuscleRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<Muscle, SimpleMuscleResponseDTO>, SimpleMuscleResponseMapper>();
            #endregion

            #region Equipment exercise usage
            builder.Services.AddScoped<ICreateService<EquipmentUsage>, CreateService<EquipmentUsage>>();
            builder.Services.AddScoped<ICreateRangeService<EquipmentUsage>, CreateService<EquipmentUsage>>();
            builder.Services.AddScoped<IReadRangeService<EquipmentUsage>, ReadExpressionService<EquipmentUsage>>();
            builder.Services.AddScoped<IDeleteRangeService<EquipmentUsage>, DeleteRangeService<EquipmentUsage>>();
            #endregion

            #region Equipment
            builder.Services.AddScoped<ICreateService<Equipment>, CreateService<Equipment>>();
            builder.Services.AddScoped<ICreateRangeService<Equipment>, CreateService<Equipment>>();
            builder.Services.AddScoped<IReadSingleService<Equipment>, ReadExpressionService<Equipment>>();
            builder.Services.AddScoped<IReadRangeService<Equipment>, ReadExpressionService<Equipment>>();
            builder.Services.AddScoped<IUpdateService<Equipment>, UpdateService<Equipment>>();
            builder.Services.AddScoped<IDeleteService<Equipment>, DeleteService<Equipment>>();
            builder.Services.AddScoped<IRequestMapper<CreateEquipmentRequestDTO, Equipment>, CreateEquipmentRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<Equipment, SimpleEquipmentResponseDTO>, SimpleEquipmentResponseMapper>();
            #endregion

            #region Primary muscle group
            builder.Services.AddScoped<ICreateService<PrimaryMuscleGroupInExercise>, CreateService<PrimaryMuscleGroupInExercise>>();
            builder.Services.AddScoped<ICreateRangeService<PrimaryMuscleGroupInExercise>, CreateService<PrimaryMuscleGroupInExercise>>();
            builder.Services.AddScoped<IReadRangeService<PrimaryMuscleGroupInExercise>, ReadExpressionService<PrimaryMuscleGroupInExercise>>();
            builder.Services.AddScoped<IDeleteRangeService<PrimaryMuscleGroupInExercise>, DeleteRangeService<PrimaryMuscleGroupInExercise>>();
            #endregion

            #region Secondary muscle group
            builder.Services.AddScoped<ICreateService<SecondaryMuscleGroupInExercise>, CreateService<SecondaryMuscleGroupInExercise>>();
            builder.Services.AddScoped<ICreateRangeService<SecondaryMuscleGroupInExercise>, CreateService<SecondaryMuscleGroupInExercise>>();
            builder.Services.AddScoped<IReadRangeService<SecondaryMuscleGroupInExercise>, ReadExpressionService<SecondaryMuscleGroupInExercise>>();
            builder.Services.AddScoped<IDeleteRangeService<SecondaryMuscleGroupInExercise>, DeleteRangeService<SecondaryMuscleGroupInExercise>>();
            #endregion

            #region Primary muscle
            builder.Services.AddScoped<ICreateRangeService<PrimaryMuscleInExercise>, CreateService<PrimaryMuscleInExercise>>();
            builder.Services.AddScoped<IReadRangeService<PrimaryMuscleInExercise>, ReadExpressionService<PrimaryMuscleInExercise>>();
            builder.Services.AddScoped<IDeleteRangeService<PrimaryMuscleInExercise>, DeleteRangeService<PrimaryMuscleInExercise>>();
            #endregion

            #region Secondary muscle
            builder.Services.AddScoped<ICreateRangeService<SecondaryMuscleInExercise>, CreateService<SecondaryMuscleInExercise>>();
            builder.Services.AddScoped<IReadRangeService<SecondaryMuscleInExercise>, ReadExpressionService<SecondaryMuscleInExercise>>();
            builder.Services.AddScoped<IDeleteRangeService<SecondaryMuscleInExercise>, DeleteRangeService<SecondaryMuscleInExercise>>();
            #endregion

            #region Auth tokens
            builder.Services.AddScoped<ICreateService<RefreshToken>, RefreshTokenCreateService>();
            builder.Services.AddScoped<IReadSingleService<RefreshToken>, ReadExpressionService<RefreshToken>>();
            builder.Services.AddScoped<IReadRangeService<RefreshToken>, ReadExpressionService<RefreshToken>>();
            builder.Services.AddScoped<IUpdateService<RefreshToken>, UpdateService<RefreshToken>>();
            builder.Services.AddScoped<IDeleteService<RefreshToken>, DeleteService<RefreshToken>>();
            builder.Services.AddScoped<IDeleteRangeService<RefreshToken>, DeleteRangeService<RefreshToken>>();

            builder.Services.AddScoped<IResponseMapper<string, SimpleJWTResponseDTO>, SimpleJWTResponseMapper>();
            #endregion

            #region Split
            builder.Services.AddScoped<ICreateService<Split>, CreateService<Split>>();
            builder.Services.AddScoped<IReadRangeService<Split>, SplitReadExpressionService>();
            builder.Services.AddScoped<IReadSingleService<Split>, SplitReadExpressionService>();
            builder.Services.AddScoped<IUpdateService<Split>, UpdateService<Split>>();
            builder.Services.AddScoped<IRequestMapper<CreateSplitRequestDTO, Split>, CreateSplitRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<Split, SimpleSplitResponseDTO>, SimpleSplitResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<Split, DetailedSplitResponseDTO>, DetailedSplitResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<Split, DetailedUserSplitResponseDTO>, DetailedUserSplitResponseMapper>();
            #endregion

            #region Split workout
            builder.Services.AddScoped<IUpdateService<SplitWorkout>, UpdateService<SplitWorkout>>();
            builder.Services.AddScoped<IRequestMapper<CreateSplitWorkoutRequestDTO, SplitWorkout>, CreateSplitWorkoutRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO>, SimpleSplitWorkoutResponseMapper>();
            #endregion

            #region Split like
            builder.Services.AddScoped<ICreateService<SplitLike>, CreateService<SplitLike>>();
            builder.Services.AddScoped<IReadSingleService<SplitLike>, ReadExpressionService<SplitLike>>();
            builder.Services.AddScoped<IDeleteService<SplitLike>, DeleteService<SplitLike>>();
            #endregion

            #region Split comment
            builder.Services.AddScoped<ICreateService<SplitComment>, CreateService<SplitComment>>();
            builder.Services.AddScoped<IReadSingleService<SplitComment>, ReadExpressionService<SplitComment>>();
            builder.Services.AddScoped<IReadRangeService<SplitComment>, ReadExpressionService<SplitComment>>();
            builder.Services.AddScoped<IDeleteService<SplitComment>, DeleteService<SplitComment>>();
            builder.Services.AddScoped<IDeleteRangeService<SplitComment>, DeleteRangeService<SplitComment>>();
            builder.Services.AddScoped<IRequestMapper<CreateSplitCommentRequestDTO, SplitComment>, CreateSplitCommentRequestMapper>();
            builder.Services.AddScoped<IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO>, SimpleSplitCommentResponseMapper>();
            #endregion

            #region Split comment like
            builder.Services.AddScoped<ICreateService<SplitCommentLike>, CreateService<SplitCommentLike>>();
            builder.Services.AddScoped<IReadSingleService<SplitCommentLike>, ReadExpressionService<SplitCommentLike>>();
            builder.Services.AddScoped<IDeleteService<SplitCommentLike>, DeleteService<SplitCommentLike>>();
            #endregion

            #region Split favorite
            builder.Services.AddScoped<ICreateService<FavoriteSplit>, CreateService<FavoriteSplit>>();
            builder.Services.AddScoped<IReadSingleService<FavoriteSplit>, ReadExpressionService<FavoriteSplit>>();
            builder.Services.AddScoped<IDeleteService<FavoriteSplit>, DeleteService<FavoriteSplit>>();
            #endregion

            #region Completed workouts
            builder.Services.AddScoped<ICreateService<CompletedWorkout>, CreateService<CompletedWorkout>>();
            builder.Services.AddScoped<IReadRangeService<CompletedWorkout>, CompletedWorkoutReadExpressionService>();
            builder.Services.AddScoped<IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO>, SimpleWeekOfCompletedWorkoutsResponseMapper>();
            builder.Services.AddScoped<IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO>, DetailedWeekOfCompletedWorkoutsResponseMapper>();
            #endregion

            var app = builder.Build();

            #region Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseClientRateLimiting();
            #endregion

            app.MapControllers();
            app.Run();
        }
    }
}