﻿using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.CastleWindsor.Wpf.Tests.Support;
using Prism.Logging;

namespace Prism.CastleWindsor.Wpf.Tests
{
    [TestClass]
    public class CastleWindsorBootstrapperNullLoggerFixture : BootstrapperFixtureBase
    {
        [TestMethod]
        public void NullLoggerThrows()
        {
            var bootstrapper = new NullLoggerBootstrapper();

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "ILoggerFacade");
        }

        internal class NullLoggerBootstrapper : CastleWindsorBootstrapper
        {
            protected override ILoggerFacade CreateLogger()
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