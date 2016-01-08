using System;
using System.Linq;

namespace ViccosLite.Core.Domain.Users
{
    public static class UserExtensions
    {
        public static bool IsRegistered(this User user, bool onlyActiveCustomerRoles = true)
        {
            return IsInUserRole(user, SystemUserRoleNames.Registered, onlyActiveCustomerRoles);
        }

        public static bool IsInUserRole(this User user,
            string userRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(userRoleSystemName))
                throw new ArgumentNullException("userRoleSystemName");

            var result = user.UserRoles
                .FirstOrDefault(
                    cr => (!onlyActiveCustomerRoles || cr.Active) && (cr.SystemName == userRoleSystemName)) != null;
            return result;
        }

        public static bool IsSearchEngineAccount(this User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.IsSystemAccount || String.IsNullOrEmpty(user.SystemName))
                return false;

            var result = user.SystemName.Equals(SystemUserNames.SearchEngine,
                StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        public static bool IsBackgroundTaskAccount(this User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.IsSystemAccount || String.IsNullOrEmpty(user.SystemName))
                return false;

            var result = user.SystemName.Equals(SystemUserNames.BackgroundTask,
                StringComparison.InvariantCultureIgnoreCase);
            return result;
        }
    }
}