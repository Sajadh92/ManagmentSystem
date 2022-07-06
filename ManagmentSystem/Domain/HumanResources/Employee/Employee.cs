namespace Domain.HumanResources;

public class Employee : Entity
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Phone { get; set; }
    public DateTime? Birthday { get; set; }
    public string JobTitle { get; set; }
    public int JobRank { get; set; }
}
