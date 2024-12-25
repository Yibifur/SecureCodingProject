﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly Context _context;

        public GenericRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var values = _context.Set<T>().Find(id);
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values), "Entity not found");
            }
            _context.Set<T>().Remove(values);
           await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
             _context.Set<T>().Update(entity);
           await _context.SaveChangesAsync();
        }
    }
}
