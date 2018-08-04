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
    public class ArchiveController : Controller
    {
        private readonly DataRepository data;
        
        public ArchiveController(DataRepository repository)
        {
            data = repository;
        }
        /// <summary>
          /// Get drive with given id
          /// </summary>
          /// <param name="id"></param>
          /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Library.Drive>>> Get()
        {
            var drives = await data.GetArchiveDrives();
            if (drives == null)
                return NotFound();
            else
                return Ok(drives);
        }

        /// <summary>
        /// Get drive with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Library.Drive>> GetById(int id)
        {
            Library.Drive drive = await data.GetArchiveDrive(id);
            if (drive == null)
                return NotFound();
            else
                return Ok(drive);
        }
    }
}
