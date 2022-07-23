namespace Domain.Common.Out;

public class FilterResultOut<T>
{
    public FilterResultOut(int PageSize, int TotalCount, List<T>? FilterResult)
    {
        this.TotalCount = TotalCount;
        Data = FilterResult ?? new List<T>();
        PagesCount = (int)Math.Ceiling((decimal)TotalCount / PageSize);
    }

    public List<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int PagesCount { get; set; }
}
