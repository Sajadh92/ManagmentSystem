//using Infrastructure.Validation.Attribute;

namespace Domain.Common.Filter;

public class PagingFilter
{
    public string OrderBy { get; set; } = "CreateDate";

    //[NumberValidate(Min: 1, Null: true)]
    public int PageNo { get; set; } = 1;

    //[NumberValidate(Min: 1, Max: 100, Null: true)]
    public int PageSize { get; set; } = 20;

    //[RequiredValidate]
    public bool IsDesc { get; set; } = true;
}