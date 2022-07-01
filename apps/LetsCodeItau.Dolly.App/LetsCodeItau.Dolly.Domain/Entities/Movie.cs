namespace LetsCodeItau.Dolly.Domain.Entities;

public class Movie
{
    public int MovieId { get; set; }
    public string ImdbId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Plot { get; set; } = default!;
    public int RuntimeMinutes { get; set; }
}
