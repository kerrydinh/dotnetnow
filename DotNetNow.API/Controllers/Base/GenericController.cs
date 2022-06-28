using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetNow.API.Model;
using DotNetNow.Application.Common;
using DotNetNow.Application.DTO;
using DotNetNow.Application.Interface;

namespace DotNetNow.API.Controllers.Base
{
    public class GenericController<T, IService> : BaseController where IService: IGenericService<T>
    {
        private readonly IService _service;
        private readonly ILogger<GenericController<T, IService>> _logger;
        public GenericController(
            IService service,
            ILogger<GenericController<T, IService>> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        [Route("")]
        public async Task<Pagination<T>> Get([FromQuery] ListQueryModel query)
        {
            var result = await _service.Get(query.PageSize, query.PageNumber);
            return new Pagination<T> {
                Data = result,
                Page = query.PageNumber,
                PageSize = query.PageSize
            };
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<T> Get([FromRoute] int id)
        {
            return await _service.Get(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<int> Add(T payload)
        {
            return await _service.Add(payload);
        }
        
        [HttpPut]
        [Route("")]
        public async Task Update(T payload)
        {
            await _service.Update(payload);
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public async Task Remove([FromRoute] int id)
        {
            await _service.Remove(id);
        }
    }
}