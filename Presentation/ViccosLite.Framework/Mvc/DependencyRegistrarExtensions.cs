using System;
using Autofac;
using ViccosLite.Core.Data;
using ViccosLite.Core.Infrastructure.DependencyManagement;
using ViccosLite.Data.Entities;

namespace ViccosLite.Framework.Mvc
{
    public static class DependencyRegistrarExtensions
    {
        public static void RegisterPluginDataContext<T>(this IDependencyRegistrar dependencyRegistrar,
           ContainerBuilder builder, string contextName)
           where T : ISoftContext
        {
            //Capa de datos
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                //registra el nombre del contexto
                builder.Register(
                    c => (ISoftContext)Activator.CreateInstance(typeof(T), dataProviderSettings.DataConnectionString))
                    .Named<ISoftContext>(contextName)
                    .InstancePerLifetimeScope();

                builder.Register(
                    c => (T)Activator.CreateInstance(typeof(T), dataProviderSettings.DataConnectionString))
                    .InstancePerLifetimeScope();
            }
            else
            {
                //registra el nombre del contexto
                builder.Register(
                    c => (T)Activator.CreateInstance(typeof(T), c.Resolve<DataSettings>().DataConnectionString))
                    .Named<ISoftContext>(contextName)
                    .InstancePerLifetimeScope();

                builder.Register(
                    c => (T)Activator.CreateInstance(typeof(T), c.Resolve<DataSettings>().DataConnectionString))
                    .InstancePerLifetimeScope();
            }
        }
    }
}