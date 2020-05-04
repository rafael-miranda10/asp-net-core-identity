using Auth.Models;
using Auth.ViewModels;
using AutoMapper;

namespace Auth.Mapping
{
    public class IdentityMeppingProfile : Profile
    {
        public IdentityMeppingProfile()
        {
            UserMapping();
        }

        private void UserMapping()
        {
            CreateMap<UserRegistrationViewModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
