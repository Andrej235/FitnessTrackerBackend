namespace FitnessTracker.Services.Create
{
    public interface ICreateRangeService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds entities to database
        /// </summary>
        /// <param name="toAdd">Entities to save in the database</param>
        /// <exception cref="OperationCanceledException"/>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException"/>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException"/>
        Task Add(IEnumerable<TEntity> toAdd);
    }
}
