using ViccosLite.Core.Domain.Sales;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Users
{
    public class UserRegistrationRequest
    {
        public UserRegistrationRequest(User user, string email, string username, string password,
            bool isApproved = true)
        {
            User = user;
            Email = email;
            Username = username;
            Password = password;
            IsApproved = isApproved;
        }

        public User User { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsApproved { get; set; }
    }
}