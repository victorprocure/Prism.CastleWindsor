using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using Prism.CastleWindsor.Ioc;
using Prism.CastleWindsor.Properties;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace Prism.CastleWindsor
{
    [Obsolete("Recommend you use PrismApplication as the App's base class. This will require updating the App.xaml and App.xaml.cs files.", false)]
    public abstract class CastleWindsorBootstrapper : Prism.CastleWindsor.Bootstrapper
    {
        private bool _useDefaultConfiguration = true;

        /// <summary>
        /// Gets the default StructureMap <see cref="IWindsorContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IWindsorContainer"/> instance.</value>
        public IWindsorContainer Container { get; protected set; }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true" />, registers default Prism Library services in the container. This is the default behavior.</param>
        /// <inheritdoc />
        public override void Run(bool runWithDefaultConfiguration)
        {
            _useDefaultConfiguration = runWithDefaultConfiguration;

            Logger = CreateLogger();
            if (Logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }
            Logger.Log(Resources.LoggerCreatedSuccessfully, Category.Debug, Priority.Low);

            Logger.Log(Resources.CreatingModuleCatalog, Category.Debug, Priority.Low);
            ModuleCatalog = CreateModuleCatalog();
            if (ModuleCatalog == null)
            {
                throw new InvalidOperationException(Resources.NullModuleCatalogException);
            }

            Logger.Log(Resources.ConfiguringModuleCatalog, Category.Debug, Priority.Low);
            ConfigureModuleCatalog();

            Logger.Log(Resources.CreatingCastleWindsorContainer, Category.Debug, Priority.Low);
            Container = CreateContainer();
            if (Container == null)
            {
                throw new InvalidOperationException(Resources.NullCastleWindsorContainerException);
            }

            ContainerExtension = CreateContainerExtension();

            Logger.Log(Resources.ConfiguringCastleWindsorContainer, Category.Debug, Priority.Low);
            ConfigureContainer();

            Logger.Log(Resources.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            ConfigureServiceLocator();

            Logger.Log(Resources.ConfiguringViewModelLocator, Category.Debug, Priority.Low);
            ConfigureViewModelLocator();

            Logger.Log(Resources.ConfiguringRegionAdapters, Category.Debug, Priority.Low);
            ConfigureRegionAdapterMappings();

            Logger.Log(Resources.ConfiguringDefaultRegionBehaviors, Category.Debug, Priority.Low);
            ConfigureDefaultRegionBehaviors();

            Logger.Log(Resources.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);
            RegisterFrameworkExceptionTypes();

            Logger.Log(Resources.CreatingShell, Category.Debug, Priority.Low);
            Shell = CreateShell();
            if (Shell != null)
            {
                Logger.Log(Resources.SettingTheRegionManager, Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(Shell, Container.Resolve<IRegionManager>());

                Logger.Log(Resources.UpdatingRegions, Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                Logger.Log(Resources.InitializingShell, Category.Debug, Priority.Low);
                InitializeShell();
            }

            if (Container.Kernel.HasComponent(typeof(IModuleManager)))
            {
                Logger.Log(Resources.InitializingModules, Category.Debug, Priority.Low);
                InitializeModules();
            }

            Logger.Log(Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        /// <inheritdoc />
        /// <summary>
        /// Configures the LocatorProvider for the <see cref="T:CommonServiceLocator.ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());
        }

        /// <inheritdoc />
        /// <summary>
        /// Configures the <see cref="T:Prism.Mvvm.ViewModelLocator" /> used by Prism.
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => Container.Resolve(type));
        }

        /// <inheritdoc />
        /// <summary>
        /// Registers in the <see cref="T:Castle.Windsor.IWindsorContainer" /> the <see cref="T:System.Type" /> of the Exceptions
        /// that are not considered root exceptions by the <see cref="T:System.ExceptionExtensions" />.
        /// </summary>
        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();

            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentResolutionException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentNotFoundException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentRegistrationException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(CircularDependencyException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentDependencyRegistrationExtensions));
        }

        /// <summary>
        /// Creates the <see cref="WindsorContainer"/> that will be used to create the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="WindsorContainer"/>.</returns>
        protected virtual IWindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            return container;
        }

        /// <inheritdoc />
        /// <summary>
        /// Create the <see cref="CastleWindsorContainerExtension"/> that will be used as the
        /// default <see cref="IContainerExtension{TContainer}" />
        /// </summary>
        protected override IContainerExtension CreateContainerExtension()
        {
            return new CastleWindsorContainerExtension(Container);
        }

        /// <summary>
        /// Configures the <see cref="IWindsorContainer"/>. 
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Container
                .Register(Component.For<ILoggerFacade>().Instance(Logger))
                .Register(Component.For<IModuleCatalog>().Instance(ModuleCatalog))
                .Register(Component.For<IContainerExtension>().Instance(ContainerExtension));

            if (!_useDefaultConfiguration)
            {
                return;
            }

            Container.RegisterTypeIfMissing(Container, true);
            Container.RegisterTypeIfMissing<IServiceLocator, CastleWindsorServiceLocatorAdapter>(true);
            Container.RegisterTypeIfMissing<IModuleInitializer, ModuleInitializer>(true);
            Container.RegisterTypeIfMissing<IModuleManager, ModuleManager>(true);
            Container.RegisterTypeIfMissing<RegionAdapterMappings, RegionAdapterMappings>(true);
            Container.RegisterTypeIfMissing<IRegionManager, RegionManager>(true);
            Container.RegisterTypeIfMissing<IEventAggregator, EventAggregator>(true);
            Container.RegisterTypeIfMissing<IRegionViewRegistry, RegionViewRegistry>(true);
            Container.RegisterTypeIfMissing<IRegionBehaviorFactory, RegionBehaviorFactory>(true);
            Container.RegisterTypeIfMissing<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>(false);
            Container.RegisterTypeIfMissing<IRegionNavigationJournal, RegionNavigationJournal>(false);
            Container.RegisterTypeIfMissing<IRegionNavigationService, RegionNavigationService>(false);
            Container.RegisterTypeIfMissing<IRegionNavigationContentLoader, RegionNavigationContentLoader>(true);

            Container.Register(Classes.FromAssemblyContaining<IRegionAdapter>().BasedOn<IRegionAdapter>().LifestyleTransient());
            Container.Register(Classes.FromAssemblyContaining<IRegionBehavior>().BasedOn<IRegionBehavior>().LifestyleTransient());
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = Container.Resolve<IModuleManager>();
            }
            catch (ComponentNotFoundException ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                {
                    throw new InvalidOperationException(Resources.NullModuleCatalogException);
                }

                throw;
            }

            manager.Run();
        }
    }
}
