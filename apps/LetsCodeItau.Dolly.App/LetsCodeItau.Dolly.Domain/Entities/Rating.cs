namespace LetsCodeItau.Dolly.Domain.Entities;

public class Rating
{
    public int RatingId { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public double Rate { get; set; }

}
