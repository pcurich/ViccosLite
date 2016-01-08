using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Core
{
    public interface IStoreContext
    {
        /// <summary>
        /// Contexto actual de la tienda
        /// </summary>
        Store CurrentStore { get; }
    }
}