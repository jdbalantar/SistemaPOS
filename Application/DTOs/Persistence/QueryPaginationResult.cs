namespace ApplicationLayer.DTOs.Persistence
{
    public class QueryPaginationResult<T>(int currentPage, int pages, int count, IQueryable<T> data)
    {
        public int CurrentPage { get; internal set; } = currentPage;
        public int Pages { get; internal set; } = pages;
        public int QuantityRecords { get; internal set; } = count;
        public IQueryable<T> Data { get; internal set; } = data;
    }
}
