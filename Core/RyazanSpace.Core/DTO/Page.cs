using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Core.DTO
{
    public class Page<T> : IPage<T>
    {
        public Page() { }
        public Page(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize)
        {
            this.Items = Items;
            this.TotalCount = TotalCount;
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
        }

        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
