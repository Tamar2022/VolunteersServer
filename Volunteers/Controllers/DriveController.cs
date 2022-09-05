using Entity;
using BL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Volunteers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DriveController : ControllerBase
    {
        IMapper _mapper;
        IDriveBL driveBL;
        //MatchingFunctionBL matchingFunctionBL;
        public DriveController(IDriveBL driveBL, IMapper mapper)//, MatchingFunctionBL matchingFunctionBL)
        {
            this.driveBL = driveBL;
            _mapper = mapper;
            //   this.matchingFunctionBL = matchingFunctionBL;
        }


        // GET: api/<DriveController> /5  
        [HttpGet("FutureDrives/{userId}")]
        public async Task<ActionResult<List<DriveDTO>>> GetAsync(int userId)
        {
            var futureDrives= await driveBL.GetFutureDrivesBLAsync(userId);
            List<DriveDTO> futureDrivesDTO=new List<DriveDTO>();
            if(futureDrives!=null)
            {
            foreach (var drive in futureDrives)
            {
                futureDrivesDTO.Add( _mapper.Map<Drive, DriveDTO>(drive));
            }
            }
            return futureDrivesDTO;
        }


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
