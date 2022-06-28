using System.Threading.Tasks;
using DotNetNow.Domain.Entity;

namespace DotNetNow.Auth.UserService
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        void SetUser(AppUser user);
        AppUser GetUser();
    }
}