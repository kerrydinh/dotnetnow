using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotNetNow.Application.Interface;
using DotNetNow.Persistence.Interface;

namespace DotNetNow.Application.Main.Generic
{
    public class GenericService<TModel, TEntity, IRepository> : IGenericService<TModel> where IRepository : IGenericRepository<TEntity>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        
        public GenericService(
            IRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<IList<TModel>> Get(int pageSize = 10, int pageNumber = 1)
        {
            var result = await _repository.Get(pageSize, pageNumber);
            return _mapper.Map<IList<TEntity>, IList<TModel>>(result);
        }

        public async Task<TModel> Get(int id)
        {
            var result = await _repository.GetById(id);
            return _mapper.Map<TEntity, TModel>(result);
        }

        public async Task<int> Add(TModel payload)
        {
            var record = _mapper.Map<TModel, TEntity>(payload);
            return await _repository.Insert(record);
        }

        public async Task Update(TModel payload)
        {
            var record = _mapper.Map<TModel, TEntity>(payload);
            await _repository.Update(record);
        }
        

        public async Task Remove(int id)
        {
            await _repository.Delete(id);
        }

        public async Task Disable(int id)
        {
            await _repository.Disable(id);
        }
        
        public async Task Enable(int id)
        {
            await _repository.Enable(id);
        }

        public async Task Hide(int id)
        {
            await _repository.Hide(id);
        }

        public async Task UnHide(int id)
        {
            await _repository.UnHide(id);
        }
    }
}