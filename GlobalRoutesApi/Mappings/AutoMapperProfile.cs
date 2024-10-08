using AutoMapper;
using GlobalRoutes.Api.Requests.Account;
using GlobalRoutes.Core.Entities.Users;

namespace GlobalRoutes.Api.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            //User
            CreateMap<User, PostUserRequest>().ReverseMap();

        }
    }
}
