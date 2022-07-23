using Domain.Common.Filter;

namespace Domain.HumanResources;

public class EmployeeFilter : MasterFilter
{
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public DateTime? FromBirthday { get; set; }
    public DateTime? ToBirthday { get; set; }
    public string? JobTitle { get; set; }
    public int? FromJobRank { get; set; }
    public int? ToJobRank { get; set; }
}
