using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.SplitMappers
{
    public class CreateSplitCommentRequestMapper : IRequestMapper<CreateSplitCommentRequestDTO, SplitComment>
    {
        public SplitComment Map(CreateSplitCommentRequestDTO from)
        {
            return new()
            {
                Text = from.Comment,
                CreatedAt = DateTime.Now,
            };
        }
    }
}
