namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public async Task Delete(int id) => await deleteService.Delete(x => x.Id == id);
    }
}
