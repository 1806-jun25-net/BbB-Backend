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
    public class DataRepository
    {
        private readonly BbBContext bbBContext;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DataRepository(BbBContext input)
        {
            bbBContext = input ?? throw new ArgumentException(nameof(input));
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
        /// All active drives from a driver at the given company.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Drive>> GetDrives(string company)
        {
            return Mapper.Map(await bbBContext.Drive.Include(d => d.Destination)
                .Include(dr => dr.Driver).Include(u => u.UserJoin).Where(x => x.Driver.User.Company == company).AsNoTracking().ToListAsync());
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
            return Mapper.Map(await bbBContext.Destination.FirstOrDefaultAsync(m => m.Id == id));
        }
        /// <summary>
        /// Destination with the given name.
        /// <para>Returns null if not found.</para>
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Destination>> GetDestinationByTitle(string title)
        {
            return Mapper.Map(await bbBContext.Destination.Where(m => m.Title == title).ToListAsync());
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
        public async Task<User> GetUser(int id)
        {
            var list = await bbBContext.Usr.Where(u => u.Id == id).ToListAsync();
            if (list.Any())
                return Mapper.Map(list.First());
            else
                return null;
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
        public async Task AddDriver(int driverId, int userId, int seats, string meetingLoc)
        {
            var driver = new Data.Driver
            {
                UserId = userId,
                Id = driverId,
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
