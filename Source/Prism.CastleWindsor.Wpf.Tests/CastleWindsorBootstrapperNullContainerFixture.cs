using System;
using System.Windows;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.CastleWindsor.Wpf.Tests.Support;

namespace Prism.CastleWindsor.Wpf.Tests
{
    [TestClass]
    public class CastleWindsorBootstrapperNullContainerFixture : BootstrapperFixtureBase
    {
        [TestMethod]
        public void RunThrowsWhenNullContainerCreated()
        {
            var bootstrapper = new NullContainerBootstrapper();

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IWindsorContainer");
        }

        private class NullContainerBootstrapper : CastleWindsorBootstrapper
        {
            protected override IWindsorContainer CreateContainer()
            {
                return null;
            }

            protected override DependencyObject CreateShell()
            {
                throw new NotImplementedException();
            }

            protected override void InitializeShell()
            {
                throw new NotImplementedException();
            }
        }
    }
}