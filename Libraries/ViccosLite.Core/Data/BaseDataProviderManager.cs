using System;

namespace ViccosLite.Core.Data
{
    public abstract class BaseDataProviderManager
    {
        protected DataSettings Settings { get; private set; }
        public abstract IDataProvider LoadDataProvider();

        protected BaseDataProviderManager(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            Settings = settings;
        }
    }
}