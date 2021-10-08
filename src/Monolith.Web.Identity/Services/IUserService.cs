using System.Collections.Generic;
using System.Threading.Tasks;
using Monolith.Core;
using Monolith.Web.Identity.Models;
using Monolith.Web.Identity.Requests.Users;

namespace Monolith.Web.Identity.Services
{
    public interface IUserService
    {
        Result<IEnumerable<User>> GetAll(GetAllRequest req);

        Task<Result<User>> GetByNameAsync(GetByNameRequest req);
    }
}
