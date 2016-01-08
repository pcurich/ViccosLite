using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Authentication
{
    public interface IAuthenticationService
    {
        void SignIn(User user, bool createPersistentCookie);
        void SignOut();
        User GetAuthenticatedUser();
    }
}