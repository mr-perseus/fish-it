using System.Collections.Generic;
using Fishit.Dal.Entities;
using Fishit.TestEnvironment;
using Xunit;

namespace Fishit.Common.Testing
{
    public class CatchDaoTest : TestBase
    {
        private CatchDao _catchDao;

        private CatchDao CatchDao =>
            _catchDao ?? (_catchDao = new CatchDao());

        [Fact]
        public async void GetAllCatches()
        {
            List<Catch> catches = await CatchDao.GetAllCatches();
            Assert.True(catches.Count >= 0);
        }
    }
}