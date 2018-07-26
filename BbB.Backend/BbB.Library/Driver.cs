using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbB.Library
{
    public class Driver : User
    {
        /// <summary>
        /// internal Id
        /// -1 if untracked by database
        /// </summary>
        public int DriverId { get; set; }

        /// <summary>
        /// Default number of people who driver will take on a join drive
        /// </summary>
        public int Seats { get; set; }

        /// <summary>
        /// Default message for where to meet
        /// </summary>
        public string MeetLoc { get; set; }
        
        /// <summary>
        /// Rating of the user.
        /// Integral portion is number of reviews,
        /// Decimal is 1/10 of the users total rating.
        /// Use NumRating and AvgRating to access.
        /// </summary>
        public decimal DriverRating { get; set; }

        /// <summary>
        /// Default Constructor, a blank driver+user
        /// </summary>
        public Driver() : base() {
            DriverId = -1;
            Seats = 3;
            MeetLoc = "";
            DriverRating = 0;
        }

        /// <summary>
        /// Creates a new Driver based on an existing user
        /// </summary>
        /// <param name="user"></param>
        public Driver(User user)
        {
            Id = user.Id;
            Company = user.Company;
            Name = user.Name;
            Email = user.Email;
            Credit = user.Credit;
            Rating = user.Rating;
            DriverId = -1;
            Seats = 3;
            MeetLoc = "";
            DriverRating = 0;
        }

        /// <summary>
        /// Average rating as a driver
        /// </summary>
        /// <returns></returns>
        public decimal AvgDriverRating()
        {
            return (DriverRating - NumDriverRating()) * 10;
        }
        
        /// <summary>
        /// Number of ratings recieved as a driver
        /// </summary>
        /// <returns></returns>
        public int NumDriverRating()
         {
            return (int)Math.Truncate(DriverRating);
        }
}
}
