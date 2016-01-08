using System;
using System.Collections.Generic;
using System.Linq;
using ViccosLite.Core;
using ViccosLite.Core.Caching;
using ViccosLite.Core.Data;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Services.Stores
{
    public class StoreMappingService : IStoreMappingService
    {
        #region Constantes

        /// <summary>
        /// Llave para el cache
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity nombre
        /// </remarks>
        private const string STORE_MAPPING_BY_ENTITYID_NAME_KEY = "Soft.storemapping.entityid-name-{0}-{1}";

        /// <summary>
        /// Llave para borrar el patron 
        /// </summary>
        private const string STORE_MAPPING_PATTERN_KEY = "Soft.storemapping.";

        #endregion

        #region Campos

        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        public StoreMappingService(ICacheManager cacheManager,
            IStoreContext storeContext,
            IRepository<StoreMapping> storeMappingRepository )
        {
            _cacheManager = cacheManager;
            _storeContext = storeContext;
            _storeMappingRepository = storeMappingRepository;
        }

        #endregion

        #region Metodos

        public virtual void DeleteStoreMapping(StoreMapping storeMapping)
        {
            if (storeMapping == null)
                throw new ArgumentNullException("storeMapping");

            _storeMappingRepository.Delete(storeMapping);

            //cache
            _cacheManager.RemoveByPattern(STORE_MAPPING_PATTERN_KEY);
        }

        public virtual StoreMapping GetStoreMappingById(int storeMappingId)
        {
            return storeMappingId == 0 ? null : _storeMappingRepository.GetById(storeMappingId);
        }

        public virtual IList<StoreMapping> GetStoreMappings<T>(T entity) where T : BaseEntity, IStoreMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entityId = entity.Id;
            var entityName = typeof(T).Name;

            var query = from sm in _storeMappingRepository.Table
                        where sm.EntityId == entityId &&
                              sm.EntityName == entityName
                        select sm;
            var storeMappings = query.ToList();
            return storeMappings;
        }

        public void InsertStoreMapping(StoreMapping storeMapping)
        {
            if (storeMapping == null)
                throw new ArgumentNullException("storeMapping");

            _storeMappingRepository.Insert(storeMapping);

            //cache
            _cacheManager.RemoveByPattern(STORE_MAPPING_PATTERN_KEY);
        }

        public virtual void InsertStoreMapping<T>(T entity, int storeId) where T : BaseEntity, IStoreMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (storeId == 0)
                throw new ArgumentOutOfRangeException("storeId");

            var entityId = entity.Id;
            var entityName = typeof(T).Name;

            var storeMapping = new StoreMapping
            {
                EntityId = entityId,
                EntityName = entityName,
                StoreId = storeId
            };

            InsertStoreMapping(storeMapping);
        }

        public virtual void UpdateStoreMapping(StoreMapping storeMapping)
        {
            if (storeMapping == null)
                throw new ArgumentNullException("storeMapping");

            _storeMappingRepository.Update(storeMapping);

            //cache
            _cacheManager.RemoveByPattern(STORE_MAPPING_PATTERN_KEY);
        }

        public virtual int[] GetStoresIdsWithAccess<T>(T entity) where T : BaseEntity, IStoreMappingSupported
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entityId = entity.Id;
            var entityName = typeof(T).Name;

            var key = string.Format(STORE_MAPPING_BY_ENTITYID_NAME_KEY, entityId, entityName);
            return _cacheManager.Get(key, () =>
            {
                var query = from sm in _storeMappingRepository.Table
                            where sm.EntityId == entityId &&
                                  sm.EntityName == entityName
                            select sm.StoreId;
                return query.ToArray();
            });
        }

        public bool Authorize<T>(T entity) where T : BaseEntity, IStoreMappingSupported
        {
            return Authorize(entity, _storeContext.CurrentStore.Id);
        }

        public bool Authorize<T>(T entity, int storeId) where T : BaseEntity, IStoreMappingSupported
        {
            if (entity == null)
                return false;

            if (storeId == 0)
                //return true if no store specified/found
                return true;

            if (!entity.LimitedToStores)
                return true;

            foreach (var storeIdWithAccess in GetStoresIdsWithAccess(entity))
                if (storeId == storeIdWithAccess)
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        #endregion
    }
}