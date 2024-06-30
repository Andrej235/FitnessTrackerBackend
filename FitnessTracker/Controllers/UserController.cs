using FitnessTracker.Auth;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Responses.AuthTokens;
using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Services.Update;
using FitnessTracker.Services.UserServices.EmailConfirmationSenderService;
using FitnessTracker.Services.UserServices.EmailConfirmationService;
using FitnessTracker.Services.UserServices.ResetPasswordSenderService;
using FitnessTracker.Services.UserServices.ResetPasswordService;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FitnessTracker.Controllers
{
    [Route("api/user")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class UserController(ICreateService<User> createService,
                                        ICreateService<CompletedWorkout> completedWorkoutcreateService,
                                        IReadSingleService<User> readSingleService,
                                        IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService,
                                        IUpdateService<User> updateService,
                                        ITokenManager tokenManager,
                                        IEmailConfirmationSenderService emailConfirmationSender,
                                        IEmailConfirmationService emailConfirmationService,
                                        IResetPasswordEmailSenderService passwordResetEmailSender,
                                        IResetPasswordService passwordResetEmailService,
                                        IRequestMapper<RegisterUserRequestDTO, User> registrationMapper,
                                        IResponseMapper<User, DetailedUserResponseDTO> detailedResponseMapper,
                                        IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO> simpleWeekOfCompletedWorkoutsResponseMapper,
                                        IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO> detailedWeekOfCompletedWorkoutsResponseMapper,
                                        IResponseMapper<string, SimpleJWTResponseDTO> jwtResponseMapper) : ControllerBase
    {
        private readonly ICreateService<User> createService = createService;
        private readonly ICreateService<CompletedWorkout> completedWorkoutcreateService = completedWorkoutcreateService;
        private readonly IReadSingleService<User> readSingleService = readSingleService;
        private readonly IReadRangeService<CompletedWorkout> completedWorkoutReadRangeService = completedWorkoutReadRangeService;
        private readonly IUpdateService<User> updateService = updateService;
        private readonly ITokenManager tokenManager = tokenManager;
        private readonly IEmailConfirmationSenderService emailConfirmationSender = emailConfirmationSender;
        private readonly IEmailConfirmationService emailConfirmationService = emailConfirmationService;
        private readonly IResetPasswordEmailSenderService passwordResetEmailSender = passwordResetEmailSender;
        private readonly IResetPasswordService passwordResetEmailService = passwordResetEmailService;
        private readonly IRequestMapper<RegisterUserRequestDTO, User> registrationMapper = registrationMapper;
        private readonly IResponseMapper<User, DetailedUserResponseDTO> detailedResponseMapper = detailedResponseMapper;
        private readonly IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO> simpleWeekOfCompletedWorkoutsResponseMapper = simpleWeekOfCompletedWorkoutsResponseMapper;
        private readonly IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO> detailedWeekOfCompletedWorkoutsResponseMapper = detailedWeekOfCompletedWorkoutsResponseMapper;
        private readonly IResponseMapper<string, SimpleJWTResponseDTO> jwtResponseMapper = jwtResponseMapper;

        [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        private static partial Regex ValidEmailRegex();
    }
}
