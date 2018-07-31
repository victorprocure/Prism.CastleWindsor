using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using CommonServiceLocator;
using Prism.CastleWindsor.Ioc;
using Prism.Ioc;
using Prism.Regions;

namespace Prism.CastleWindsor
{
    /// <inheritdoc />
    public abstract class PrismApplication : PrismApplicationBase
    {
        /// <inheritdoc />
        protected override IContainerExtension CreateContainerExtension()
        {
            return new CastleWindsorContainerExtension();
        }

        /// <inheritdoc />
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, RegionNavigationContentLoader>();
            containerRegistry.RegisterSingleton<IServiceLocator, CastleWindsorServiceLocatorAdapter>();
        }

        /// <inheritdoc />
        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();

            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentResolutionException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentNotFoundException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentRegistrationException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(CircularDependencyException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentDependencyRegistrationExtensions));
        }
    }
}
