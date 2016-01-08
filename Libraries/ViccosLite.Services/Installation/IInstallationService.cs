namespace ViccosLite.Services.Installation
{
    public interface IInstallationService
    {
        void InstallData(string defaultUserEmail, string defaultUserPassword, bool installSampleData = true);
    }
}