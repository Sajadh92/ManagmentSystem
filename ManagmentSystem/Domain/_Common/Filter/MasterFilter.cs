//using Infrastructure.Validation.Attribute;

namespace Domain.Common.Filter;

public class MasterFilter : PagingFilter
{
    //[DateValidate(Null: true)]
    public DateTime? FromCreateDate { get; set; } = null;

    //[DateValidate(Null: true)]
    public DateTime? ToCreateDate { get; set; } = null;

    //[DateValidate(Null: true)]
    public DateTime? FromUpdateDate { get; set; } = null;

    //[DateValidate(Null: true)]
    public DateTime? ToUpdateDate { get; set; } = null;

    //[DateValidate(Null: true)]
    public DateTime? FromRemoveDate { get; set; } = null;

    //[DateValidate(Null: true)]
    public DateTime? ToRemoveDate { get; set; } = null;

    //[RequiredValidate]
    public bool IsRemoved { get; set; } = false;
}
