using FitnessTracker.Auth;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FitnessTracker.Data;
using FitnessTracker.DTOs;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Update;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Emails;
using MailKit.Net.Smtp;

namespace FitnessTracker
{
    public class Program
    {
        private static IServiceProvider serviceProvider = null!;
        private static ConfigurationManager configuration = null!;

        public static object? GetService<T>() => serviceProvider.GetService(typeof(T));
        public static object? GetService(Type type) => serviceProvider.GetService(type);

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            configuration = builder.Configuration;

            #region JWT
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
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddScheme<AuthenticationSchemeOptions, AllowExpiredAuthenticationHandler>("AllowExpired", (p) => { });
            #endregion

            builder.Services.AddAuthorization();
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

            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            if (emailConfig is not null)
                builder.Services.AddSingleton(emailConfig);

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IEmailConformationService, EmailConformationService>();



            builder.Services.AddDbContext<DataContext>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString"));
                x.EnableSensitiveDataLogging(); //TODO-PROD: remove in production
            });

            builder.Services.AddSingleton(configuration);
            builder.Services.AddScoped<ITokenManager, TokenManager>();

            #region Request mappers
            builder.Services.AddScoped<IRequestMapper<RegisterUserRequestDTO, User>, RegisterUserRequestMapper>();
            #endregion

            #region Response mappers
            builder.Services.AddScoped<IResponseMapper<User, DetailedUserResponseDTO>, DetailedUserResponseMapper>();
            #endregion

            builder.Services.AddScoped<ICreateService<EmailConformation>, CreateService<EmailConformation>>();
            builder.Services.AddScoped<IReadService<EmailConformation>, EmailConformationReadService>();
            builder.Services.AddScoped<IDeleteService<EmailConformation>, DeleteService<EmailConformation>>();
            builder.Services.AddScoped<IEmailConformationService, EmailConformationService>();

            builder.Services.AddScoped<SmtpClient>();

            #region Exercise
            builder.Services.AddScoped<ICreateService<Exercise>, ExerciseCreateService>();
            builder.Services.AddScoped<IReadService<Exercise>, ExerciseReadService>();
            builder.Services.AddScoped<IUpdateService<Exercise>, ExerciseUpdateService>();
            builder.Services.AddScoped<IDeleteService<Exercise>, DeleteService<Exercise>>();
            builder.Services.AddScoped<IEntityMapper<Exercise, ExerciseDTO>, ExerciseMapper>();
            builder.Services.AddScoped<IEntityMapper<Exercise, object>, FullExerciseMapper>();
            builder.Services.AddScoped<IEntityMapperAsync<Exercise, ExerciseDTO>, ExerciseMapper>();
            #endregion

            #region User
            builder.Services.AddScoped<ICreateService<User>, UserCreateService>();
            builder.Services.AddScoped<IReadService<User>, UserReadService>();
            builder.Services.AddScoped<IUpdateService<User>, UpdateService<User>>();
            builder.Services.AddScoped<IDeleteService<User>, DeleteService<User>>();
            builder.Services.AddScoped<IEntityMapper<User, UserDTO>, UserMapper>();
            #endregion

            #region Set
            builder.Services.AddScoped<ICreateService<Set>, CreateService<Set>>();
            builder.Services.AddScoped<IReadService<Set>, SetReadService>();
            builder.Services.AddScoped<IUpdateService<Set>, UpdateService<Set>>();
            builder.Services.AddScoped<IDeleteService<Set>, DeleteService<Set>>();
            builder.Services.AddScoped<IEntityMapper<Set, SetDTO>, SetMapper>();
            #endregion

            #region Workouts
            builder.Services.AddScoped<IReadService<Workout>, WorkoutReadService>();
            builder.Services.AddScoped<ICreateService<Workout>, CreateService<Workout>>();
            builder.Services.AddScoped<IUpdateService<Workout>, WorkoutUpdateService>();
            builder.Services.AddScoped<IDeleteService<Workout>, DeleteService<Workout>>();
            builder.Services.AddScoped<IEntityMapper<Workout, WorkoutDTO>, WorkoutMapper>();
            #endregion

            #region Muscle group
            builder.Services.AddScoped<ICreateService<MuscleGroup>, MuscleGroupCreateService>();
            builder.Services.AddScoped<IReadService<MuscleGroup>, MuscleGroupReadService>();
            builder.Services.AddScoped<IUpdateService<MuscleGroup>, UpdateService<MuscleGroup>>();
            builder.Services.AddScoped<IDeleteService<MuscleGroup>, DeleteService<MuscleGroup>>();
            builder.Services.AddScoped<IEntityMapper<MuscleGroup, MuscleGroupDTO>, MuscleGroupMapper>();
            #endregion

            #region Muscle
            builder.Services.AddScoped<ICreateService<Muscle>, CreateService<Muscle>>();
            builder.Services.AddScoped<IReadService<Muscle>, MuscleReadService>();
            builder.Services.AddScoped<IUpdateService<Muscle>, UpdateService<Muscle>>();
            builder.Services.AddScoped<IDeleteService<Muscle>, DeleteService<Muscle>>();
            builder.Services.AddScoped<IEntityMapper<Muscle, MuscleDTO>, MuscleMapper>();
            #endregion

            #region Equipment exercise usage
            builder.Services.AddScoped<ICreateService<EquipmentUsage>, EquipmentExerciseUsageCreateService>();
            builder.Services.AddScoped<IReadService<EquipmentUsage>, EquipmentExerciseUsageReadService>();
            builder.Services.AddScoped<IDeleteService<EquipmentUsage>, DeleteService<EquipmentUsage>>();
            #endregion

            #region Equipment
            builder.Services.AddScoped<ICreateService<Equipment>, EquipmentCreateService>();
            builder.Services.AddScoped<IReadService<Equipment>, EquipmentReadService>();
            builder.Services.AddScoped<IUpdateService<Equipment>, UpdateService<Equipment>>();
            builder.Services.AddScoped<IDeleteService<Equipment>, DeleteService<Equipment>>();
            builder.Services.AddScoped<IEntityMapper<Equipment, EquipmentDTO>, EquipmentMapper>();
            #endregion

            #region Primary muscle group
            builder.Services.AddScoped<ICreateService<PrimaryMuscleGroupInExercise>, PrimaryMuscleExerciseConnectionCreateService>();
            builder.Services.AddScoped<IReadService<PrimaryMuscleGroupInExercise>, PrimaryMuscleGroupReadService>();
            builder.Services.AddScoped<IDeleteService<PrimaryMuscleGroupInExercise>, DeleteService<PrimaryMuscleGroupInExercise>>();
            #endregion

            #region Secondary muscle group
            builder.Services.AddScoped<IReadService<SecondaryMuscleGroupInExercise>, SecondaryMuscleGroupReadService>();
            builder.Services.AddScoped<ICreateService<SecondaryMuscleGroupInExercise>, SecondaryMuscleExerciseConnectionCreateService>();
            builder.Services.AddScoped<IDeleteService<SecondaryMuscleGroupInExercise>, DeleteService<SecondaryMuscleGroupInExercise>>();
            #endregion

            #region Primary muscle
            builder.Services.AddScoped<IReadService<PrimaryMuscleInExercise>, PrimaryMuscleReadService>();
            builder.Services.AddScoped<IDeleteService<PrimaryMuscleInExercise>, DeleteService<PrimaryMuscleInExercise>>();
            #endregion

            #region Secondary muscle
            builder.Services.AddScoped<IReadService<SecondaryMuscleInExercise>, SecondaryMuscleReadService>();
            builder.Services.AddScoped<IDeleteService<SecondaryMuscleInExercise>, DeleteService<SecondaryMuscleInExercise>>();
            #endregion

            #region RefreshTokens
            builder.Services.AddScoped<ICreateService<RefreshToken>, RefreshTokenCreateService>();
            builder.Services.AddScoped<IReadService<RefreshToken>, RefreshTokenReadService>();
            builder.Services.AddScoped<IUpdateService<RefreshToken>, UpdateService<RefreshToken>>();
            builder.Services.AddScoped<IDeleteService<RefreshToken>, DeleteService<RefreshToken>>();
            #endregion

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseCors();
            serviceProvider = app.Services;

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}