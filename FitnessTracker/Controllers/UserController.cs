using FitnessTracker.Auth;
using FitnessTracker.DTOs.Requests.User;
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
    public partial class UserController(IRequestMapper<RegisterUserRequestDTO, User> registrationMapper,
                                        IResponseMapper<User, DetailedUserResponseDTO> detailedResponseMapper,
                                        ICreateService<User> createService,
                                        IReadSingleService<User> readSingleService,
                                        ITokenManager tokenManager,
                                        IEmailConfirmationSenderService emailConfirmationSender,
                                        IEmailConfirmationService emailConfirmationService,
                                        IResetPasswordEmailSenderService passwordResetEmailSender,
                                        IResetPasswordService passwordResetEmailService,
                                        IUpdateService<User> updateService) : ControllerBase
    {
        private readonly IRequestMapper<RegisterUserRequestDTO, User> registrationMapper = registrationMapper;
        private readonly IResponseMapper<User, DetailedUserResponseDTO> detailedResponseMapper = detailedResponseMapper;
        private readonly ICreateService<User> createService = createService;
        private readonly IReadSingleService<User> readSingleService = readSingleService;
        private readonly ITokenManager tokenManager = tokenManager;
        private readonly IEmailConfirmationSenderService emailConfirmationSender = emailConfirmationSender;
        private readonly IEmailConfirmationService emailConfirmationService = emailConfirmationService;
        private readonly IResetPasswordEmailSenderService passwordResetEmailSender = passwordResetEmailSender;
        private readonly IResetPasswordService passwordResetEmailService = passwordResetEmailService;
        private readonly IUpdateService<User> updateService = updateService;

        [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        private static partial Regex ValidEmailRegex();
    }
}
