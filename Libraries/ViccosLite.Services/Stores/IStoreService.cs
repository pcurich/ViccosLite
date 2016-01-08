using System.Collections.Generic;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Services.Stores
{
    public interface IStoreService
    {
        void DeleteStore(Store store);
        IList<Store> GetAllStores();
        Store GetStoreById(int storeId);
        void InsertStore(Store store);
        void UpdateStore(Store store);
    }
}