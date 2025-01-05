using System.Linq.Expressions;

namespace API.Repository
{
    public interface IGenericRepository<TModel> where TModel : class
    {

        Task<TModel> GetModel(Expression<Func<TModel,bool>>filter);
        Task<TModel> Create(TModel model);
        Task<bool> Edit(TModel model);
        Task<bool> Delete(TModel model);
        IQueryable<TModel> Query(Expression<Func<TModel, bool>> ?filter=null);
    }
}
