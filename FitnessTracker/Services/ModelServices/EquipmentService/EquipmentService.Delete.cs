using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.EquipmentService
{
    public partial class EquipmentService
    {
        public Task Delete(int id)
        {
            if (id < 1)
                throw new InvalidArgumentException(nameof(id) + " cannot be less than 1");

            return deleteService.Delete(x => x.Id == id);
        }
    }
}
