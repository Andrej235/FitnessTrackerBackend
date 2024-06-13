using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;

namespace FitnessTracker.Controllers
{
    public class MuscleGroupController(ICreateService<MuscleGroup> createService, IUpdateService<MuscleGroup> updateService, IDeleteService<MuscleGroup> deleteService, IReadService<MuscleGroup> readService, IEntityMapper<MuscleGroup, MuscleGroupDTO> mapper) : RepositoryController<MuscleGroup, MuscleGroupDTO>(createService, updateService, deleteService, readService, mapper) { }
}