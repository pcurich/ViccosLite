using System.Collections.Generic;
using ViccosLite.Core.Configuration;

namespace ViccosLite.Core.Domain.Settings.Security
{
    public class SecuritySettings : ISettings
    {
        public List<string> AdminAreaAllowedIpAddresses { get; set; }
    }
}