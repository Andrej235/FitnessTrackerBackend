namespace FitnessTracker.Services.Create
{
    public interface ICreateRangeService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds entities to database
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        /// <param name="toAdd">Entities to save in the database</param>
        Task<bool> Add(IEnumerable<TEntity> toAdd);
    }
}
