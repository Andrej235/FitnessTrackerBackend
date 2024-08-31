﻿using FitnessTracker.Data;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Create
{
    //Has to exist because RefreshTokens' primary key is not called 'Id'
    public class RefreshTokenCreateService(DataContext context) : ICreateService<RefreshToken>
    {
        private readonly DataContext context = context;

        public async Task<object?> Add(RefreshToken toAdd)
        {
            _ = await context.AddAsync(toAdd);
            _ = await context.SaveChangesAsync();
            return toAdd.Token;
        }
    }
}
