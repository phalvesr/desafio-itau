namespace LetsCodeItau.Dolly.Domain.Validators;

public static class RatingValidator
{
    public static (bool, string) IsRatingValid(int rating)
    {
        const int MIN_RATE = 0;
        const int MAX_RATE = 10;

        var isValid = rating >= MIN_RATE && rating <= MAX_RATE;

        return (isValid, $"Rating must be between {MIN_RATE} and {MAX_RATE}");
    }
}
