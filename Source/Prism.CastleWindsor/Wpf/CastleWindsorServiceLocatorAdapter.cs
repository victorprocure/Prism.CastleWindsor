using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using CommonServiceLocator;

namespace Prism.CastleWindsor
{
    /// <summary>
    /// Defines a <see cref="IWindsorContainer"/> adapter for the <see cref="IServiceLocator"/> interface to be used by the Prism Library.
    /// </summary>
    /// <seealso cref="ServiceLocatorImplBase" />
    public class CastleWindsorServiceLocatorAdapter : ServiceLocatorImplBase
    {
        private readonly IWindsorContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleWindsorServiceLocatorAdapter"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IWindsorContainer"/> that will be used by
        /// the <see cref="DoGetInstance"/> and <see cref="DoGetAllInstances"/> methods.</param>
        /// <exception cref="ArgumentNullException">container</exception>
        public CastleWindsorServiceLocatorAdapter(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            _container = container;

        }

        /// <summary>
        /// Resolves the instance of the requested service
        /// </summary>
        /// <param name="serviceType">Type of the service requested.</param>
        /// <param name="key">Optional name of the service you want. Can be null.</param>
        /// <returns>The requested service instance</returns>
        /// <inheritdoc />
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null ? _container.Resolve(key, serviceType) : _container.Resolve(serviceType);
        }

        /// <summary>
        /// Resolves all the instances of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of the service requested.</param>
        /// <returns><see cref="IEnumerable{T}"/> of the service instances</returns>
        /// <inheritdoc />
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            var instance = _container.ResolveAll(serviceType).Cast<object>();

            return instance;
        }
    }
}
