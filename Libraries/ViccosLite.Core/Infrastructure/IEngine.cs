using System;
using ViccosLite.Core.Configuration;
using ViccosLite.Core.Infrastructure.DependencyManagement;

namespace ViccosLite.Core.Infrastructure
{
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }
        void Initialize(Config config);
        T Resolve<T>() where T : class;
        object Resolve(Type type);
        T[] ResolveAll<T>();
    }
}