namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record GetCommentsResponse
(
    IEnumerable<GetCommentsData> Comments,
    int PagesCount,
    int LastIndex,
    int FetchCount
);
