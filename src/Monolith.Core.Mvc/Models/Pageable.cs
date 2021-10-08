namespace Monolith.Core.Mvc.Models
{
    public abstract class Pageable
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 50;
    }
}
