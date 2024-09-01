namespace FitnessTracker.Services.Create
{
    public interface ICreateService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds entity to database
        /// </summary>
        /// <returns>Added entity with its new primary key</returns>
        /// <param name="toAdd">Entity to save in the database</param>
        /// <exception cref="OperationCanceledException"/>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException"/>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException"/>
        Task<TEntity> Add(TEntity toAdd);
    }
}
