namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public Task Delete(int id) => deleteService.Delete(x => x.Id == id);
    }
}
