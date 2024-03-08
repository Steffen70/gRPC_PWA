using AutoMapper;
using Seventy.Common.Model;
using Seventy.Common;

namespace Seventy.WebService.Utils;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, LoginResponse>();
    }
}
