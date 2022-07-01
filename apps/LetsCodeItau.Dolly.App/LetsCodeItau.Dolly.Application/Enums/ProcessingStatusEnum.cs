namespace LetsCodeItau.Dolly.Application.Enums;

public enum ProcessingStatusEnum
{
    SuccessfullyProcessed = 1,
    CouldNotFindUser = 2,
    WrongCredentials = 3,
    ServerHadProblemsProcessingRequest = 5,
    UserAlreadyRegistered = 6,
    ExternalDependecyFailed = 7,
    ResourceCreated = 8,
    UserNotAllowedTo = 9,
    CouldNotFindResource = 10
}
