using BbB.Library;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BbB.Test
{
    public class MapperTests
    {
        [Fact]
        public void TestDataDestination()
        {
            Data.Destination dest = new Data.Destination();
            var expected = Mapper.Map(dest);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDestination()
        {
            Destination dest = new Destination("Food", 0, "101 5th Ave.");
            var expected = Mapper.Map(dest);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataDriver() //todo: figure out why this test fails
        {
            Data.Driver driver = new Data.Driver()
            {
                Id = 0,
                MeetLoc = "Garage",
                Seats = 4,
                UserId = 1,
                Rating = 0,
                User = new Data.Usr
                {
                    UserName = "wknain",
                    EmailAddress = "wknain@gra.midco.net",
                    Pass = "Password!23",
                    Company = "Revature"
                }
            };
            var expected = Mapper.Map(driver);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDriver()
        {
            User user = new User()
            {
                Id = 0,
                Name = "Mark",
                Email = "mark@mark.com",
                Pass = "Password!23",
                Company = "Hollywood"
            };
            Driver driver = new Driver(user);
            var expected = Mapper.Map(driver);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataUser()
        {
            Data.Usr usr = new Data.Usr();
            var expected = Mapper.Map(usr);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestUser()
        {
            User user = new User();
            var expected = Mapper.Map(user);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataDrivePickup()
        {
            Data.Drive drive = new Data.Drive()
            {
                Dtype = "Pickup",
                Driver = new Data.Driver()
                {
                    Id = 0,
                    MeetLoc = "Garage",
                    Seats = 4,
                    UserId = 1,
                    Rating = 0,
                    User = new Data.Usr
                    {
                        UserName = "wknain",
                        EmailAddress = "wknain@gra.midco.net",
                        Pass = "Password!23",
                        Company = "Revature"
                    }
                },
                Destination = new Data.Destination()
                {
                    Title = "Taco Bell",
                    StreetAddress = "101 Street"
                },
                Dtime = DateTime.Now
            };
            var expected = Mapper.Map(drive);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataDriveJoin()
        {
            Data.Drive drive = new Data.Drive()
            {
                Dtype = "Join",
                Driver = new Data.Driver()
                {
                    Id = 0,
                    MeetLoc = "Garage",
                    Seats = 4,
                    UserId = 1,
                    Rating = 0,
                    User = new Data.Usr
                    {
                        UserName = "wknain",
                        EmailAddress = "wknain@gra.midco.net",
                        Pass = "Password!23",
                        Company = "Revature"
                    }
                },
                Destination = new Data.Destination()
                {
                    Title = "Taco Bell",
                    StreetAddress = "101 Street"
                },
                Dtime = DateTime.Now
            };
            var expected = Mapper.Map(drive);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataArchiveDrivePickup()
        {
            Data.Drive drive = new Data.Drive()
            {
                Dtype = "Pickup",
                Driver = new Data.Driver()
                {
                    Id = 0,
                    MeetLoc = "Garage",
                    Seats = 4,
                    UserId = 1,
                    Rating = 0,
                    User = new Data.Usr
                    {
                        UserName = "wknain",
                        EmailAddress = "wknain@gra.midco.net",
                        Pass = "Password!23",
                        Company = "Revature"
                    }
                },
                Destination = new Data.Destination()
                {
                    Title = "Taco Bell",
                    StreetAddress = "101 Street"
                },
                Dtime = DateTime.Now
            };
            var expected = Mapper.Map(drive);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataArchiveDriveJoin()
        {
            Data.Drive drive = new Data.Drive()
            {
                Dtype = "Join",
                Driver = new Data.Driver()
                {
                    Id = 0,
                    MeetLoc = "Garage",
                    Seats = 4,
                    UserId = 1,
                    Rating = 0,
                    User = new Data.Usr
                    {
                        UserName = "wknain",
                        EmailAddress = "wknain@gra.midco.net",
                        Pass = "Password!23",
                        Company = "Revature"
                    }
                },
                Destination = new Data.Destination()
                {
                    Title = "Taco Bell",
                    StreetAddress = "101 Street"
                },
                Dtime = DateTime.Now
            };
            var expected = Mapper.Map(drive);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestActiveDrive()
        {

        }

        [Fact]
        public void TestArchiveDrive()
        {

        }

        [Fact]
        public void TestDataOrderItem()
        {
            Data.OrderItem item = new Data.OrderItem()
            {
                Item = new Data.MenuItem()
                {
                    DestinationId = 4,
                    ItemName = "Taco",
                    Cost = 1
                },
                Quantity = 1,
                Msg = ""
            };
            var expected = Mapper.Map(item);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataArchiveItem()
        {
            Data.ArchiveItem item = new Data.ArchiveItem()
            {
                ItemName = "Taco",
                Cost = 1,
                Quantity = 1,
                Msg = ""
            };
            var expected = Mapper.Map(item);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestOrderItemWithId()
        {
            int orderId = 1;
            MenuItem menuItem = new MenuItem("Taco", 1);
            OrderItem orderItem = new OrderItem(menuItem);
            var expected = Mapper.Map(orderItem, orderId);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestOrderItem()
        {
            MenuItem menuItem = new MenuItem("Taco", 1);
            OrderItem item = new OrderItem(menuItem);
            var expected = Mapper.Map(item);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataMenuItem()
        {
            Data.MenuItem item = new Data.MenuItem()
            {
                ItemName = "Taco",
                Cost = 1,
                Id = 1
            };
            var expected = Mapper.Map(item);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestMenuItemWithId()
        {
            int destId = 4;
            MenuItem item = new MenuItem("Taco", 1);
            var expected = Mapper.Map(item, destId);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestDataMsg()
        {
            Data.Msg msg = new Data.Msg()
            {
                SenderId = 1,
                ReceiverId = 2,
                Msg1 = "Hello",
                Dtime = DateTime.Now
            };
            var expected = Mapper.Map(msg);
            Assert.NotNull(expected);
        }

        [Fact]
        public void TestMessage()
        {
            int from = 1;
            int to = 2;
            string content = "Hello";
            Message message = new Message(from, to, content, DateTime.Now);
            var expected = Mapper.Map(message);
            Assert.NotNull(expected);
        }
    }
}
