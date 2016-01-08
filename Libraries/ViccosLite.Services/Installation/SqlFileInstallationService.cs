using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ViccosLite.Core;
using ViccosLite.Core.Data;
using ViccosLite.Core.Domain.Sales;
using ViccosLite.Core.Domain.Stores;
using ViccosLite.Core.Domain.Users;
using ViccosLite.Core.Infrastructure;
using ViccosLite.Data.Entities;
using ViccosLite.Services.Users;

namespace ViccosLite.Services.Installation
{
    public class SqlFileInstallationService:IInstallationService
    {
        #region Ctr

        public SqlFileInstallationService(
            IRepository<User> userRepository,
            IRepository<Store> storeRepository,
            ISoftContext softContext,
            IWebHelper webHelper)
        {
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _softContext = softContext;
            _webHelper = webHelper;
        }

        #endregion

        #region Campos

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly ISoftContext _softContext;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Util

        protected virtual void UpdateDefaultCustomer(string defaultUserEmail, string defaultUserPassword)
        {
            var adminUser = _userRepository.Table.Single(x => !x.IsSystemAccount);
            if (adminUser == null)
                throw new Exception("El usuario administrador no puede cargar");

            adminUser.UserGuid = Guid.NewGuid();
            adminUser.Email = defaultUserEmail;
            adminUser.UserName = defaultUserEmail;
            _userRepository.Update(adminUser);

            var customerRegistrationService = EngineContext.Current.Resolve<IUserRegistrationService>();
            customerRegistrationService.ChangePassword(new ChangePasswordRequest(defaultUserEmail, false,defaultUserPassword));
        }

        protected virtual void UpdateDefaultStoreUrl()
        {
            var store = _storeRepository.Table.FirstOrDefault();
            if (store == null)
                throw new Exception("La tienda por default no puede ser inicializada");

            store.Url = _webHelper.GetStoreLocation(false);
            _storeRepository.Update(store);
        }

        protected virtual void ExecuteSqlFile(string path)
        {
            var statements = new List<string>();

            using (var stream = File.OpenRead(path))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                    statements.Add(statement);
            }

            foreach (var stmt in statements)
                _softContext.ExecuteSqlCommand(stmt);
        }

        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Metodo

        public void InstallData(string defaultUserEmail, string defaultUserPassword, bool installSampleData = true)
        {
            ExecuteSqlFile(_webHelper.MapPath("~/App_Data/Install/create_required_data.sql"));
            UpdateDefaultCustomer(defaultUserEmail, defaultUserPassword);
            UpdateDefaultStoreUrl();

            if (installSampleData)
            {
                ExecuteSqlFile(_webHelper.MapPath("~/App_Data/Install/create_sample_data.sql"));
            }
        }

        #endregion
    }
}