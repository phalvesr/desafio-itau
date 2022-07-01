using AutoMapper;
using LetsCodeItau.Dolly.Application.Models.Requests.Comments;
using LetsCodeItau.Dolly.Application.Models.Requests.Users;
using LetsCodeItau.Dolly.Application.Models.Responses;
using LetsCodeItau.Dolly.Application.Requests;
using LetsCodeItau.Dolly.Application.Responses.Omdb.SearchMovieInformationByTitle;
using LetsCodeItau.Dolly.Domain.Entities;

namespace LetsCodeItau.Dolly.Application.Mappers;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<CreateRequest, CreateUserRequest>();
        CreateMap<LoginRequest, UserSignInRequest>();
        CreateMap<CreateCommentRequest, Movie>();
        CreateMap<User, UserDataResponse>();

        CreateMap<SearchMovieInformationByTitleResponseDto, Movie>()
            .ForMember(d => d.RuntimeMinutes, o => o.MapFrom(x => x.Runtime == "N/A" ? "0" : x.Runtime.Replace(" min", string.Empty)));
    }
}
