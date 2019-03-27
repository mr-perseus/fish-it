using Fishit.Dal;

namespace Fishit.TestEnvironment
{
    public abstract class TestBase
    {
        private static bool _firstTestInExecution = true;
        private static readonly object LockObject = new object();

        protected TestBase()
        {
            InitializeTestEnvironment();
        }

        private static void InitializeTestEnvironment()
        {
            lock (LockObject)
            {
                if (_firstTestInExecution)
                {
                    using (FishitContext context = new FishitContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        context.InitializeTestData();
                        _firstTestInExecution = false;
                        return;
                    }
                }
            }

            using (FishitContext context = new FishitContext())
            {
                context.Database.EnsureCreated();
                context.InitializeTestData();
            }
        }
    }
}