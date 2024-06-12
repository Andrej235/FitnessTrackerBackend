﻿using ProjectGym.Data;
using ProjectGym.Exceptions;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class UserCreateService(ExerciseContext context, IReadService<User> readService) : CreateService<User>(context)
    {
        protected override async Task<Exception?> IsEntityValid(User entity)
        {
            if (await readService.Get(eq => eq.Email == entity.Email, "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
