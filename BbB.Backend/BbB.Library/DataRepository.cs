using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BbB.Data;

namespace BbB.Library
{
    public class DataRepository
    {
        private readonly BbBContext bbBContext;

        public DataRepository(BbBContext input)
        {
            bbBContext = input ?? throw new ArgumentException(nameof(input));
        }

        public IEnumerable<Drive> GetDrives(string Company)
        {
            return Mapper.Map(bbBContext.Drive.Include(d => d.Destination)
                .Include(dr => dr.Driver).Include(u => u.UserJoin).AsNoTracking().ToList());
        }

        public IEnumerable<Destination> GetDestinations()
        {
            return Mapper.Map(bbBContext.Destination.Include(m => m.MenuItem)
                .Include(d => d.Drive).Include(a => a.ArchiveDrive).AsNoTracking().ToList());
        }

        public IEnumerable<Message> GetMsgFrom(int userId)
        {
            return Mapper.Map(bbBContext.Msg.Where(m => m.SenderId == userId).AsNoTracking().ToList());
        }

        public IEnumerable<Message> GetMsgTo(int userId)
        {
            return Mapper.Map(bbBContext.Msg.Where(m => m.ReceiverId == userId).AsNoTracking().ToList());
        }

        public List<UserReview> GetUserReviews()
        {
            return bbBContext.UserReview.Include(d => d.DriverId)
                .Include(u => u.UserId).AsNoTracking().ToList();
        }

        public List<DriverReview> GetDriverReviews()
        {
            return bbBContext.DriverReview.Include(d => d.DriverId)
                .Include(u => u.UserId).AsNoTracking().ToList();
        }

        public IEnumerable<MenuItem> GetMenuItems(int destId)
        {
            return Mapper.Map(bbBContext.MenuItem.Where(i => i.DestinationId == destId).AsNoTracking().ToList());
        }

        public bool VerifyLogin(string username, string pass)
        {
            List<Usr> usrs = bbBContext.Usr.AsNoTracking().ToList();

            foreach (var item in usrs)
            {
                if (item.UserName == username && item.Pass == pass)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckUserName(string name)
        {
            List<Usr> usrs = bbBContext.Usr.AsNoTracking().ToList();

            foreach (var item in usrs)
            {
                if (item.UserName == name)
                {
                    return false;
                }
            }
            return true;
        }

        public int? LookupUserId(string name)
        {
            List<Usr> usrs = bbBContext.Usr.AsTracking().ToList();

            foreach (var item in usrs)
            {
                if (item.UserName == name)
                {
                    return item.Id;
                }
            }
            return null; // method that calls this should check for null, which means the user was not found
        }

        public int? LookupDestinationId(string name)
        {
            List<Data.Destination> destinations = bbBContext.Destination.AsTracking().ToList();

            foreach (var item in destinations)
            {
                if (item.Title == name)
                {
                    return item.Id;
                }
            }
            return null; // method that calls this should check for null, which means the location was not found
        }

        public void AddUser(string name, string email, string pass, string company)
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
                bbBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddUserCredit(int id, decimal credit)
        {
            Usr lookup = bbBContext.Usr.Where(x => x.Id == id).First();
            lookup.Credit += credit;

            try
            {
                bbBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void RemoveUserCredit(int id, decimal credit)
        {
            Usr lookup = bbBContext.Usr.Where(x => x.Id == id).First();
            lookup.Credit -= credit;

            try
            {
                bbBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        public void AddDriver(int driverId, int seats, string meetingLoc)
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
                bbBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddArchiveDrive(int driverId, int destinationId, string dtype, DateTime dtime)
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
                bbBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
