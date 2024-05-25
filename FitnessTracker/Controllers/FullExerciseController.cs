using Microsoft.AspNetCore.Mvc;
using ProjectGym;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace FitnessTracker.Controllers
{
    [Route("api/fullexercise")]
    [ApiController]
    public class FullExerciseController(IReadService<Exercise> readService) : ControllerBase
    {
        public IReadService<Exercise> ReadService { get; } = readService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var exercises = await ReadService.Get(q, offset, limit, include);
            //await Task.Delay(2000);

            return Ok(exercises.Select(Map).ToList());
        }

        private object Map(Exercise entity)
        {
            var muscleGroupMapper = entity.PrimaryMuscleGroups.Any() || entity.SecondaryMuscleGroups.Any() ? Program.GetService(typeof(IEntityMapper<MuscleGroup, MuscleGroupDTO>)) as IEntityMapper<MuscleGroup, MuscleGroupDTO> : null;
            var muscleMapper = entity.PrimaryMuscles.Any() || entity.SecondaryMuscles.Any() ? Program.GetService(typeof(IEntityMapper<Muscle, MuscleDTO>)) as IEntityMapper<Muscle, MuscleDTO> : null;

            return new
            {
                entity.Id,
                entity.Name,
                PrimaryMuscleGroups = muscleGroupMapper is null ? [] : entity.PrimaryMuscleGroups.Select(muscleGroupMapper.Map),
                SecondaryMuscleGroups = muscleGroupMapper is null ? [] : entity.SecondaryMuscleGroups.Select(muscleGroupMapper.Map),
                PrimaryMuscles = muscleMapper is null ? [] : entity.PrimaryMuscles.Select(muscleMapper.Map),
                SecondaryMuscles = muscleMapper is null ? [] : entity.SecondaryMuscles.Select(muscleMapper.Map),
                Equipment = !entity.Equipment.Any() || Program.GetService(typeof(IEntityMapper<Equipment, EquipmentDTO>)) is not IEntityMapper<Equipment, EquipmentDTO> equipmentMapper ? [] : entity.Equipment.Select(equipmentMapper.Map),
                Images = !entity.Images.Any() || Program.GetService(typeof(IEntityMapper<Image, ImageDTO>)) is not IEntityMapper<Image, ImageDTO> imageMapper ? [] : entity.Images.Select(imageMapper.Map),
                Notes = !entity.Notes.Any() || Program.GetService(typeof(IEntityMapper<Note, NoteDTO>)) is not IEntityMapper<Note, NoteDTO> noteMapper ? [] : entity.Notes.Select(noteMapper.Map),
                Aliases = !entity.Aliases.Any() || Program.GetService(typeof(IEntityMapper<Alias, AliasDTO>)) is not IEntityMapper<Alias, AliasDTO> aliasMapper ? [] : entity.Aliases.Select(aliasMapper.Map),
                //Bookmarks = !entity.Bookmarks.Any() || Program.serviceProvider.GetService(typeof(IEntityMapper<User, UserDTO>)) is not IEntityMapper<User, UserDTO> bookmarkMapper ? [] : entity.Bookmarks.Select(bookmarkMapper.Map)
            };
        }
    }
}
