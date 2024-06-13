using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;

namespace FitnessTracker.Controllers
{
    public class WorkoutController(ICreateService<Workout> createService, IUpdateService<Workout> updateService, IDeleteService<Workout> deleteService, IReadService<Workout> readService, IEntityMapper<Workout, WorkoutDTO> mapper) : RepositoryController<Workout, WorkoutDTO>(createService, updateService, deleteService, readService, mapper) { }
}
