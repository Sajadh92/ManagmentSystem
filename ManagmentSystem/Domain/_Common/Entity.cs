namespace Domain;

public class Entity
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow.AddHours(3);
    public int CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; } = null;
    public int? UpdateUserId { get; set; } = null;
    public bool IsRemoved { get; set; } = false;
    public DateTime? RemoveDate { get; set; } = null;
    public int? RemoveUserId { get; set; } = null;
    public byte[] Version { get; set; }
}
