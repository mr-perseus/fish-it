using System.Collections.Generic;
using System.Threading.Tasks;
using Fishit.Common;
using Fishit.Dal.Entities;
using Fishit.Logging;

namespace Fishit.BusinessLayer
{
    internal class CatchManager
    {
        private readonly CatchDao _catchDao;
        private readonly ILogger _logger;

        public CatchManager()
        {
            _logger = LogManager.GetLogger(nameof(CatchManager));
            _catchDao = new CatchDao();
        }

        public async Task<Response<List<Catch>>> GetAllCatches()
        {
            return await _catchDao.GetAllCatches();
        }

        public async Task<Response<Catch>> GetCatch()
        {
            return await _catchDao.GetCatch();
        }

        public async Task<Response<Catch>> CreateCatch(Catch aCatch)
        {
            return await _catchDao.CreateCatch(aCatch);
        }

        public async Task<Response<Catch>> UpdateCatch(Catch aCatch)
        {
            return await _catchDao.UpdateCatch(aCatch);
        }

        public async Task<Response<Catch>> DeleteCatch(Catch aCatch)
        {
            return await _catchDao.DeleteCatch(aCatch);
        }
    }
}