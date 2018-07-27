using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BbB.Library
{
    public static class Mapper
    {
        //todo later where we actually need it
        //public static void MapperMain()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    IConfigurationRoot configuration = builder.Build();

        //    var optionsBuilder = new DbContextOptionsBuilder<BbBContext>();
        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("BbB"));

        //    var repo = new DataRepository(new BbBContext(optionsBuilder.Options));
        //}

        /* list of to objects
         * ArchiveItem (waiting on item)
         * Message
         */

        public static Destination Map(Data.Destination dest)
        {
            Destination d = new Destination(dest.Title, dest.Id, dest.StreetAddress);
            foreach (Data.MenuItem item in dest.MenuItem)
                d.AddItem(Map(item));
            return d; 
        }

        public static Data.Destination Map(Destination dest)
        {
            var d = new Data.Destination();
            if (dest.Id > -1)
                d.Id = dest.Id;
            d.Title = dest.Name;
            d.StreetAddress = dest.Address;
            return d;
        }

        public static Driver Map(Data.Driver driver)
        {
            Driver d = new Driver(Map(driver.User));
            d.DriverId = driver.Id;
            d.Seats = driver.Seats.Value;
            d.MeetLoc = driver.MeetLoc;
            //d.DriverRating = driver.Rating; TODO
            return d;
        }

        public static Data.Driver Map(Driver driver)
        {
            Data.Driver d = new Data.Driver();
            if (driver.Id > -1)
                d.Id = driver.Id;
            d.Seats = driver.Seats;
            d.MeetLoc = driver.MeetLoc;
            d.UserId = driver.Id;
            //d.Rating = driver.DriverRating; TODO
            return d;
        }


        public static User Map(Data.Usr user)
        {
            User u = new User();
            u.Company = user.Company;
            u.Credit = user.Credit;
            u.Email = user.EmailAddress;
            u.Id = user.Id;
            u.Name = user.UserName;
            //u.Rating = user.Rating; TODO
            u.Pass= user.Pass;
            return u;
        }

        public static Data.Usr Map(User user)
        {
            Data.Usr u = new Data.Usr();
            if (user.Id > -1)
                u.Id = user.Id;
            u.Company = user.Company;
            u.Credit = user.Credit;
            u.EmailAddress = user.Email;
            u.Pass = user.Pass;
            //u.Rating = user.Rating TODO
            return u;
        }

        public static Drive Map(Data.Drive drive)
        {
            if(drive.Dtype == "Pickup")//Pickup
            {
                var d = new PickupDrive(Map(drive.Driver), Map(drive.Destination), drive.Dtime.Value, drive.Id);
                foreach(Data.UserPickup pickup in drive.UserPickup)//adds users and items
                {
                    User u = Map(pickup.User);
                    d.AddUser(u);
                    foreach(Data.OrderItem item in pickup.OrderItem)
                    {
                        d.AddItem(u, Map(item));
                    }
                }
                return d;
            }
            else//Join
            {
                var d = new JoinDrive(Map(drive.Driver), Map(drive.Destination), drive.Dtime.Value, drive.Id);
                foreach (Data.UserJoin join in drive.UserJoin)//adds users
                {
                    d.AddUser(Map(join.User));
                }
                return d;
            }
        }

        public static Drive Map(Data.ArchiveDrive drive)
        {
            if (drive.Dtype == "Pickup")//Pickup
            {
                var d = new PickupDrive(Map(drive.Driver), Map(drive.Destination), drive.Dtime.Value, drive.Id);
                foreach (Data.ArchiveOrder pickup in drive.ArchiveOrder)//adds users and items
                {
                    User u = Map(pickup.User);
                    d.AddUser(u);
                    foreach (Data.ArchiveItem item in pickup.ArchiveItem)
                    {
                        d.AddItem(u, Map(item));
                    }
                }
                return d;
            }
            else//Join
            {
                var d = new JoinDrive(Map(drive.Driver), Map(drive.Destination), drive.Dtime.Value, drive.Id);
                foreach (Data.ArchiveUserJoin join in drive.ArchiveUserJoin)//adds users
                {
                    d.AddUser(Map(join.User));
                }
                return d;
            }
        }

        public static Data.Drive MapActive(Drive drive)
        {
            if (drive.IsPast())
                throw new ArgumentOutOfRangeException("drive", "Drive should be archived.");
            var d = new Data.Drive
            {
                DestinationId = drive.Dest.Id,
                DriverId = drive.Driver.DriverId,
                Dtime = drive.Time
            };
            if (drive.IsPickup())
                d.Dtype = "Pickup";
            else
                d.Dtype = "Join";
            if (drive.Id > -1)
                d.Id = drive.Id;
            return d;
        }

        /// <summary>
        /// note, drive will need to have id = -1 before it is archived
        /// </summary>
        public static Data.ArchiveDrive MapArchive(Drive drive)
        {
            var d = new Data.ArchiveDrive
            {
                DestinationId = drive.Dest.Id,
                DriverId = drive.Driver.DriverId,
                Dtime = drive.Time
            };
            if (drive.IsPickup())
                d.Dtype = "Pickup";
            else
                d.Dtype = "Join";
            if (drive.Id > -1)
                d.Id = drive.Id;
            return d;
        }

        public static OrderItem Map(Data.OrderItem orderItem)
        {
            return new OrderItem(Map(orderItem.Item), orderItem.Quantity, orderItem.Msg);
        }

        public static OrderItem Map(Data.ArchiveItem archiveItem)
        {
            return new OrderItem(
                new MenuItem(archiveItem.ItemName, archiveItem.Cost.Value),
                archiveItem.Quantity.Value, archiveItem.Msg);
        }

        public static Data.OrderItem Map(OrderItem orderItem,int orderId)
        {
            return new Data.OrderItem
            {
                ItemId = orderItem.Item.Id,
                OrderId = orderId,
                Msg = orderItem.Message,
                Quantity = orderItem.Quantity
            };
        }

        public static Data.ArchiveItem Map(OrderItem orderItem)
        {
            return new Data.ArchiveItem
            {
                ArchiveOrderId = 0,
                Cost = orderItem.Item.Cost,
                Id = orderItem.Id,
                ItemName = orderItem.Item.Name,
                Msg = orderItem.Message,
                Quantity = orderItem.Quantity
            };

        }

        public static MenuItem Map(Data.MenuItem menuItem)
        {
            return new MenuItem(menuItem.ItemName, menuItem.Cost.Value,menuItem.Id);
        }

        public static Data.MenuItem Map(MenuItem menuItem, int destId)
        {
            return new Data.MenuItem
            {
                Cost = menuItem.Cost,
                DestinationId = destId,
                Id = menuItem.Id,
                ItemName = menuItem.Name
            };
        }

        public static Message Map(Data.Msg msg)
        {
           return new Message(msg.SenderId,msg.ReceiverId, msg.Msg1, msg.Dtime.Value, msg.Id);
        }

        public static Data.Msg Map(Message message)
        {
            var m =new Data.Msg
            {
                SenderId = message.FromId,
                ReceiverId = message.ToId,
                Msg1 = message.Content
            };
            if (message.Id > -1)
                m.Id = message.Id;
            return m;
        }
        
        public static IEnumerable<Destination> Map(IEnumerable<Data.Destination> destinations)
        {
            return destinations.Select(Map);
        }

        public static IEnumerable<Data.Destination> Map(IEnumerable<Destination> destinations)
        {
            return destinations.Select(Map);
        }

        public static IEnumerable<Driver> Map(IEnumerable<Data.Driver> drivers)
        {
            return drivers.Select(Map);
        }

        public static IEnumerable<Data.Driver> Map(IEnumerable<Driver> drivers)
        {
            return drivers.Select(Map);
        }

        public static IEnumerable<User> Map(IEnumerable<Data.Usr> users)
        {
            return users.Select(Map);
        }

        public static IEnumerable<Data.Usr> Map(IEnumerable<User> users)
        {
            return users.Select(Map);
        }

        public static IEnumerable<Drive> Map(IEnumerable<Data.Drive> drives)
        {
            return drives.Select(Map);
        }

        public static IEnumerable<Drive> Map(IEnumerable<Data.ArchiveDrive> drives)
        {
            return drives.Select(Map);
        }

        public static IEnumerable<Data.Drive> MapActive(IEnumerable<Drive> drives)
        {
            return drives.Select(MapActive);
        }

        public static IEnumerable<Data.ArchiveDrive> MapArchive(IEnumerable<Drive> drives)
        {
            return drives.Select(MapArchive);
        }

        public static IEnumerable<OrderItem> Map(IEnumerable<Data.OrderItem> items)
        {
            return items.Select(Map);
        }

        public static IEnumerable<OrderItem> Map(IEnumerable<Data.ArchiveItem> items)
        {
            return items.Select(Map);
        }

        public static IEnumerable<Data.OrderItem> Map(IEnumerable<OrderItem> items, int destId)
        {
            return items.Select(i=>Map(i,destId));
        }

        public static IEnumerable<Data.ArchiveItem> Map(IEnumerable<OrderItem> items)
        {
            return items.Select(i=>Map(i));
        }


        public static IEnumerable<MenuItem> Map(IEnumerable<Data.MenuItem> items)
        {
            return items.Select(Map);
        }

        public static IEnumerable<Data.MenuItem> Map(IEnumerable<MenuItem> items, int destId)
        {
            return items.Select(i=>Map(i,destId));
        }

        public static IEnumerable<Message> Map(IEnumerable<Data.Msg> items)
        {
            return items.Select(Map);
        }

        public static IEnumerable<Data.Msg> Map(IEnumerable<Message> items)
        {
            return items.Select(Map);
        }

    }
}
