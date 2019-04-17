using System;
using System.Reflection;
using Fishit.Logging;
using Xunit.Sdk;

namespace Fishit.TestEnvironment.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SetupDb : BeforeAfterTestAttribute
    {
        private readonly ILogger _logger;

        public SetupDb()
        {
            _logger = LogManager.GetLogger(nameof(SetupDb));
        }

        public override async void Before(MethodInfo methodUnderTest)
        {
            await TestEnvironmentHelper.InitializeTestDataAndGetId();

            _logger.Info(nameof(Before) + "; methodUnderTest; " + methodUnderTest);
        }

        public override void After(MethodInfo methodUnderTest)
        {
            _logger.Info(nameof(After) + "; methodUnderTest; " + methodUnderTest);
        }
    }
}