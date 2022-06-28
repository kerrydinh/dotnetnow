using DotNetNow.API.Controllers.Base;
using DotNetNow.Application.DTO;
using DotNetNow.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetNow.API.Controllers
{
    [Route("api/category", Name = "Category")]
    [ApiController]
    public class CategoryController : GenericController<CategoryDTO, IGenericService<CategoryDTO>>
    {
        public CategoryController(IGenericService<CategoryDTO> service, ILogger<GenericController<CategoryDTO, IGenericService<CategoryDTO>>> logger) : base(service, logger)
        {}
    }
}