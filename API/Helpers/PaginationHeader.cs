namespace API.Helpers;

public class PaginationHeader(int currentPage, int pageSize, int totalItems, int totalPages)
{
    public int CurrentPage { get; set; } = currentPage;
    public int PageSize { get; set; } = pageSize;
    public int TotalItems { get; set; } = totalItems;
    public int TotalPages { get; set; } = totalPages;
}
