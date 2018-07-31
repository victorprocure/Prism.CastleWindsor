using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Prism.CastleWindsor
{
    public static class CastleWindsorExtensions
    {
        /// <summary>
        /// Registers the type if missing.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="container">The container to register in.</param>
        /// <param name="asSingleton">if set to <c>true</c> [as singleton].</param>
        public static void RegisterTypeIfMissing<TInterface, TImplementation>(this IWindsorContainer container, bool asSingleton)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            var hasComponent = container.Kernel.HasComponent(typeof(TInterface));
            if (hasComponent)
            {
                return;
            }

            container.Register(asSingleton
                ? Component.For<TInterface>().ImplementedBy<TImplementation>().LifestyleSingleton()
                : Component.For<TInterface>().ImplementedBy<TImplementation>().LifestyleTransient());
        }

        public static void RegisterTypeIfMissing<TInterface>(this IWindsorContainer container, TInterface instance,
            bool asSingleton)
            where TInterface : class
        {
            var hasComponent = container.Kernel.HasComponent(typeof(TInterface));
            if (hasComponent)
            {
                return;
            }

            container.Register(asSingleton
                ? Component.For<TInterface>().Instance(instance).LifestyleSingleton()
                : Component.For<TInterface>().Instance(instance).LifestyleTransient());
        }
    }
}
