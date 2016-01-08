using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using ViccosLite.Core;

namespace ViccosLite.Framework.Security
{
    public static class FilePermissionHelper
    {
        /// <summary>
        ///     Revisa los permisos de los archivos.
        /// </summary>
        /// <param name="path">El path.</param>
        /// <param name="checkRead">Si es <c>true</c> [check read].</param>
        /// <param name="checkWrite">Si es <c>true</c> [check write].</param>
        /// <param name="checkModify">Si es <c>true</c> [check modify].</param>
        /// <param name="checkDelete">Si es <c>true</c> [check delete].</param>
        /// <returns></returns>
        public static bool CheckPermissions(string path, bool checkRead, bool checkWrite, bool checkModify,
            bool checkDelete)
        {
            var flag = false;
            var flag2 = false;
            var flag3 = false;
            var flag4 = false;
            var flag5 = false;
            var flag6 = false;
            var flag7 = false;
            var flag8 = false;
            var current = WindowsIdentity.GetCurrent();
            AuthorizationRuleCollection rules;
            try
            {
                rules = Directory.GetAccessControl(path).GetAccessRules(true, true, typeof(SecurityIdentifier));
            }
            catch
            {
                return true;
            }
            try
            {
                foreach (FileSystemAccessRule rule in rules)
                {
                    if (current != null && !current.User.Equals(rule.IdentityReference))
                    {
                        continue;
                    }
                    if (AccessControlType.Deny.Equals(rule.AccessControlType))
                    {
                        if ((FileSystemRights.Delete & rule.FileSystemRights) == FileSystemRights.Delete)
                            flag4 = true;

                        if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                            flag3 = true;

                        if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                            flag = true;

                        if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                            flag2 = true;

                        continue;
                    }
                    if (AccessControlType.Allow.Equals(rule.AccessControlType))
                    {
                        if ((FileSystemRights.Delete & rule.FileSystemRights) == FileSystemRights.Delete)
                            flag8 = true;

                        if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                            flag7 = true;

                        if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                            flag5 = true;

                        if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                            flag6 = true;
                    }
                }
                foreach (var reference in current.Groups)
                {
                    foreach (FileSystemAccessRule rule2 in rules)
                    {
                        if (!reference.Equals(rule2.IdentityReference))
                        {
                            continue;
                        }
                        if (AccessControlType.Deny.Equals(rule2.AccessControlType))
                        {
                            if ((FileSystemRights.Delete & rule2.FileSystemRights) == FileSystemRights.Delete)
                                flag4 = true;

                            if ((FileSystemRights.Modify & rule2.FileSystemRights) == FileSystemRights.Modify)
                                flag3 = true;

                            if ((FileSystemRights.Read & rule2.FileSystemRights) == FileSystemRights.Read)
                                flag = true;

                            if ((FileSystemRights.Write & rule2.FileSystemRights) == FileSystemRights.Write)
                                flag2 = true;

                            continue;
                        }
                        if (AccessControlType.Allow.Equals(rule2.AccessControlType))
                        {
                            if ((FileSystemRights.Delete & rule2.FileSystemRights) == FileSystemRights.Delete)
                                flag8 = true;

                            if ((FileSystemRights.Modify & rule2.FileSystemRights) == FileSystemRights.Modify)
                                flag7 = true;

                            if ((FileSystemRights.Read & rule2.FileSystemRights) == FileSystemRights.Read)
                                flag5 = true;

                            if ((FileSystemRights.Write & rule2.FileSystemRights) == FileSystemRights.Write)
                                flag6 = true;
                        }
                    }
                }
                var flag9 = !flag4 && flag8;
                var flag10 = !flag3 && flag7;
                var flag11 = !flag && flag5;
                var flag12 = !flag2 && flag6;
                var flag13 = true;
                if (checkRead)
                    flag13 = flag13 && flag11;
                if (checkWrite)
                    flag13 = flag13 && flag12;
                if (checkModify)
                    flag13 = flag13 && flag10;
                if (checkDelete)
                    flag13 = flag13 && flag9;
                return flag13;
            }
            catch (IOException)
            {
            }
            return false;
        }

        /// <summary>
        ///     Lista de directorios fisicos que requieren permisos para escribir
        /// </summary>
        /// <param name="webHelper">The web helper.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetDirectoriesWrite(IWebHelper webHelper)
        {
            var rootDir = webHelper.MapPath("~/");
            var dirsToCheck = new List<string>
            {
                Path.Combine(rootDir, "App_Data"),
                Path.Combine(rootDir, "bin"),
                Path.Combine(rootDir, "content"),
                Path.Combine(rootDir, @"content\images"),
                Path.Combine(rootDir, @"content\images\thumbs"),
                Path.Combine(rootDir, @"content\images\uploaded"),
                Path.Combine(rootDir, @"content\files\exportimport"),
                Path.Combine(rootDir, "plugins"),
                Path.Combine(rootDir, @"plugins\bin")
            };
            //dirsToCheck.Add(rootDir);
            return dirsToCheck;
        }

        /// <summary>
        ///     Lista de paths fisicos que requieren permisos para escribir
        /// </summary>
        /// <param name="webHelper">The web helper.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetFilesWrite(IWebHelper webHelper)
        {
            var rootDir = webHelper.MapPath("~/");
            var filesToCheck = new List<string>
            {
                Path.Combine(rootDir, "Global.asax"),
                Path.Combine(rootDir, "web.config"),
                Path.Combine(rootDir, @"App_Data\InstalledPlugins.txt"),
                Path.Combine(rootDir, @"App_Data\Settings.txt")
            };
            return filesToCheck;
        }
    }
}