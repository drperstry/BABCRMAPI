namespace BabCrm.Service.Models
{
    public class CollectionResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public CollectionResult(IEnumerable<T> items, int totalCount, int pageSize, int currentPage)
        {
            Items = items ?? Enumerable.Empty<T>();

            TotalCount = totalCount;

            PageSize = pageSize;

            CurrentPage = currentPage;

            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            HasNextPage = CurrentPage < TotalPages;

            HasPreviousPage = CurrentPage > 1;
        }
    }
}
