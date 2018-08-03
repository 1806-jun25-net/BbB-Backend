using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BbB.Data;
using BbB.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BbB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriveController : Controller
    {
        private readonly DataRepository data;

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
        [HttpPost]
        public async Task<ActionResult<Library.Drive>> New(Library.Drive drive)
        {
            try
            {
                var d = await data.NewDrive(drive);
                return Ok(d);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("{driveId}/{userId}")]
        public async Task<ActionResult<string>> JoinJD(int driveId, int userId)
        {
            try
            {
                await data.JoinJDrive(driveId, userId);
                return "joined";
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
