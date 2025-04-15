namespace ApplicationLayer.DTOs.Persistence
{
    public class PaginationResult<T>(int currentPage, int pages, int count, ICollection<T> data)
    {
        public int CurrentPage { get; internal set; } = currentPage;
        public int Pages { get; internal set; } = pages;
        public int QuantityRecords { get; internal set; } = count;
        public ICollection<T> Data { get; internal set; } = data;
    }
}
