using AutoMapper;
using BL;
using DL;
using DTO;
using Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Volunteers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
 
    public class DriverController : ControllerBase
    {
        IMapper mapper;
        IDriverBL driverBL;
        IImageBL ImageBL;

        public DriverController(IDriverBL driverBL, IMapper mapper, IImageBL ImageBL)
        {
            this.driverBL = driverBL;
            this.mapper = mapper;
            this.ImageBL = ImageBL;
        }
        // GET api/<DriverController>/5
        [HttpGet("{driverId}")]
        public async Task<ActionResult<Driver>> GetAsync(int driverId)
        {
            return await driverBL.GetDriverBLAsync(driverId);
        }

        [HttpGet("{id}/DriverDTO")]
        public async Task<ActionResult<DriverDTO>> GetDriverDTOAsync(int id)
        {
            var d= await driverBL.GetDriverBLAsync(id);
            if (d == null)
                return NoContent();//204 not foand
            var dDTO= mapper.Map<Driver, DriverDTO>(d);
            return Ok(dDTO);
        }

        //GetAllDriversDLAsync
        // GET api/<DriverController>
        [HttpGet()]
       
        public async Task<ActionResult<List<DriverDTO>>> GetAllDriversAsync()
        {
            var d = await driverBL.GetAllDriversBLAsync();
           List < DriverDTO > dDTO=new List<DriverDTO>();
            if (d == null)
                return NoContent();//204 not foand
            foreach (var item in d)
            {
                dDTO.Add(mapper.Map<Driver, DriverDTO>(item));

            }
            
            return Ok(dDTO);
           
        }


        [HttpGet]
        [Route("GetImagesForUser")]
        
        public async Task<ActionResult<string>> GetImagesForUser(int driverId)
        {
            string json = string.Empty;
            var img = await ImageBL.GetImagesForUser(driverId);
            json = JsonConvert.SerializeObject(img);
            return json;
        }

        // POST api/<DriverController>
        [HttpPost]
        public async Task<Driver> PostAsync([FromBody] DriverDTO value)
        {
           //string image = ImageBL.SaveImage(value.DriverLicense,(int )value.UserId);
            return await driverBL.PostDriverBLAsync(value);
        }

        //for image
        [HttpPost("{driverId}")]
        public async Task<int> PostAsync(int driverId, [FromForm] IFormFile image)
        {           
            var folderName = Path.Combine("Resources", "Images", driverId.ToString());
            var directory = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            Directory.CreateDirectory(directory);
            string ImageFullPath = Path.Combine(folderName, image.FileName);
            string filePath = Path.Combine(directory, image.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
          
           return await driverBL.PutOnlyDriverBLAsync(driverId, ImageFullPath);

            //string image = ImageBL.SaveImage(value.DriverLicense,(int )value.UserId);
        }



        

        // PUT api/<DriverController>/5
        [HttpPut("{driverId}")]//("{DriverId}")
        public async Task<Driver> PutAsync( int driverId, [FromBody] UserPerson value)
        {
            return await driverBL.PutDriverBLAsync(driverId, value);
        }

        [HttpPut("{driverId}/DriverDTO")]//("{DriverId}")
        public async Task<Driver> PutAsync(int driverId, [FromBody] DriverDTO value)
        {
            return await driverBL.PutDriverDTOBLAsync(driverId, value);//למה 2 פונקציות????????????????????????
        }




        [HttpPut("confirmDriver/{driverId}")]//("{DriverId}")
        public async Task<int> PutAsync(int driverId,Driver d)
        {
            return await driverBL.ConfirmDriverBLAsync(driverId,d);
        }

        // DELETE api/<DriverController>/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {
             await driverBL.DeleteDriverBLAsync(id);
        }
    }
}
