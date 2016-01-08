using System;
using System.Linq;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Stores;
using ViccosLite.Services.Stores;

namespace ViccosLite.Framework.Context
{
    public class WebStoreContext : IStoreContext
    {
        private readonly IStoreService _storeService;
        private readonly IWebHelper _webHelper;

        private Store _cachedStore;

        public WebStoreContext(IStoreService storeService, IWebHelper webHelper)
        {
            _storeService = storeService;
            _webHelper = webHelper;
        }

        public virtual Store CurrentStore
        {
            get
            {
                if (_cachedStore != null)
                    return _cachedStore;

                //Se trata de determinar la tienda actual por el HTTP_POST
                var host = _webHelper.ServerVariables("HTTP_HOST");
                var allStores = _storeService.GetAllStores();
                var store = allStores.FirstOrDefault(s => s.ContainsHostValue(host)) ?? allStores.FirstOrDefault();

                if (store == null)
                    throw new Exception("Ninguna tienda a podido ser cargado");

                _cachedStore = store;
                return _cachedStore;
            }
        }
    }
}