using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.Update
{
    public class ExerciseUpdateService(DataContext context, IReadService<Exercise> readService) : IUpdateService<Exercise>
    {
        public async Task Update(Exercise updatedEntity)
        {
            try
            {
                var exercise = await readService.Get(x => x.Id == updatedEntity.Id, "none") ?? throw new NullReferenceException("Exercise was not found.");
                context.AttachRange(exercise);

                exercise.Name = updatedEntity.Name;
                exercise.Description = updatedEntity.Description;
                exercise.Image = updatedEntity.Image;

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }
    }
}
