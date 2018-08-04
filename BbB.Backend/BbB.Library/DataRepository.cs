using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BbB.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using NLog;

namespace BbB.Library
{
    public class DataRepository : IDataRepository
    {
        private readonly BbBContext bbBContext;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DataRepository(BbBContext input)
        {
            bbBContext = input ?? throw new ArgumentException(nameof(input));
        }

        /// <summary>
        /// Finds and archives all drives which need archived
        /// </summary>
        /// <returns></returns>
        private async Task Archive()
        {
            List<int> removeMe = new List<int>();
            foreach (Data.Drive drive in 
                await bbBContext.Drive
                .AsNoTracking().ToListAsync())
            {
                if (drive.Dtime.Value.AddMinutes(Drive.Buffer) < DateTime.Now)
                {
                    removeMe.Add(drive.Id);
                }
            }
            foreach(int i in removeMe)
            {
                await ArchiveDrive(i);
            }
        }

        /// <summary>
        /// Archives the drive with given and its sub values. Removes itself and all subvalues
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task ArchiveDrive(int id)
            //order: arch drive, arch orders, arch items, remove items, remove orders, remove drive
        {
            //arch drive
            Data.Drive drive = await bbBContext.Drive
                .Where(d => d.Id == id).AsNoTracking().FirstAsync();
            bbBContext.ArchiveDrive.Add(new ArchiveDrive()
            {
                DestinationId = drive.DestinationId,
                DriverId = drive.DriverId,
                Dtime = drive.Dtime,
                Dtype = drive.Dtype
            });
            await (bbBContext.SaveChangesAsync());
            int archiveId = (await bbBContext.ArchiveDrive
                .Where(d => (d.Dtime == drive.Dtime && d.DriverId == drive.DriverId))
                .AsNoTracking().FirstAsync()).Id;
            if(drive.Dtype == "Join")
            {
                //arch orders
                var joins = await bbBContext.UserJoin.Where(j => j.DriveId == drive.Id).AsNoTracking().ToListAsync();
                foreach (UserJoin j in joins)
                    bbBContext.ArchiveUserJoin.Add(
                        new ArchiveUserJoin() { ArchiveDriveId = archiveId, UserId = j.UserId });
                //remove orders
                bbBContext.UserJoin.RemoveRange(joins);
                await bbBContext.SaveChangesAsync();
            }
            else
            {
                var pickups = await bbBContext.UserPickup.Include(p=>p.OrderItem)
                    .Where(j => j.DriveId == drive.Id).AsNoTracking().ToListAsync();
                foreach (UserPickup p in pickups)
                {
                    //arch order
                    bbBContext.ArchiveOrder.Add(new ArchiveOrder() { ArchiveDriveId = archiveId, UserId = p.UserId});
                    await bbBContext.SaveChangesAsync();
                    int orderId = (await bbBContext.ArchiveOrder
                        .Where(o => o.ArchiveDriveId == archiveId && o.UserId == p.UserId)
                        .AsNoTracking().FirstAsync()).Id;
                    foreach (Data.OrderItem i in p.OrderItem)
                        //arch items
                        bbBContext.ArchiveItem.Add(new ArchiveItem()
                        {
                            ArchiveOrderId = orderId,
                            Cost = i.Item.Cost,
                            ItemName = i.Item.ItemName,
                            Msg = i.Msg,
                            Quantity = i.Quantity
                        });
                    //remove items
                    bbBContext.OrderItem.RemoveRange(p.OrderItem);
                }
                //remove orders
                bbBContext.UserPickup.RemoveRange(pickups);
                await bbBContext.SaveChangesAsync();
            }
            //Remove drive
            bbBContext.Drive.Remove(drive);
            await bbBContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get all values of a drive, including user but excluding Orders and menu.
        /// <para>Archives old drives before querying</para>
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Data.Drive>> GetFullDrives()
        {
            DateTime nextArchive = (await bbBContext.Drive.Select(d => d.Dtime).MinAsync()).GetValueOrDefault();
            if (nextArchive < DateTime.Now)
            {
                await Archive();
            }
            var drives = await bbBContext.Drive
                .Include(d => d.Driver)
                .Include(d=>d.Driver.User)
                .Include(d => d.Destination)
                .Include(d => d.UserPickup)
                .Include(d => d.UserJoin)
                .AsNoTracking()
                .ToListAsync();
            foreach (var drive in drives)
            {
                if (drive.Dtype == "Join")
                {
                    foreach(UserJoin j in drive.UserJoin)
                    {
                        j.User = await GetUsr(j.UserId);
                    }
                }
                else
                {
                    foreach (UserPickup p in drive.UserPickup)
                    {
                        p.User = await GetUsr(p.UserId);
                    }
                }
            }
            return drives;
        }

        /// <summary>
        /// All Users ever
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return Mapper.Map(await bbBContext.Usr.AsNoTracking().ToListAsync());
        }


