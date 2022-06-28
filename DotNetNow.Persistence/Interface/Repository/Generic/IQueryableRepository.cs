using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetNow.Persistence.Interface
{
    public interface IQueryableRepository<T> : IGenericRepository<T>
    {
        Task<IList<T>> Get(string name, int pageSize = 10, int pageNumber = 1);
    }
}