
namespace CQRSMediatr.DataService.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        
        Task<T?> GetById(int id);
        
        Task<bool> Add(T entity);
        
        Task<bool> Update(T entity);
        
        Task<bool> Delete(Guid id);
    }
}
