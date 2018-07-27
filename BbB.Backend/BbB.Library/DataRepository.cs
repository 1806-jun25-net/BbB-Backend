using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BbB.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class DataRepository
    {
        private readonly BbBContext bbBContext;

        public DataRepository(BbBContext input)
        {
            bbBContext = input ?? throw new ArgumentException(nameof(input));
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return Mapper.Map(await bbBContext.Usr.AsNoTracking().ToListAsync());
        }

        public async Task<IEnumerable<Drive>> GetDrives(string company)
        {
            return Mapper.Map(await bbBContext.Drive.Include(d => d.Destination)
                .Include(dr => dr.Driver).Include(u => u.UserJoin).Where(x => x.Driver.User.Company == company).AsNoTracking().ToListAsync());
        }

        public async Task<IEnumerable<Destination>> GetDestinations()
        {
            return Mapper.Map(await bbBContext.Destination.Include(m => m.MenuItem)
                .Include(d => d.Drive).Include(a => a.ArchiveDrive).AsNoTracking().ToListAsync());
        }

        public async Task<Destination> GetDestinationById(int id)
        {
            return Mapper.Map(await bbBContext.Destination.FirstOrDefaultAsync(m => m.Id == id));
        }
        public async Task<Destination> GetDestinationByName(string title)
        {
            return Mapper.Map(await bbBContext.Destination.FirstOrDefaultAsync(m => m.Title == title));
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

        public async Task<IEnumerable<UserReview>> GetUserReviews()
        {
            return await bbBContext.UserReview.Include(d => d.DriverId)
                .Include(u => u.UserId).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<DriverReview>> GetDriverReviews()
        {
            return await bbBContext.DriverReview.Include(d => d.DriverId)
                .Include(u => u.UserId).AsNoTracking().ToListAsync();
        }

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

        public async Task<User> GetUser(int id)
        {
            var list = await bbBContext.Usr.Where(u => u.Id == id).ToListAsync();
            if (list.Any())
                return Mapper.Map(list.First());
            else
                return null;
        }

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

        public async Task AddUser(string name, string email, string pass, string company)
        {
            var usr = new Usr
            {
                UserName = name,
                EmailAddress = email,
                Pass = pass,
                Company = company
            };

            try
            {
                bbBContext.Add(usr);
                await bbBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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
                throw;
            }
        }

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
                throw;
            }
        }

        
        public async Task AddDriver(int driverId, int seats, string meetingLoc)
        {
            var driver = new Driver
            {
                DriverId = driverId,
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
                throw;
            }
        }

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
                throw;
            }
        }
    }
}
