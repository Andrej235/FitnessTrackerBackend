using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class SimpleSplitCommentResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<SplitComment, SimpleSplitCommentResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleSplitCommentResponseDTO Map(SplitComment from)
        {
            return new()
            {
                Id = from.Id,
                Text = from.Text,
                CreatedAt = from.CreatedAt,
                Creator = userResponseMapper.Map(from.Creator),
                LikeCount = from.Likes.Count,
            };
        }
    }
}
