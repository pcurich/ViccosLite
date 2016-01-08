using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Core
{
    public interface IWorkContext
    {
        User CurrentUser { get; }
        bool IsAdmin { get; set; }
    }
}