using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fishit.Common;
using Fishit.Dal.Entities;
using Fishit.Logging;

namespace Fishit.BusinessLayer
{
    class CatchManager
    {
        private readonly ILogger _logger;

        public CatchManager()
        {
            _logger = LogManager.GetLogger(nameof(CatchManager));
        }

        public async Task<IEnumerable<Catch>> GetAllCatches()
        {
            return await new CatchDao().GetAllCatches();
        }

        public async Task<Catch> CreateCatch(Catch aCatch)
        {
            return await new CatchDao().CreateCatch(aCatch);
        }

        public async Task<Catch> UpdateCatch(Catch aCatch)
        {
            return await new CatchDao().UpdateCatch(aCatch);
        }

        public async Task<bool> DeleteCatch(Catch aCatch)
        {
            return await new CatchDao().DeleteCatch(aCatch);
        }
    }
}
