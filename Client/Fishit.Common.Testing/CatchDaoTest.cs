using System;
using System.Collections.Generic;
using System.Net;
using Fishit.Dal.Entities;
using Xunit;

namespace Fishit.Common.Testing
{
    public class CatchDaoTest
    {
        private CatchDao _catchDao;
        private Response<Catch> _responseCreate;

        private CatchDao CatchDao =>
            _catchDao ?? (_catchDao = new CatchDao());

        private readonly Catch _aCatch = new Catch
        {
            DateTime = new DateTime(2019, 04, 17),
            FishType = new FishType
            {
                Id = "5cb34f29500b0509f4244306",
                Name = "Sea Urchin",
                Description = "A great whatever it is"
            },
            Id = "0",
            Length = 19.2,
            Weight = 15.8
        };

        [Fact]
        public async void CreateCatch()
        {
            _responseCreate = await CatchDao.CreateCatch(_aCatch);
            Assert.True(_responseCreate.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void UpdateCatch()
        {
            _responseCreate.Content.Length = 888;
            Response<Catch> response = await CatchDao.UpdateCatch(_responseCreate.Content);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void DeleteCatch()
        {
            Response<Catch> response = await CatchDao.DeleteCatch(_responseCreate.Content);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async void GetAllCatches()
        {
            Response<List<Catch>> response = await CatchDao.GetAllCatches();
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }
    }
}