using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;

namespace FitnessTracker.Controllers
{
    public class SetController(ICreateService<Set> createService, IUpdateService<Set> updateService, IDeleteService<Set> deleteService, IReadService<Set> readService, IEntityMapper<Set, SetDTO> mapper) : RepositoryController<Set, SetDTO>(createService, updateService, deleteService, readService, mapper) { }
}