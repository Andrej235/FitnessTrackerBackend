using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.MuscleGroupService
{
    public partial class MuscleGroupService
    {
        public Task Delete(int id)
        {
            if (id < 1)
                throw new InvalidArgumentException("Muscle group id must be greater than 0.");

            return deleteService.Delete(x => x.Id == id);
        }
    }
}
