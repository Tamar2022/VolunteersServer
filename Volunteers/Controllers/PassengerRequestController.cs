using BL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860 

namespace Volunteers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerRequestController : ControllerBase
    {
        IPassengerRequestBL PassengerRequestBL;
        IMapper mapper;
        IMatchingFunctionBL matchingFunctionBL;
        public PassengerRequestController(IPassengerRequestBL PassengerRequestBL, IMapper mapper, IMatchingFunctionBL matchingFunctionBL)
        {
            this.PassengerRequestBL = PassengerRequestBL;
            this.mapper = mapper;
            this.matchingFunctionBL = matchingFunctionBL;
        }


        //return PassengerRequest by user id which has done?
        // GET api/<PassengerRequest>/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<PassengerRequest>>> GetAsync(int userId)
        {
             return await PassengerRequestBL.GetPassengerRequestBLAsync(userId);
            
        }



        // POST api/<
        // Request>
        [HttpPost]
        public async Task<PassengerRequest> PostAsync([FromBody] PassengerRequest value)
        {
             PassengerRequest pr= await PassengerRequestBL.PostPassengerRequestBLAsync(value);
            if(pr!=null)
                await matchingFunctionBL.MatchingFunctionForPassengerRequest(value);
            return pr;
        }

        [HttpPost ("PassengerRequestDTO")]
       
        public async Task<PassengerRequestDTO> PostPassengerRequestDTOAsync([FromBody] PassengerRequest value)
        {
            var pr= await PassengerRequestBL.PostPassengerRequestBLAsync(value);
            
            return mapper.Map<PassengerRequest, PassengerRequestDTO>(pr);
        }

        // PUT api/<PassengerRequest>/5
        [HttpPut("{id}")]
        public async Task<PassengerRequest> PutAsync(int id,[FromBody] PassengerRequest value)
        {
            return await PassengerRequestBL.PutPassengerRequestBLAsync(id,value);  
        }

        // DELETE api/<PassengerRequest>/5
        [HttpDelete]
        public async Task DeleteAsync([FromBody] PassengerRequest pr)
        {

            await matchingFunctionBL.MatchingFunctionForCancelPassengerRequest(pr);      
                }
    }
}
