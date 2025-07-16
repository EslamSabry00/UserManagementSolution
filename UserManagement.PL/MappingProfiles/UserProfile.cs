using AutoMapper;
using UserManagement.DAL.Models;
using UserManagement.PL.ViewModels;

namespace UserManagement.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() { 
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
        
    }
}
