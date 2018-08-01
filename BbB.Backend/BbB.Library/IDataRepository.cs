﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BbB.Data;

namespace BbB.Library
{
    public interface IDataRepository
    {
        Task AddArchiveDrive(int driverId, int destinationId, string dtype, DateTime dtime);
        Task AddDriver(int driverId, int userId, int seats, string meetingLoc);
        Task AddMessage(int from, int to, string content);
        Task AddUser(string name, string email, string pass, string company);
        Task AddUserCredit(int id, decimal credit);
        Task<bool> CheckUserName(string name);
        Task<Destination> GetDestinationById(int id);
        Task<IEnumerable<Destination>> GetDestinationByTitle(string title);
        Task<IEnumerable<Destination>> GetDestinations();
        Task<Driver> GetDriver(int id);
        Task<IEnumerable<DriverReview>> GetDriverReviews();
        Task<IEnumerable<Drive>> GetDrives(string company);
        Task<IEnumerable<MenuItem>> GetMenuItems(int destId);
        Task<Message> GetMsg(int id);
        Task<IEnumerable<Message>> GetMsgFrom(int userId);
        Task<IEnumerable<Message>> GetMsgTo(int userId);
        Task<User> GetUser(int id);
        Task<IEnumerable<UserReview>> GetUserReviews();
        Task<IEnumerable<User>> GetUsers();
        Task<int?> LookupDestinationId(string name);
        Task<int?> LookupUserId(string name);
        Task<Drive> NewDrive(int driverId, int destId, DateTime time, bool isJoin);
        Task RemoveUserCredit(int id, decimal credit);
        Task<bool> VerifyLogin(string username, string pass);
    }
}