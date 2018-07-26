using BbB.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BbB.Test
{
    public class DriveTests
    {
        private static JoinDrive Join;
        private static PickupDrive Pickup;
        private static readonly OrderItem testItem = new OrderItem("Test", 1);
        private void Setup()
        {
            Join = new JoinDrive(new Driver(), new Destination(""),DateTime.Now);
            Pickup = new PickupDrive(new Driver(), new Destination(""), DateTime.Now);
        }
        private void Cleanup() { }

        [Fact]
        public void AddUser_ShouldWork_WhenUserIsNew()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };

            Assert.True(Join.AddUser(test));
            Assert.Contains(test, Join.Users());

            Assert.True(Pickup.AddUser(test));
            Assert.Contains(test, Pickup.Users());
        }

        [Fact]
        public void AddUser_ShouldFail_WhenUserExists()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            Join.AddUser(test);
            Assert.False(Join.AddUser(test));
            Assert.Contains(test, Join.Users());
            Pickup.AddUser(test);
            Assert.False(Pickup.AddUser(test));
            Assert.Contains(test, Pickup.Users());
        }
        [Fact]
        public void RemoveUser_ShouldWork_WhenUserExists()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            Join.AddUser(test);
            Assert.True(Join.RemoveUser(test));
            Assert.DoesNotContain(test, Join.Users());
            Pickup.AddUser(test);
            Assert.True(Pickup.RemoveUser(test));
            Assert.DoesNotContain(test, Pickup.Users());
        }
        [Fact]
        public void RemoveUser_ShouldFail_WhenUserNotFound()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            Assert.False(Join.RemoveUser(test));
            Assert.DoesNotContain(test, Join.Users());
            Assert.False(Pickup.RemoveUser(test));
            Assert.DoesNotContain(test, Pickup.Users());
        }
        [Fact]
        public void IsPast_ShouldTrue_WhenDriveIsOld()
        {
            Setup();
            Assert.True(Join.IsPast());
            Assert.True(Pickup.IsPast());
        }
        [Fact]
        public void IsPast_ShouldFalse_WhenDriveIsNew()
        {
            Setup();
            Join.Time = DateTime.Now.AddHours(1);
            Assert.False(Join.IsPast());
            Pickup.Time = DateTime.Now.AddHours(1);
            Assert.False(Pickup.IsPast());
        }
        [Fact]
        public void AddItem_ShouldAdd_WhenItemIsNewAndUserExists()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            Pickup.AddUser(test);
            Assert.True(Pickup.AddItem(test, testItem));
            Assert.Contains(testItem, Pickup.OrderForUser(test));
        }
        [Fact]
        public void AddItem_ShouldUpdateQuantity_WhenItemExists()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            OrderItem testItem2 = new OrderItem("Test",1);
            OrderItem testItem3 = new OrderItem("Test", 1);
            Pickup.AddUser(test);
            Pickup.AddItem(test, testItem);
            Assert.Contains(testItem, Pickup.OrderForUser(test));
            Assert.Equal(1, Pickup.OrderForUser(test).Where(i=>i == testItem).First().Quantity);
            Pickup.AddItem(test, testItem2);
            Assert.Contains(testItem, Pickup.OrderForUser(test));
            Assert.Equal(2, Pickup.OrderForUser(test).Where(i => i == testItem).First().Quantity);
            Pickup.AddItem(test, testItem3);
            Assert.Contains(testItem, Pickup.OrderForUser(test));
            Assert.Equal(3, Pickup.OrderForUser(test).Where(i => i == testItem).First().Quantity);
        }
        [Fact]
        public void AddItem_ShouldFail_WhenUserNotFound()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
           Assert.False(Pickup.AddItem(test, testItem));
        }
        [Fact]
        public void RemoveItem_ShouldWork_WhenItemExists()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            Pickup.AddUser(test);
            Pickup.AddItem(test, testItem);
            Assert.True(Pickup.RemoveItem(test, testItem));
        }
        [Fact]
        public void RemoveItem_ShouldFail_WhenItemNotFound()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            Pickup.AddUser(test);
            Assert.False(Pickup.RemoveItem(test, testItem));
        }
        [Fact]
        public void RemoveItem_ShouldFail_WhenUserNotFound()
        {
            Setup();
            User test = new User
            {
                Name = "Test"
            };
            Assert.False(Pickup.RemoveItem(test, testItem));
        }
    }
}
