namespace Identity.Server.Application.Enums;

public enum ProcessingStatusEnum
{
    SuccessfullyProcessed = 1,
    CouldNotFindUser = 2,
    WrongCredentials = 3,
    ServerHadProblemsProcessingRequest = 5,
    UserAlreadyRegistered = 6
}
