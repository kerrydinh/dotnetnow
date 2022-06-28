namespace DotNetNow.Application.Common
{
    public class ListQueryModel
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 0;
        public string Name { get; set; }
        public string SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}