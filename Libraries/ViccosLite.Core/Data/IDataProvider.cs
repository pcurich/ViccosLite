using System.Data.Common;

namespace ViccosLite.Core.Data
{
    public interface IDataProvider
    {
        bool StoredProceduredSupported { get; }
        void InitConnectionFactory();
        void SetDatabaseInitializer();
        void InitDatabase();
        DbParameter GetParameter();
    }
}