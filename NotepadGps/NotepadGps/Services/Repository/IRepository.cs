﻿using NotepadGps.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotepadGps.Services.Repository
{

    public interface IRepository 
    {
        Task<int> InsertAsync<T>(T entity) where T : new();
        Task<int> UpdateAsync<T>(T entity) where T : new();
        Task<int> DeleteAsync<T>(T entity) where T : new();
        Task<List<T>> GetAllAsync<T>() where T : new();
        List<T> GetAll<T>() where T : new();
        Task<T> GetOneAsync<T>(int Id) where T : IEntityBase, new();
        Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> pred = null) where T : class, new();
        List<T> Find<T>(Expression<Func<T, bool>> pred) where T : class, new();


    }
}
