namespace Application.HumanResources;

public class EmployeeOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public string JobTitle { get; set; }
    public int JobRank { get; set; }
}
