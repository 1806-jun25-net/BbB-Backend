using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BbB.Library
{
    public class Mapper
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



    }
}
