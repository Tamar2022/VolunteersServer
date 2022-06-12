using Entity;
using BL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Volunteers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriveController : ControllerBase
    {
        IDriveBL driveBL;
        //MatchingFunctionBL matchingFunctionBL;
        public DriveController(IDriveBL driveBL)//, MatchingFunctionBL matchingFunctionBL)
        {
            this.driveBL = driveBL;
         //   this.matchingFunctionBL = matchingFunctionBL;
        }
        // GET: api/<DriveController>
        //[HttpGet]
        //public async Task<ActionResult<List<Drive>> GetAsync()
        //{
        //    return 
        //}

        // GET api/<DriveController>/5
        //get for history
        [HttpGet("DriverHistory/{driverId}")]

        public async Task<ActionResult<List<Drive>>> GetForHistoryAsync(int driverId)
        {
            return await driveBL.GetDriveBLForHistoryAsync(driverId);
        }

        //get for future
        [HttpGet("DriverFuture/{driverId}")]
        
        public async Task<ActionResult<List<Drive>>> GetForFutureAsync(int driverId)
        {
            try
            {
                return await driveBL.GetDriveBLForFutureAsync(driverId);

            }catch(Exception e)
            {
                return null;
            }
        }

       // POST api/<DriveController>
        [HttpPost]
        public void Post([FromBody] Drive value)
        {
            //return await driveBL.PostDriveBLAsync(value);
        }

        //[HttpGet("googlemap")]
        //public IActionResult googlemap(Drive value,int n)
        //{
        //  //  var res = matchingFunctionBL.MatchingFunctionForDisposableDrive(value,n);
            
        //    return Ok();
        //}
        

        //DELETE api/<DriveController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
            await driveBL.DeleteDriveBLAsync(id);
        }

    }
}
