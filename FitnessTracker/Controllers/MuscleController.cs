using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;

namespace FitnessTracker.Controllers
{
    public class MuscleController(ICreateService<Muscle> createService, IUpdateService<Muscle> updateService, IDeleteService<Muscle> deleteService, IReadService<Muscle> readService, IEntityMapper<Muscle, MuscleDTO> mapper) : RepositoryController<Muscle, MuscleDTO>(createService, updateService, deleteService, readService, mapper) { }
}