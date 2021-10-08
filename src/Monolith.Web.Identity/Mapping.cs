using AutoMapper;
using Monolith.Web.Identity.Data.Entities;

namespace Monolith.Web.Identity
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, Models.User>();
        }
    }
}
