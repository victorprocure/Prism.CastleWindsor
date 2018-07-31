using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.CastleWindsor.Wpf.Tests.Support;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;

namespace Prism.CastleWindsor.Wpf.Tests
{
    [TestClass]
    public class CastleWindsorBootstrapperNullModuleManagerFixture : BootstrapperFixtureBase
    {
        [TestMethod]
        public void RunShouldNotCallInitializeModulesWhenModuleManagerNotFound()
        {
            var bootstrapper = new NullModuleManagerBootstrapper();

            bootstrapper.Run();

            Assert.IsFalse(bootstrapper.InitializeModulesCalled);
        }

        private class NullModuleManagerBootstrapper : CastleWindsorBootstrapper
        {
            public bool InitializeModulesCalled;

            protected override IWindsorContainer CreateContainer()
            {
                return new WindsorContainer();
            }

            protected override void ConfigureContainer()
            {
                Container
                    .Register(Component.For<ILoggerFacade>().Instance(Logger))
                    .Register(Component.For<IModuleCatalog>().Instance(ModuleCatalog));
            }

            protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
            {
                return null;
            }

            protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
            {
                return null;
            }

            protected override DependencyObject CreateShell()
            {
                return null;
            }

            protected override void InitializeModules()
            {
                InitializeModulesCalled = true;
            }
        }
    }
}