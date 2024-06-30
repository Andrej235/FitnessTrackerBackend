using FitnessTracker.DTOs.Responses.AuthTokens;

namespace FitnessTracker.Services.Mapping.Response.AuthTokens
{
    public class SimpleJWTResponseMapper : IResponseMapper<string, SimpleJWTResponseDTO>
    {
        public SimpleJWTResponseDTO Map(string from)
        {
            return new()
            {
                Token = from
            };
        }
    }
}
