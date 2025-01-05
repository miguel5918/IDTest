using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Repository
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly MessageDbContext _dbContext;
        public GenericRepository(MessageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TModel> GetModel(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                TModel? model = await _dbContext.Set<TModel>().AsNoTracking().FirstOrDefaultAsync(filter);
                    
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<TModel> Create(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Add(model);
                await _dbContext.SaveChangesAsync();
                return  model ;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Edit(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Update(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

       
        public async Task<bool> Delete(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Remove(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public  IQueryable<TModel> Query(Expression<Func<TModel, bool>> ?filter = null)
        {
            try
            {
               var queryModel = filter == null ? _dbContext.Set<TModel>().AsNoTracking() : _dbContext.Set<TModel>().Where(filter).AsNoTracking();
                return queryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }




        //public IQueryable<TModel> Query(Expression<Func<TModel, bool>> filter = null)
        //{
        //    try
        //    {
        //        IQueryable<TModel> queryModel = filter == null ? _dbContext.Set<TModel>() : _dbContext.Set<TModel>().Where(filter);
        //        return queryModel;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //Task<IQueryable<TModel>> IGenericRepository<TModel>.Query(Expression<Func<TModel, bool>> filter)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
