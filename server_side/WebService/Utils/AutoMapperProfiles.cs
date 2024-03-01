using AutoMapper;
using Seventy.Common.Model;
using SwissPension.SP7.Common;

namespace Seventy.WebService.Utils;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, LoginResponse>();
    }
}
