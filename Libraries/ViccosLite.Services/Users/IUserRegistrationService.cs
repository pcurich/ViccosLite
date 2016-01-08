using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Users
{
    public interface IUserRegistrationService
    {
        UserLoginResults ValidateUser(string usernameOrEmail, string password);
        UserRegistrationResult RegisterUser(UserRegistrationRequest request);
        PasswordChangeResult ChangePassword(ChangePasswordRequest request);
        void SetEmail(User user , string newEmail);
        void SetUsername(User user, string newUsername);
    }
}