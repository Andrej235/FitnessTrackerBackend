using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class ExerciseCreateService(ExerciseContext context, IReadService<Exercise> readService) : ICreateService<Exercise>
    {
        public async Task<object?> Add(Exercise toAdd)
        {
            if (await readService.Get(x => x.Name.ToLower().Equals(toAdd.Name.ToLower()), "none") != null)
                return default;

            try
            {
                context.AttachRange(toAdd.Equipment);
                context.AttachRange(toAdd.PrimaryMuscleGroups);
                context.AttachRange(toAdd.SecondaryMuscleGroups);
                context.AttachRange(toAdd.PrimaryMuscles);
                context.AttachRange(toAdd.SecondaryMuscles);

                await context.Exercises.AddAsync(toAdd);
                await context.SaveChangesAsync();
                return toAdd.Id;
            }
            catch (Exception ex)
            {
                ex.LogError();
                return default;
            }
        }
    }
}
