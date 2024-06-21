using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Read.ExpressionBased;

namespace FitnessTracker.Services.Update
{
    public class WorkoutUpdateService(DataContext context, IReadSingleService<Workout> readService) : IUpdateService<Workout>
    {
        private readonly IReadSingleService<Workout> readService = readService;

        public async Task Update(Workout updatedEntity)
        {
            var entity = await readService.Get(x => x.Id == updatedEntity.Id, "none") ?? throw new NullReferenceException("Entity not found");
            context.AttachRange(entity);
            entity.Name = updatedEntity.Name;
            entity.IsPublic = updatedEntity.IsPublic;
            await context.SaveChangesAsync();
        }
    }
}
