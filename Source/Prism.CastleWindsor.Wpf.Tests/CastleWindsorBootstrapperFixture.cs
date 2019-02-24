using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Castle.MicroKernel;
using Castle.Windsor;
using CommonServiceLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.CastleWindsor.Wpf.Tests.Support;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;

namespace Prism.CastleWindsor.Wpf.Tests
{
    [TestClass]
    public class CastleWindsorBootstrapperFixture : BootstrapperFixtureBase
    {
        [TestMethod]
        public void ContainerDefaultsToNull()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            var container = bootstrapper.BaseContainer;

            Assert.IsNull(container);
        }

        [TestMethod]
        public void CanCreateConcreteBootstrapper()
        {
            new DefaultCastleWindsorBootstrapper();
        }

        [TestMethod]
        public void CreateContainerShouldInitializeContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            var container = bootstrapper.CallCreateContainer();

            Assert.IsNotNull(container);
            Assert.IsInstanceOfType(container, typeof(IWindsorContainer));
        }

        [TestMethod]
        public void ConfigureContainerAddsModuleCatalogToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            var returnedCatalog = bootstrapper.BaseContainer.Resolve<IModuleCatalog>();
            Assert.IsNotNull(returnedCatalog);
            Assert.IsTrue(returnedCatalog is ModuleCatalog);
        }

        [TestMethod]
        public void ConfigureContainerAddsLoggerFacadeToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            var returnedCatalog = bootstrapper.BaseContainer.Resolve<ILoggerFacade>();
            Assert.IsNotNull(returnedCatalog);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationJournalEntryToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournalEntry>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationJournalToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationJournal>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsRegionNavigationServiceToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationService>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void ConfigureContainerAddsNavigationTargetHandlerToContainer()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();
            bootstrapper.Run();

            var actual1 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader>();
            var actual2 = bootstrapper.BaseContainer.Resolve<IRegionNavigationContentLoader>();

            Assert.IsNotNull(actual1);
            Assert.IsNotNull(actual2);
            Assert.AreSame(actual1, actual2);
        }

        [TestMethod]
        public void RegisterFrameworkExceptionTypesShouldRegisterActivationException()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ActivationException)));
        }

        [TestMethod]
        public void RegisterFrameworkExceptionTypesShouldRegisterResolutionFailedException()
        {
            var bootstrapper = new DefaultCastleWindsorBootstrapper();

            bootstrapper.CallRegisterFrameworkExceptionTypes();

            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ComponentResolutionException)));
            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ComponentNotFoundException)));
            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(ComponentRegistrationException)));
            Assert.IsTrue(ExceptionExtensions.IsFrameworkExceptionRegistered(
                typeof(CircularDependencyException)));
        }
    }

    internal class DefaultCastleWindsorBootstrapper : CastleWindsorBootstrapper
    {
        public bool ConfigureContainerCalled;
        public bool ConfigureDefaultRegionBehaviorsCalled;
        public bool ConfigureModuleCatalogCalled;
        public bool ConfigureRegionAdapterMappingsCalled;
        public bool ConfigureServiceLocatorCalled;
        public bool CreateContainerCalled;
        public bool CreateLoggerCalled;
        public bool CreateModuleCatalogCalled;
        public bool CreateShellCalled;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public bool InitializeModulesCalled;
        public bool InitializeShellCalled;
        public List<string> MethodCalls = new List<string>();
        public DependencyObject ShellObject = new UserControl();

        public DependencyObject BaseShell
        {
            get { return Shell; }
        }

        public IWindsorContainer BaseContainer
        {
            get { return Container; }
            set { Container = value; }
        }

        public MockLoggerAdapter BaseLogger
        {
            get { return Logger as MockLoggerAdapter; }
        }

        public IWindsorContainer CallCreateContainer()
        {
            return CreateContainer();
        }

        protected override IWindsorContainer CreateContainer()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateContainerCalled = true;
            return base.CreateContainer();
        }

        protected override void ConfigureContainer()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureContainerCalled = true;
            base.ConfigureContainer();
        }

        protected override ILoggerFacade CreateLogger()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateLoggerCalled = true;
            return new MockLoggerAdapter();
        }

        protected override DependencyObject CreateShell()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateShellCalled = true;
            return ShellObject;
        }

        protected override void ConfigureServiceLocator()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureServiceLocatorCalled = true;
            base.ConfigureServiceLocator();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            CreateModuleCatalogCalled = true;
            return base.CreateModuleCatalog();
        }

        protected override void ConfigureModuleCatalog()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureModuleCatalogCalled = true;
            base.ConfigureModuleCatalog();
        }

        protected override void InitializeShell()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            InitializeShellCalled = true;
            // no op
        }

        protected override void InitializeModules()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            InitializeModulesCalled = true;
            base.InitializeModules();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureDefaultRegionBehaviorsCalled = true;
            return base.ConfigureDefaultRegionBehaviors();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            ConfigureRegionAdapterMappingsCalled = true;
            var regionAdapterMappings = base.ConfigureRegionAdapterMappings();

            DefaultRegionAdapterMappings = regionAdapterMappings;

            return regionAdapterMappings;
        }

        protected override void RegisterFrameworkExceptionTypes()
        {
            MethodCalls.Add(MethodBase.GetCurrentMethod().Name);
            base.RegisterFrameworkExceptionTypes();
        }

        public void CallRegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();
        }
    }
}