using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ShareOwnerControl.DAL.Base;

namespace ShareOwnerControl.BLL.Base
{
    public interface ILogicBase<TModel>
    {
        Task<IEnumerable<TModel>> Get(Expression<Func<TModel, bool>> filter = null, string[] includeProperties = null);
        Task<TModel> GetOne(Expression<Func<TModel, bool>> filter = null);
        Task<TModel> Insert(TModel entity);
        Task<TModel> Update(TModel entity);
        Task Delete(TModel entity);
    }

    public abstract class LogicBase<TModel> : ILogicBase<TModel> where TModel : class
    {
        protected IDataManagerBase<TModel> _dataManager;

        protected LogicBase(IDataManagerBase<TModel> dataManager)
        {
            _dataManager = dataManager;
        }
        public virtual async Task<IEnumerable<TModel>> Get(Expression<Func<TModel, bool>> filter = null, string[] includeProperties = null)
        {
            return await _dataManager.Get(filter, includeProperties);
        }

        public virtual async Task<TModel> GetOne(Expression<Func<TModel, bool>> filter = null)
        {
            return await _dataManager.GetOne(filter);
        }

        public virtual async Task<TModel> Insert(TModel entity)
        {
            return await _dataManager.Insert(entity);
        }

        public virtual async Task<TModel> Update(TModel entity)
        {
            await _dataManager.Update(entity);
            return entity;
        }
        public virtual async Task Delete(TModel entity)
        {
            await _dataManager.Delete(entity);
        }
    }
}