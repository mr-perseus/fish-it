using System;
using System.Collections.Generic;
using Fishit.Dal.Entities;
using Xunit;

namespace Fishit.Common.Testing
{
    public class CatchDaoTest
    {
        private CatchDao _catchDao;

        private CatchDao CatchDao =>
            _catchDao ?? (_catchDao = new CatchDao());

        private readonly Catch _aCatch = new Catch()
        {
            DateTime = new DateTime(2019, 04, 17),
            FishType = new FishType()
            {
                Id = "5cb34f29500b0509f4244306",
                Name = "Sea Urchin",
                Description = "A great whatever it is"
            },
            Id = "0",
            Length = 19.2,
            Weight = 15.8
        };

        private readonly Catch _bCatch = new Catch()
        {
            DateTime = new DateTime(2017, 12, 03),
            FishType = new FishType()
            {
                Id = "5cb34f29500b0509f4244306",
                Name = "Sea Urchin",
                Description = "A great whatever it is"
            },
            Id = "0",
            Length = 866.2,
            Weight = 175.2
        };


        [Fact]
        public async void GetAllCatches()
        {
            List<Catch> catches = await CatchDao.GetAllCatches();
            Assert.True(catches.Count >= 0);
        }

        [Fact]
        public async void CreateCatch()
        {
            Catch aCatch = await CatchDao.CreateCatch(_aCatch);
        }
    }
}