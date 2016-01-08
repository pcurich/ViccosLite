using System;
using ViccosLite.Core;
using ViccosLite.Core.Data;
using ViccosLite.Data.Provider;

namespace ViccosLite.Data.Entities
{
    public class EfDataProviderManager : BaseDataProviderManager
    {
        public EfDataProviderManager(DataSettings settings)
            : base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {
            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new KsException("Data Settings no contiene un providerName");

            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();
                case "sqlce":
                    return new SqlCeDataProvider();
                default:
                    throw new KsException(string.Format("No soporta dataprovider nombre: {0}", providerName));
            }
        }
    }
}