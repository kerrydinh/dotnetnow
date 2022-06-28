using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetNow.Application.Interface
{
    public interface IGenericService<TModel>
    {
        Task<IList<TModel>> Get(int pageSize = 10, int pageNumber = 1);

        Task<TModel> Get(int id);

        Task<int> Add(TModel novel);

        Task Update(TModel novel);

        Task Remove(int id);

        Task Disable(int id);

        Task Enable(int id);
        Task Hide(int id);
        Task UnHide(int id);
    }
}