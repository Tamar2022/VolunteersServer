using BL;
using DTO;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Volunteers.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        ILogger<UserController> logger;
        IUserBL userBL;
        public UserController(IUserBL userBL, ILogger<UserController> logger)
        {
            this.userBL = userBL;
            this.logger = logger;
        }
        // GET: by userName and password
        [HttpPost("Get")]
        [AllowAnonymous]

        public async Task<ActionResult<UserPerson>> GetAsync(userDTO userDTO )
        {
            try { 

            return await userBL.GetUserBLAsync(userDTO.Email, userDTO.Password);
             
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
            }
            return null;
        }
        //[HttpPost("GetUserType/userType")]
      

        //public async Task<ActionResult<int>> GetUserTypeAsync(userDTO userDTO)
        //{
        //    try
        //    {

        //        return await userBL.GetUserTypeBLAsync(userDTO.Email, userDTO.Password);

        //    }
        //    catch (Exception e)
        //    {
        //        logger.LogError(e.Message);
        //    }
        //    return null;
        //}


        // GET: get all passangers
        [HttpGet("{typeId}")]//למי יש הרשאה לגשת לזה
        
        
        public async Task<ActionResult<List<UserPerson>>> GetAllByTypeAsync( int typeId)
        {
            
           return await userBL.GetUserBLAsync(typeId);
            
        }
        //PostByPersonAndTypeId
        [HttpPost("{typeId}")]
       
        public async Task<UserPerson> PostAsync([FromBody] Person value, int typeId)
        {
            return await userBL.PostUserBLAsync(value, typeId);
        }

        //PUT
        [HttpPut("{personId}")]
        
        public async Task<Person> PutAsync(int personId, [FromBody] Person pd)
        {       
           return await userBL.PutUserBLAsync(personId, pd);             
        }


        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
