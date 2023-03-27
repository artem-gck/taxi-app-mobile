using Realms;
using System.Security.Cryptography;
using System.Text;
using Taxi_mobile.Helpers;
using Taxi_mobile.Interfaces;
using Taxi_mobile.Models.Db;

namespace Taxi_mobile.Services
{
    public class DbService : IDbService
    {
        private readonly RealmConfiguration _realmConfiguration;
        private readonly SHA512 _sha512;

        public DbService()
        {
            _sha512 = SHA512.Create();

            _realmConfiguration = new RealmConfiguration("Environment.SpecialFolder.Personal.AnAbApp.data")
            {
                EncryptionKey = GetKeyFromInstallationId(),
                ShouldDeleteIfMigrationNeeded = false,
                SchemaVersion = 1,
                ShouldCompactOnLaunch = (totalBytes, usedBytes) =>
                {
                    var maxDbSize = 150 * 1024 * 1024;
                    return totalBytes > (ulong)maxDbSize && (double)usedBytes / totalBytes < 0.66;
                }
            };
        }

        public async Task AddOrUpdateAsync<T>(T entity) where T : RealmObject
        {
            using var realm = await GetRealmInstanceAsync();
            await realm.RefreshAsync();

            await realm.WriteAsync(() =>
            {
                realm.Add(entity);
            });
        }

        public async Task<List<T>> GetAllAsync<T>() where T : RealmObject
        {
            var realm = await GetRealmInstanceAsync();
            await realm.RefreshAsync();

            var result = realm.All<T>()
                              .ToList();

            return result;
        }

        public async Task<List<PlaceAutoCompletePredictionEntity>> GetTopByDateAsync(int count)
        {
            var realm = await GetRealmInstanceAsync();
            await realm.RefreshAsync();

            var result = realm.All<PlaceAutoCompletePredictionEntity>()
                              .OrderByDescending(pl => pl.CreatedAt)
                              .ToList()
                              .Take(count)
                              .ToList();

            return result;
        }

        public async Task ClearDatabaseAsync()
        {
            using var realm = await GetRealmInstanceAsync();

            await realm.WriteAsync(
                () => realm.RemoveAll()
            );
        }

        private async Task<Realm> GetRealmInstanceAsync()
        {
            return await Realm.GetInstanceAsync(_realmConfiguration);
        }


        private byte[] GetKeyFromInstallationId()
        {
            if (Preferences.ContainsKey(PrefKeys.InstallationIdKey))
            {
                var id = Preferences.Get(PrefKeys.InstallationIdKey, string.Empty);
    
                var hashValue = _sha512.ComputeHash(Encoding.UTF8.GetBytes(id));

                return hashValue;   
            }

            throw new InvalidOperationException("No instalation key in preferences");
        }
    }
}
