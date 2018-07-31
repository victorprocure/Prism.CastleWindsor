using Castle.Windsor;
using Prism.Ioc;

namespace Prism.CastleWindsor.Ioc
{
    public static class PrismIocExtensions
    {
        public static IWindsorContainer GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IWindsorContainer>) containerProvider).Instance;
        }

        public static IWindsorContainer GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<IWindsorContainer>) containerRegistry).Instance;
        }
    }
}
