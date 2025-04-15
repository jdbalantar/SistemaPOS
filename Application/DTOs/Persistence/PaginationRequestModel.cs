namespace ApplicationLayer.DTOs.Persistence
{
    public class PaginationRequestModel
    {
        public int Page { get; set; }
        public float Limit { get; set; }
        public required FilterModel[] Filters { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
    }
}
