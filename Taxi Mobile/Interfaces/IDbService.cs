using Realms;
using Taxi_mobile.Models.Db;

namespace Taxi_mobile.Interfaces
{
    public interface IDbService
    {
        public Task AddOrUpdateAsync<T>(T entity) where T : RealmObject;
        public Task<List<T>> GetAllAsync<T>() where T : RealmObject;
        public Task<List<PlaceAutoCompletePredictionEntity>> GetTopByDateAsync(int count);
        public Task ClearDatabaseAsync();
    }
}
