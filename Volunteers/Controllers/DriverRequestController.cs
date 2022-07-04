using BL;
using DTO;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Volunteers.Controllers
{[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DriverRequestController : ControllerBase
    {
        IDriverRequestBL DriverRequestBL;
        IMatchingFunctionBL matchingFunctionBL;
       
        public DriverRequestController(IDriverRequestBL permanentRideBL, IMatchingFunctionBL matchingFunctionBL)
        {
            this.DriverRequestBL = permanentRideBL;
            this.matchingFunctionBL = matchingFunctionBL;

        }

        // GET api/<PermanentRideController>/5
        [HttpGet("{DriverId}")]
        public async Task<List<DriverRequest>> GetByDriverIdAsync(int DriverId)
        {
            var drive = await DriverRequestBL.GetDriverRequestBL(DriverId);
            return drive;
        }

        // POST api/<PermanentRideController>
        [HttpPost]
        public async Task<DriverRequest> PostAsync([FromBody] DriverRequest d)
        {
            DriverRequest dr= await DriverRequestBL.PostDriverRequestBL(d);
            await matchingFunctionBL.MatchingFunctionForDriverReq(d);
            return dr;
        }

        // PUT api/<PermanentRideController>/5
        [HttpPut("{id}")]
        public async Task<DriverRequest> PutAsync(int id,[FromBody] DriverRequest d)
        {
            return await DriverRequestBL.PutDriverRequestBL(id,d);
        }
        // DELETE api/<PermanentRideController>/5
        [HttpDelete]
        public async void Delete([FromBody] DriverRequestDTO dr)
        {

            await matchingFunctionBL.MatchingFunctionForCancelDriverRequest(dr);
        }
    }
}
