namespace ViccosLite.Services.Users
{
    public class ChangePasswordRequest
    {
        public ChangePasswordRequest(string email, bool validateRequest,
            string newPassword, string oldPassword = "")
        {
            Email = email;
            ValidateRequest = validateRequest;
            NewPassword = newPassword;
            OldPassword = oldPassword;
        }

        public string Email { get; set; }
        public bool ValidateRequest { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}