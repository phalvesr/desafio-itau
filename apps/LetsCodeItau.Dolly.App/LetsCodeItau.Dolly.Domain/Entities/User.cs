namespace LetsCodeItau.Dolly.Domain.Entities;

public class User
{
    public int Points { get; set; }
    public string Username { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public int UserId { get; set; }
    public string GlobalId { get; set; } = default!;
    public DateTime RegisterDate { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool Deleted { get; set; } = false;

    public virtual ICollection<Comment> Comments { get; set; } = default!;
    public virtual ICollection<Rating> Ratings { get; set; } = default!;
}
