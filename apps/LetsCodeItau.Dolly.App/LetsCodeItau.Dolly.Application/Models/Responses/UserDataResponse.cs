namespace LetsCodeItau.Dolly.Application.Models.Responses;

public record UserDataResponse
(
    int UserId,
    int Points,
    string Username,
    string DisplayName,
    string GlobalId,
    DateTime RegisterDate
);
