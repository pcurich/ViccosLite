using System;
using System.Collections.Generic;
using System.Linq;
using ViccosLite.Core.Caching;
using ViccosLite.Core.Data;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Services.Stores
{
    public class StoreService : IStoreService
    {
        #region Constantes

        /// <summary>
        /// Llave para todos los caches de tienda
        /// </summary>
        private const string STORES_ALL_KEY = "Soft.stores.all";
        /// <summary>
        /// Llave para el cache
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string STORES_BY_ID_KEY = "Soft.stores.id-{0}";
        /// <summary>
        /// Patron de llave para borrar el cache
        /// </summary>
        private const string STORES_PATTERN_KEY = "Soft.stores.";

        #endregion

        #region Campos

        private readonly IRepository<Store> _storeRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="storeRepository">Store repository</param>
        public StoreService(ICacheManager cacheManager,
            IRepository<Store> storeRepository )
        {
            _cacheManager = cacheManager;
            _storeRepository = storeRepository;
        }

        #endregion

        #region Metodos

        public virtual void DeleteStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            var allStores = GetAllStores();
            if (allStores.Count == 1)
                throw new Exception("You cannot delete the only configured store");

            _storeRepository.Delete(store);

            _cacheManager.RemoveByPattern(STORES_PATTERN_KEY);
        }

        public virtual IList<Store> GetAllStores()
        {
            const string KEY = STORES_ALL_KEY;
            return _cacheManager.Get(KEY, () =>
            {
                var query = from s in _storeRepository.Table
                            orderby s.DisplayOrder, s.Id
                            select s;
                var stores = query.ToList();
                return stores;
            });
        }

        public virtual Store GetStoreById(int storeId)
        {
            if (storeId == 0)
                return null;

            var key = string.Format(STORES_BY_ID_KEY, storeId);
            return _cacheManager.Get(key, () => _storeRepository.GetById(storeId));
        }

        public virtual void InsertStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            _storeRepository.Insert(store);

            _cacheManager.RemoveByPattern(STORES_PATTERN_KEY);
        }

        public virtual void UpdateStore(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            _storeRepository.Update(store);

            _cacheManager.RemoveByPattern(STORES_PATTERN_KEY);
        }

        #endregion
    }
}