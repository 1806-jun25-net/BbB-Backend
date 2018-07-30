using BbB.Data;
using BbB.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BbB.Test
{
    public class RepoTests
    {
        private readonly DataRepository repo;

        public RepoTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<BbBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("BbB"));

            repo = new DataRepository(new BbBContext(optionsBuilder.Options));
        }

        [Fact]
        public async void TestGetUsers()
        {
            var actual = await repo.GetUsers();
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestGetDrives()
        {
            string company = "Revature";
            var actual = await repo.GetDrives(company);
            Assert.Empty(actual);
        }
        
        [Fact]
        public async void TestGetDestinations()
        {
            var actual = await repo.GetDestinations();
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestGetDestinationById()
        {
            int id = 4;
            var actual = await repo.GetDestinationById(id);
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestGetDestinationByTitle()
        {
            string title = "Taco Bell";
            var actual = await repo.GetDestinationByTitle(title);
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestGetMsgFrom()
        {
            int id = 1;
            var actual = await repo.GetMsgFrom(id);
            Assert.Null(actual);
        }

        [Fact]
        public async void TestGetMsgTo()
        {
            int id = 1;
            var actual = await repo.GetMsgTo(id);
            Assert.Null(actual);
        }

        [Fact]
        public async void TestGetMsg()
        {
            int id = 1;
            var actual = await repo.GetMsg(id);
            Assert.Null(actual);
        }

        [Fact]
        public async void TestGetUserReviews()
        {
            var actual = await repo.GetUserReviews();
            Assert.Empty(actual);
        }

        [Fact]
        public async void TestGetDriverReviews()
        {
            var actual = await repo.GetDriverReviews();
            Assert.Empty(actual);
        }

        [Fact]
        public async void TestGetMenuItems()
        {
            int id = 4;
            var actual = await repo.GetMenuItems(id);
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestVerifyLoginTrue()
        {
            string username = "wknain", password = "Password!23";
            bool expected = true;
            var actual = await repo.VerifyLogin(username, password);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void TestVerifyLoginFalse()
        {
            string username = "wknain", password = "Password123";
            bool expected = false;
            var actual = await repo.VerifyLogin(username, password);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void TestGetUserTrue()
        {
            int id = 1;
            var actual = await repo.GetUser(id);
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestGetUserFalse()
        {
            int id = -1;
            var actual = await repo.GetUser(id);
            Assert.Null(actual);
        }

        [Fact]
        public async void TestCheckUserNameTrue()
        {
            string userName = "Nothing";
            bool expected = true;
            var actual = await repo.CheckUserName(userName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void TestCheckUserNameFalse()
        {
            string userName = "wknain";
            bool expected = false;
            var actual = await repo.CheckUserName(userName);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void TestLookupUserId()
        {
            string name = "wknain";
            var actual = await repo.LookupUserId(name);
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestLookupDestinationId()
        {
            string name = "Taco Bell";
            var actual = await repo.LookupDestinationId(name);
            Assert.NotNull(actual);
        }

        [Fact]
        public async void TestLookupUserIdNull()
        {
            string name = "Nothing";
            var actual = await repo.LookupUserId(name);
            Assert.Null(actual);
        }

        [Fact]
        public async void TestLookupDestinationIdNull()
        {
            string name = "Nothing";
            var actual = await repo.LookupDestinationId(name);
            Assert.Null(actual);
        }
    }
}
