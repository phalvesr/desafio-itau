using AutoMapper;
using LetsCodeItau.Dolly.Application.Core;
using LetsCodeItau.Dolly.Application.Enums;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Application.Interfaces.UseCases;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.UseCases;

public class FindUserDataAsyncUseCase : IFindUserDataAsyncUseCase
{
    private readonly IUserRepository userRepository;
    private readonly IRepositoryBase<User> userBaseRepository;
    private readonly IMapper mapper;

    public FindUserDataAsyncUseCase(
        IRepositoryBase<User> userBaseRepository,
        IMapper mapper)
    {
        this.userBaseRepository = userBaseRepository;
        this.mapper = mapper;
    }

    public async Task<Notification<UserDataResponse>> ExecuteAsync(string userGlobalId)
    {
        var user = await userBaseRepository.SelectWhere(u => u.GlobalId == userGlobalId && !u.Deleted);

        if (user is null)
        {
            return Notification<UserDataResponse>.Create(null!, ProcessingStatusEnum.CouldNotFindUser, "User not found");
        }

        var viewModel = mapper.Map<UserDataResponse>(user);
        return Notification<UserDataResponse>.Create(viewModel, ProcessingStatusEnum.SuccessfullyProcessed);
    }
}
