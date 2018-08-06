using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BbB.Data;
using BbB.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BbB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriveController : Controller
    {
        private readonly DataRepository data;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DriveController(DataRepository repository)
        {
            data = repository;
        }

        /// <summary>
        /// Get drive with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Library.Drive>> Get(int id)
        {
            Library.Drive drive = await data.GetDrive(id);
            if (drive == null)
                return NotFound();
            else
                return Ok(drive);
        }


        /// <summary>
        /// Get all drives from a driver from the given company
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpGet("{company}/company")]
        public async Task<ActionResult<List<Library.Drive>>> GetByCompany(string company)
        {
            var drive = await data.GetDrivesByCompany(company);
            return drive.ToList();
        }

        /// <summary>
        /// Get all drives for the user with given Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/user")]
        public async Task<ActionResult<List<Library.Drive>>> GetByUserId(int userId)
        {
            return Ok(await data.GetDrivesByUser(userId));
        }

        /// <summary>
        /// Get all drives from the driver with given Id
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpGet("{driverId}/driver")]
        public async Task<ActionResult<List<Library.Drive>>> GetByDriverId(int driverId)
        {
            return Ok(await data.GetDrivesByDriver(driverId));
        }

        /// <summary>
        /// Attempt to start a new drive
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<Library.Drive>> New(PickupDrive drive)
        {
            if (drive.Dest.Id == 0)
            {
                drive.Dest.Id = await data.LookupDestinationId(drive.Dest.Name);
            }
            try
            {
                var d = await data.NewDrive(drive);
                return Ok(d);
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                return BadRequest();
            }
        }

        [HttpPost("{driveId}/{userId}")]
        public async Task<ActionResult> JoinJD(int driveId, int userId)
        {
            try
            {
                await data.JoinJDrive(driveId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                return BadRequest();
            }
        }

        [HttpPost("{driveId}/{userId}/leave")]
        public async Task<ActionResult> Leave(int driveId, int userId)
        {
            try
            {
                await data.LeaveJD(driveId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                return BadRequest();
            }
        }

        [HttpPost("{driveId}/{userId}/pickup")]
        public async Task<ActionResult<int>> JoinPickup(int driveId, int userId)
        {
            try
            {
                var id = await data.JoinPickup(driveId, userId);
                return Ok(id);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("{orderId}/NewOrderItem")]
        public async Task<ActionResult> StoreOrderItem(Library.OrderItem orderItem, int orderId)
        {
            try
            {
                await data.NewOrderItem(orderItem, orderId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("{driveId}/{userId}/leavePickup")]
        public async Task<ActionResult> LeavePickup(int driveId, int userId)
        {
            try
            {
                await data.LeavePickup(driveId, userId);
                return Ok();
            }
            catch(Exception ex)
            {
                logger.Info(ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// Get the Ids of joined drives by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/JoinedDrives")]
        public async Task<ActionResult<List<int>>> JoinedDrives(int userId)
        {
            try
            {
                var joined = await data.GetIdOfJoinedDrives(userId);
                return Ok(joined);
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                return BadRequest();
            }
        }

        [HttpGet("{userId}/JoinedPickups")]
        public async Task<ActionResult<List<int>>> JoinedPickups(int userId)
        {
            try
            {
                var joined = await data.GetIdOfJoinedPickups(userId);
                return Ok(joined);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get the number of people that have joined a pickup drive
        /// </summary>
        /// <param name="driveId"></param>
        /// <returns></returns>
        [HttpGet("{driveId}/ORCount")]
        public ActionResult<int> ORCount(int driveId)
        {
            try
            {
                int count = data.GetOrderRealCount(driveId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                logger.Info(ex);
                return BadRequest();
            }
        }

        //[HttpGet("{driveId}/Pickups")]
        //public async Task<ActionResult> PickupsByDriveId(int driveId)
        //{
        //    var pickups = await data.GetPickups(driveId);
        //}
    }
}
