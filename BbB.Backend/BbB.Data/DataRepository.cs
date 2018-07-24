using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BbB.Data
{
    public class DataRepository
    {
        private readonly BbBContext bbBContext;

        public DataRepository(BbBContext input)
        {
            bbBContext = input ?? throw new ArgumentException(nameof(input));
        }

        public List<Drive> GetDrives()
        {
            List<Drive> drives = bbBContext.Drive.Include(d => d.Destination)
                .Include(dr => dr.Driver).Include(u => u.UserJoin).AsNoTracking().ToList();
            return drives;
        }

        public List<Destination> GetDestinations()
        {
            List<Destination> destinations = bbBContext.Destination.Include(m => m.MenuItem)
                .Include(d => d.Drive).Include(a => a.ArchiveDrive).AsNoTracking().ToList();
            return destinations;
        }

        public List<Msg> GetMsgs()
        {
            List<Msg> msgs = bbBContext.Msg.Include(r => r.Receiver).Include(s => s.Sender).AsNoTracking().ToList();
            return msgs;
        }

        public List<UserReview> GetUserReviews()
        {
            List<UserReview> userReviews = bbBContext.UserReview.Include(d => d.DriverId)
                .Include(u => u.UserId).AsNoTracking().ToList();
            return userReviews;
        }

        public List<DriverReview> GetDriverReviews()
        {
            List<DriverReview> driverReviews = bbBContext.DriverReview.Include(d => d.DriverId)
                .Include(u => u.UserId).AsNoTracking().ToList();
            return driverReviews;
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
            List<Destination> destinations = bbBContext.Destination.AsTracking().ToList();

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
            bbBContext.Add(usr);
            bbBContext.SaveChanges();
        }

        public void AddUserCredit(int id, decimal credit)
        {
            Usr lookup = bbBContext.Usr.Where(x => x.Id == id).First();
            lookup.Credit += credit;
            bbBContext.SaveChanges();
        }

        public void AddDriver(int userId, int seats, string meetingLoc)
        {
            var driver = new Driver
            {
                UserId = userId,
                Seats = seats,
                MeetLoc = meetingLoc
            };
            bbBContext.Add(driver);
            bbBContext.SaveChanges();
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
            bbBContext.Add(archiveDrive);
            bbBContext.SaveChanges();
        }
    }
}
