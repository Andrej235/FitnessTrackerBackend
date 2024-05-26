﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectGym.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;

namespace FitnessTracker.Auth
{
    public class AllowExpiredAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ConfigurationManager configuration) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        private readonly ConfigurationManager configuration = configuration;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.NoResult();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                if (validatedToken is not JwtSecurityToken jwtToken)
                    return AuthenticateResult.Fail("Invalid token");

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
