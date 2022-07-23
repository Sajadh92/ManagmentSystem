using Domain.Common.Out;

namespace Domain.HumanResources;

public class EmployeeDataOut : DataOut
{
    public string Name { get; set; } 
    public string Gender { get; set; }
    public string Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public string JobTitle { get; set; } 
    public int JobRank { get; set; }
}