        /// <summary>
        /// Active drive with given id
        /// </summary>
        /// <returns></returns>
        public async Task<Drive> GetDrive(int id)
        {
            return Mapper.Map((await GetFullDrives()).FirstOrDefault(d => d.Id == id));
        }

        /// <summary>
        /// All active drives by driver with given id
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Drive>> GetDrivesByDriver(int driverId)
        {
            return Mapper.Map((await GetFullDrives()).Where(d => d.DriverId == driverId).ToList());
        }

        /// <summary>
        /// All active drives by driver with given id
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Drive>> GetDrivesByUser(int userId)
        {
            return Mapper.Map((await GetFullDrives()).Where(
                d=> (d.UserJoin.Any(u => u.UserId == userId) ||
                d.UserPickup.Any(u => u.UserId == userId))
                )).ToList();
        }

        /// <summary>
        /// All active drives from a driver at the given company.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Drive>> GetDrivesByCompany(string company)
        {
            return Mapper.Map((await GetFullDrives()).Where(d => d.Driver.User.Company == company).ToList());
        }

        /// <summary>
        /// All known destinations
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Destination>> GetDestinations()
        {
            return Mapper.Map(await bbBContext.Destination.Include(m => m.MenuItem)
                .Include(d => d.Drive).Include(a => a.ArchiveDrive).AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Destination with the given id.
        /// <para>Returns null if not found.</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Destination> GetDestinationById(int id)
        {
            return Mapper.Map(await bbBContext.Destination.Include(m => m.MenuItem)
                .Include(d => d.Drive).Include(a => a.ArchiveDrive).AsNoTracking().FirstOrDefaultAsync(m => m.Id == id));
        }
        /// <summary>
        /// Destination with the given name.
        /// <para>Returns null if not found.</para>
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Destination>> GetDestinationByTitle(string title)
        {
            return Mapper.Map(await bbBContext.Destination.Include(m => m.MenuItem)
                .Include(d => d.Drive).Include(a => a.ArchiveDrive).Where(m => m.Title == title)
                .AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Returns the list of mesages from the user with given userId. null if no users with given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Message>> GetMsgFrom(int userId)
        {
            if (await bbBContext.Usr.Where(u => u.Id == userId).AnyAsync())
                return null;
            return Mapper.Map(await bbBContext.Msg.Where(m => m.SenderId == userId).AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Returns the list of mesages to the user with given userId. null if no users with given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Message>> GetMsgTo(int userId)
        {
            if (await bbBContext.Usr.Where(u => u.Id == userId).AnyAsync())
                return null;
            return Mapper.Map(await bbBContext.Msg.Where(m => m.ReceiverId == userId).AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Returns the message with given id, or null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Message> GetMsg(int id)
        {
            var msgs = await bbBContext.Msg.Where(m => m.Id == id).AsNoTracking().ToListAsync();
            if (msgs.Any())
                return Mapper.Map(msgs.First());
            else
                return null;
        }

        /// <summary>
        /// All user reviews
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserReview>> GetUserReviews()
        {
            return await bbBContext.UserReview.Include(d => d.Driver)
                .Include(u => u.User).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// All driver reviews
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DriverReview>> GetDriverReviews()
        {
            return await bbBContext.DriverReview.Include(d => d.Driver)
                .Include(u => u.User).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Returns all Menu Items from the given destination.
        /// </summary>
        /// <param name="destId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MenuItem>> GetMenuItems(int destId)
        {
            return Mapper.Map(await bbBContext.MenuItem.Where(i => i.DestinationId == destId).AsNoTracking().ToListAsync());
        }
        
        public async Task<bool> VerifyLogin(string username, string pass)
        {
            List<Usr> usrs = await bbBContext.Usr.AsNoTracking().ToListAsync();

            foreach (var item in usrs)
            {
                if (item.UserName == username && item.Pass == pass)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the user with the given id, null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Usr> GetUsr(int id)
        {
            var list = await bbBContext.Usr.Where(u => u.Id == id).ToListAsync();
            if (list.Any())
                return list.First();
            else
                return null;
        }

        /// <summary>
         /// Returns the user with the given id, null if not found
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
        public async Task<User> GetUser(int id)
        {
            return Mapper.Map(await GetUsr(id));
        }

        /// <summary>
        /// True if username is available, false if taken
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserName(string name)
        {
            List<Usr> usrs = await bbBContext.Usr.AsNoTracking().ToListAsync();

            foreach (var item in usrs)
            {
                if (item.UserName == name)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the user based on the username provided
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<User> GetUserByUsername(string username)
        {
            User user = Mapper.Map(await bbBContext.Usr.FirstOrDefaultAsync(x => x.UserName == username));
            return user;
        }

        /// <summary>
        /// Returns the id for the user with the given name. null if not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<int?> LookupUserId(string name)
        {
            List<Usr> usrs = await bbBContext.Usr.AsTracking().ToListAsync();

            foreach (var item in usrs)
            {
                if (item.UserName == name)
                {
                    return item.Id;
                }
            }
            return null; // method that calls this should check for null, which means the user was not found
        }

        /// <summary>
        /// Returns the destination id for the given location name. null if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<int?> LookupDestinationId(string name)
        {
            List<Data.Destination> destinations = await bbBContext.Destination.AsTracking().ToListAsync();

            foreach (var item in destinations)
            {
                if (item.Title == name)
                {
                    return item.Id;
                }
            }
            return null; // method that calls this should check for null, which means the location was not found
        }

        /// <summary>
        /// Gets the driver with the given Id. returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Driver> GetDriverByDriverId(int id)
        {
            return Mapper.Map(await bbBContext.Driver.Where(d => d.Id == id).Include(u=>u.User).AsNoTracking().FirstOrDefaultAsync());
        }

        /// <summary>
        /// Gets the driver with the given User Id. returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Driver> GetDriverByUserId(int? id)
        {
            return Mapper.Map(await bbBContext.Driver.Where(d => d.User.Id == id).Include(u => u.User).AsNoTracking().FirstOrDefaultAsync());
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task AddUser(string name, string email, string pass, string company)
        {
            var usr = new Usr
            {
                UserName = name,
                EmailAddress = email,
                Pass = pass,
                Company = company,
                Credit = 0.00M,
                Rating = 0.00M
            };

            try
            {
                bbBContext.Add(usr);
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds credit to the user with given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="credit"></param>
        /// <returns></returns>
        public async Task AddUserCredit(int id, decimal credit)
        {
            Usr lookup = await bbBContext.Usr.Where(x => x.Id == id).FirstAsync();
            lookup.Credit += credit;

            try
            {
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }

        /// <summary>
        /// Remove credit from the user with given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="credit"></param>
        /// <returns></returns>
        public async Task RemoveUserCredit(int id, decimal credit)
        {
            Usr lookup = await bbBContext.Usr.Where(x => x.Id == id).FirstAsync();
            lookup.Credit -= credit;

            try
            {
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Adds driver settings to the database for an existing user. does not check user exists
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="userId"></param>
        /// <param name="seats"></param>
        /// <param name="meetingLoc"></param>
        /// <returns></returns>
        public async Task AddDriver(int userId, int seats, string meetingLoc)
        {
            var driver = new Data.Driver
            {
                UserId = userId,
                Seats = seats,
                MeetLoc = meetingLoc
            };

            try
            {
                bbBContext.Add(driver);
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds a new active drive. Throws an exception if wrong parameters
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="destId"></param>
        /// <param name="time"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public async Task<Drive> NewDrive(int driverId, int destId, DateTime time, bool isPickup)
        {
            Driver driver = await GetDriverByDriverId(driverId);
            Destination dest = await GetDestinationById(destId);
            if (driver == null || dest == null || time < DateTime.Now.AddMinutes(Drive.Buffer))
                throw new Exception("Improper drive parameters.");
            Drive d;
            if (!isPickup)
                d = new JoinDrive(driver, dest, time);
            else
                d = new PickupDrive(driver, dest, time);
            try
            {
                bbBContext.Add(Mapper.MapActive(d));
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
            return d;
        }

        /// <summary>
        /// Join a join drive
        /// </summary>
        /// <param name="driveId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task JoinJDrive(int driveId, int userId)
        {
            var userJoin = new UserJoin
            {
                DriveId = driveId,
                UserId = userId
            };
            try
            {
                bbBContext.Add(userJoin);
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }

        }

        public async Task<Drive> NewDrive(Drive drive)
        {
            try { return await NewDrive(drive.Driver.Id, drive.Dest.Id, drive.Time, drive.IsPickup); }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds the base archive drive with no orders
        /// </summary>
        /// <param name="driverId"></param>
        /// <param name="destinationId"></param>
        /// <param name="dtype"></param>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public async Task AddArchiveDrive(int driverId, int destinationId, string dtype, DateTime dtime)
        {
            var archiveDrive = new ArchiveDrive
            {
                DriverId = driverId,
                DestinationId = destinationId,
                Dtype = dtype,
                Dtime = dtime
            };

            try
            {
                bbBContext.Add(archiveDrive);
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }



        /// <summary>
        /// Adds a message with given from, to, content at current Time.
        /// Does NOT check that userIds from, to exist
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task AddMessage(int from, int to, string content)
        {
            var msg = new Msg
            {
                ReceiverId = to,
                SenderId = from,
                Msg1 = content,
                Dtime = DateTime.Now
            };

            try
            {
                bbBContext.Add(msg);
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                throw;
            }
        }
    }
}
