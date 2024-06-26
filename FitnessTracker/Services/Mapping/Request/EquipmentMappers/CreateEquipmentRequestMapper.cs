﻿using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.EquipmentMappers
{
    public class CreateEquipmentRequestMapper : IRequestMapper<CreateEquipmentRequestDTO, Equipment>
    {
        public Equipment Map(CreateEquipmentRequestDTO from)
        {
            return new()
            {
                Name = from.Name,
            };
        }
    }
}
