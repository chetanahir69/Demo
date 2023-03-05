
using WebApi.Model;

namespace WebApi.Services
{
    public interface IAuthenticateService
    {
        User Authenticate(string UserName, string Password);
    };
}