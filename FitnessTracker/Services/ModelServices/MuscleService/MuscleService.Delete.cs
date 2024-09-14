using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.MuscleService
{
    public partial class MuscleService
    {
        public Task Delete(int id)
        {
            if (id < 1)
                throw new InvalidArgumentException(nameof(id) + " cannot be less than 1");

            return deleteService.Delete(x => x.Id == id);
        }
    }
}
