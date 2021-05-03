using NotepadGps.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotepadGps.Services.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private const string DbPath = "notepadgps.db3";

        private readonly Lazy<SQLiteAsyncConnection> _database;

        public RepositoryService()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbPath);
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<UserModel>().Wait();
                database.CreateTableAsync<MapPinModel>().Wait();
                database.CreateTableAsync<ImageModel>().Wait();
                database.CreateTableAsync<EventModel>().Wait();

                return database;
            });
        }

        #region -- IRepositoryService implementation --

        public async Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.DeleteAsync(entity);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return await _database.Value.Table<T>().ToListAsync();
        }

        public List<T> GetAll<T>() where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().ToListAsync().Result;
        }

        public async Task<T> GetOneAsync<T>(int id) where T : IEntityBase, new()
        {
            return await _database.Value.Table<T>().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.UpdateAsync(entity);
        }

        public async Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> pred) where T : class, IEntityBase, new()
        {
            return await _database.Value.Table<T>().Where(pred).ToListAsync();
        }

        public List<T> Find<T>(Expression<Func<T, bool>> pred) where T : class, IEntityBase, new()
        {
            return _database.Value.Table<T>().Where(pred).ToListAsync().Result;
        }
        public async Task<T> FindUserAsync<T>(Expression<Func<T, bool>> pred) where T : class, IEntityBase, new()
        {
            return await _database.Value.FindAsync<T>(pred);
        }

        #endregion
    }
}
