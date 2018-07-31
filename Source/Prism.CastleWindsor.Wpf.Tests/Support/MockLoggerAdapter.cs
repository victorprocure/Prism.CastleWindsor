using System.Collections;
using System.Collections.Generic;
using Prism.Logging;

namespace Prism.CastleWindsor.Wpf.Tests.Support
{
    public class MockLoggerAdapter : ILoggerFacade
    {
        public IList<string> Messages = new List<string>();

        public void Log(string message, Category category, Priority priority)
        {
            Messages.Add(message);
        }
    }
}