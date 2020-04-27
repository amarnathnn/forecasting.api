

using PS.CTS.Common.Entities;

namespace PS.CTS.Authentication.Repository
{
    public interface IAuthenticationRepository
    {
        LoginInfo Authenticate(string userId, string password);


    }
}
