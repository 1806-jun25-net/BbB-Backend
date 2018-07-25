using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
         * Destination
         * Drive
         * Driver
         * JoinDrive
         * Message
         * MenuItem
         * OrderItem
         * PickupDrive
         * User
         */

        public static Destination Map(Data.Destination dest, IEnumerable<MenuItem> menu)
        {
            Destination d = new Destination(dest.Title, dest.Id, dest.StreetAddress);
            foreach (MenuItem item in menu)
                d.AddItem(item);
            return null; 
        }

        public static Data.Destination Map(Destination dest)
        {
            return null;
        }

        public static Driver Map(Data.Driver driver)
        {
            return null;
        }

        public static Data.Driver Map(Driver driver)
        {
            return null;
        }


        public static User Map(Data.Usr user)
        {
            return null;
        }

        public static Data.Usr Map(User user)
        {
            return null;
        }

    }
}
