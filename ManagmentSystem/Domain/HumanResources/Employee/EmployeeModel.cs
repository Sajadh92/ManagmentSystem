using ManagmentSystem.Infrastructure.Validation.Attribute;

namespace Domain.HumanResources;

public class EmployeeModel
{
    public int Id { get; set; }

    [StringValidate(max: 75)]
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Phone { get; set; }
    public DateTime? Birthday { get; set; }

    [StringValidate(max: 50, isnull: true)]
    public string? JobTitle { get; set; }
    public int JobRank { get; set; }
}
