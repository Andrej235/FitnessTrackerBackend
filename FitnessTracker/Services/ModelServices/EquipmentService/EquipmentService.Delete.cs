using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.EquipmentService
{
    public partial class EquipmentService
    {
        public async Task Delete(int id)
        {
            if (id < 1)
                throw new InvalidArgumentException(nameof(id) + " cannot be less than 1");

            await deleteService.Delete(x => x.Id == id);
        }
    }
}
