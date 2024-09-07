namespace FitnessTracker.Services.Update
{
    public interface IUpdateRangeService<T> where T : class
    {
        /// <summary>
        /// Updates the provided entities in the database
        /// <br/>The provided entities MUST have the same primary key
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        Task Update(IEnumerable<T> updatedEntities);
    }
}
