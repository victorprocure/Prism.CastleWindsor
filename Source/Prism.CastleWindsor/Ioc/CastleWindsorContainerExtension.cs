using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Prism.Ioc;

namespace Prism.CastleWindsor.Ioc
{
    public class CastleWindsorContainerExtension : IContainerExtension<IWindsorContainer>
    {
        public CastleWindsorContainerExtension() : this(new WindsorContainer())
        {
        }

        public CastleWindsorContainerExtension(IWindsorContainer container)
        {
            Instance = container;
        }

        public object Resolve(Type type)
        {
            return Instance.Resolve(type);
        }

        public object Resolve(Type type, string name)
        {
            return Instance.Resolve(name, type);
        }

        public void RegisterInstance(Type type, object instance)
        {
            Instance.Register(Component.For(type).Instance(instance));
        }

        public void RegisterSingleton(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleSingleton());
        }

        public void Register(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleTransient());
        }

        public void Register(Type from, Type to, string name)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).Named(name).LifestyleTransient());
        }

        public void FinalizeExtension()
        {
        }

        public object ResolveViewModelForView(object view, Type viewModelType)
        {
            return Instance.Resolve(viewModelType);
        }

        public bool SupportsModules => true;

        public IWindsorContainer Instance { get; }
    }
}
