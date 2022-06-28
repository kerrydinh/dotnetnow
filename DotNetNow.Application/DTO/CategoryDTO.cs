namespace DotNetNow.Application.DTO
{
    public class CategoryDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public CategoryDTO Parent { get; set; }
    }
}