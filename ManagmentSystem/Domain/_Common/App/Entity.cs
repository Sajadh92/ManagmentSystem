namespace Domain.Common.App;

public class Entity
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public int CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int? UpdateUserId { get; set; }
    public bool IsRemoved { get; set; }
    public DateTime? RemoveDate { get; set; }
    public int? RemoveUserId { get; set; }
    public byte[]? Version { get; set; }
}
