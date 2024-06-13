using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;

namespace FitnessTracker.Controllers
{
    public class EquipmentController(ICreateService<Equipment> createService, IUpdateService<Equipment> updateService, IDeleteService<Equipment> deleteService, IReadService<Equipment> readService, IEntityMapper<Equipment, EquipmentDTO> mapper) : RepositoryController<Equipment, EquipmentDTO>(createService, updateService, deleteService, readService, mapper) { }
}
