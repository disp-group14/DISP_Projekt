using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaxService.Models;

namespace TaxService.DAL.Base
{
    public interface IDataManagerBase<TModel> where TModel : class
    {
        Task<List<TModel>> Get(Expression<Func<TModel, bool>> filter = null, string[] includeProperties = null);
        Task<TModel> GetOne(Expression<Func<TModel, bool>> filter = null, string[] includeProperties = null);
        Task<TModel> Insert(TModel entity);
        Task Update(TModel entity);
        Task Delete(TModel entity);
    }

    public abstract class DataManagerBase<TModel> : IDataManagerBase<TModel> where TModel : class
    {
        protected readonly TaxServiceDbContext _context;
        protected DbSet<TModel> _entities;

        protected DataManagerBase(TaxServiceDbContext dataContext)
        {
            this._context = dataContext;
            this._entities = this._context.Set<TModel>();
        }

        public virtual async Task<List<TModel>> Get(Expression<Func<TModel, bool>> filter = null, string[] includeProperties = null)
        {
            IQueryable<TModel> query = _entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();

        }

        public virtual async Task<TModel> GetOne(Expression<Func<TModel, bool>> filter = null, string[] includeProperties = null)
        {
            IQueryable<TModel> query = _entities;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter == null)
            {
                return await query.FirstOrDefaultAsync();
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<TModel> Insert(TModel entity)
        {
            try
            {
                this._entities.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException is SqlException se && se.Number == 2627)
                {
                    Type typeParameterType = typeof(TModel);
                    throw new ApiException("PRIMARY_KEY_ALREADY_EXIST_" + typeParameterType.Name);
                }
                throw;
            }
            return entity;
        }

        public virtual async Task Update(TModel entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task Delete(TModel entity)
        {
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}