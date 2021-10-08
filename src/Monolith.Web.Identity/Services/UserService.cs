using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Monolith.Core;
using Monolith.Core.Extensions;
using Monolith.Web.Identity.Data.Entities;
using Monolith.Web.Identity.Requests.Users;

namespace Monolith.Web.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        public UserService(
            IMapper mapper,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public Result<IEnumerable<Models.User>> GetAll(GetAllRequest req)
        {
            var users = _userManager.Users.OrderBy(user => user.NormalizedUserName)
                .Paginate(req.PageNumber, req.PageSize);

            return Result<IEnumerable<Models.User>>
                .Success(_mapper.Map<IEnumerable<Models.User>>(users));
        }

        public async Task<Result<Models.User>> GetByNameAsync(GetByNameRequest req)
        {
            var user = await _userManager.FindByNameAsync(req.Username).ConfigureAwait(false);

            if (user == null)
            {
                return Result<Models.User>.Failure(Errors.UserNotFound);
            }

            return Result<Models.User>
                .Success(_mapper.Map<Models.User>(user));
        }
    }
}
