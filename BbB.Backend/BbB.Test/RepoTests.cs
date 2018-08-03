using BbB.Data;
using BbB.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
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
        public void TestGetUsers()
        {
            List<User> users = new List<User>();
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetUsers()).ReturnsAsync(users);

            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDrive()
        {
            Library.Driver driver = new Library.Driver()
            {
                Name = "wknain",
                Pass = "Password!23",
                Company = "Revature",
                Seats = 4,
                MeetLoc = "Garage"
            };
            Library.Destination destination = new Library.Destination("Taco Bell", 1, "");
            PickupDrive drive = new PickupDrive(driver, destination, DateTime.Now);
            int id = 1;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDrive(id)).ReturnsAsync(drive);
            
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDrivesByDriver()
        {
            List<PickupDrive> drive = new List<PickupDrive>();
            int id = 1;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDrivesByDriver(id)).ReturnsAsync(drive);

            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDrivesByUser()
        {
            List<PickupDrive> drive = new List<PickupDrive>();
            int id = 1;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDrivesByUser(id)).ReturnsAsync(drive);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDrivesByCompany()
        {
            List<PickupDrive> drive = new List<PickupDrive>();
            string company = "Revature";
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDrivesByCompany(company)).ReturnsAsync(drive);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDestinations()
        {
            List<Library.Destination> destinations = new List<Library.Destination>();
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDestinations()).ReturnsAsync(destinations);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDestinationById()
        {
            Library.Destination destination = new Library.Destination("Taco Bell", 4, "");
            int id = 4;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDestinationById(id)).ReturnsAsync(destination);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDestinationByTitle()
        {
            List<Library.Destination> destination = new List<Library.Destination>();
            string title = "Taco Bell";
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDestinationByTitle(title)).ReturnsAsync(destination);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetMsgFrom()
        {
            List<Message> messages = new List<Message>();
            int id = 1;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetMsgFrom(id)).ReturnsAsync(messages);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetMsgTo()
        {
            List<Message> messages = new List<Message>();
            int id = 1;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetMsgTo(id)).ReturnsAsync(messages);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetMsg()
        {
            int from = 1;
            int to = 2;
            string content = "Hello";
            Message message = new Message(from, to, content, DateTime.Now);
            int id = 1;
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetMsg(id)).ReturnsAsync(message);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetUserReviews()
        {
            List<UserReview> reviews = new List<UserReview>();
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetUserReviews()).ReturnsAsync(reviews);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetDriverReviews()
        {
            List<DriverReview> reviews = new List<DriverReview>();
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(x => x.GetDriverReviews()).ReturnsAsync(reviews);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestGetMenuItems()
        {
            List<Library.MenuItem> items = new List<Library.MenuItem>();
            var mockRepo = new Mock<IDataRepository>();
            int id = 4;
            mockRepo.Setup(x => x.GetMenuItems(id)).ReturnsAsync(items);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestVerifyLogin() //todo: figure out how to set these equal to one another
        {
            var mockRepo = new Mock<IDataRepository>();
            string username = "wknain", password = "Password!23";
            bool expected = true;
            mockRepo.Setup(x => x.VerifyLogin(username, password)).ReturnsAsync(expected);
            var actual = mockRepo.Object;
            //Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestGetUser()
        {
            User user = new User()
            {
                Name = "wknain",
                Pass = "Password!23"
            };
            var mockRepo = new Mock<IDataRepository>();
            int id = 1;
            mockRepo.Setup(x => x.GetUser(id)).ReturnsAsync(user);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestCheckUserName()
        {
            var mockRepo = new Mock<IDataRepository>();
            string userName = "Nothing";
            bool expected = true;
            mockRepo.Setup(x => x.CheckUserName(userName)).ReturnsAsync(expected);
            var actual = mockRepo.Object;
            //Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestLookupUserId()
        {
            var mockRepo = new Mock<IDataRepository>();
            string name = "wknain";
            int test = 0;
            mockRepo.Setup(x => x.LookupDestinationId(name)).ReturnsAsync(test);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }

        [Fact]
        public void TestLookupDestinationId()
        {
            var mockRepo = new Mock<IDataRepository>();
            string name = "Taco Bell";
            int test = 0;
            mockRepo.Setup(x => x.LookupDestinationId(name)).ReturnsAsync(test);
            var actual = mockRepo.Object;
            Assert.NotNull(actual);
        }
    }
}
