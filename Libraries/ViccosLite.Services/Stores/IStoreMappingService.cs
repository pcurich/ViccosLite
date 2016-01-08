using System.Collections.Generic;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Services.Stores
{
    public interface IStoreMappingService
    {
        void DeleteStoreMapping(StoreMapping storeMapping);
        StoreMapping GetStoreMappingById(int storeMappingId);
        IList<StoreMapping> GetStoreMappings<T>(T entity) where T : BaseEntity, IStoreMappingSupported;
        void InsertStoreMapping(StoreMapping storeMapping);
        void InsertStoreMapping<T>(T entity, int storeId) where T : BaseEntity, IStoreMappingSupported;
        void UpdateStoreMapping(StoreMapping storeMapping);
        int[] GetStoresIdsWithAccess<T>(T entity) where T : BaseEntity, IStoreMappingSupported;
        bool Authorize<T>(T entity) where T : BaseEntity, IStoreMappingSupported;
        bool Authorize<T>(T entity, int storeId) where T : BaseEntity, IStoreMappingSupported;

    }
}