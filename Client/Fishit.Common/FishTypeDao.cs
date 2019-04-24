using Fishit.Logging;

namespace Fishit.Common
{
    internal class FishTypeDao
    {
        private const string EndPointUri = "http://sinv-56038.edu.hsr.ch:40007/api/fishingtrips";
        private readonly ILogger _logger;

        public FishTypeDao()
        {
            _logger = LogManager.GetLogger(nameof(FishTypeDao));
        }
    }
}