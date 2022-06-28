using DotNetNow.API.Controllers.Base;
using DotNetNow.Application.DTO;
using DotNetNow.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetNow.API.Controllers
{
    [Route("api/product", Name = "Product")]
    [ApiController]
    public class ProductController : GenericController<ProductDTO, IGenericService<ProductDTO>>
    {
        public ProductController(IGenericService<ProductDTO> service, ILogger<GenericController<ProductDTO, IGenericService<ProductDTO>>> logger) : base(service, logger)
        {}
    }
}