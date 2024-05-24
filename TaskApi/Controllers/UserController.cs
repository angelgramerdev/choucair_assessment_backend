using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using domain.Entities;
using Application.Interfaces;
using System.Net;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceUser _serviceUser;
        private readonly ObjResponse _response;
        public UserController(IServiceUser serviceUser,ObjResponse response) 
        { 
            _response = response;
            _serviceUser = serviceUser; 
        }

        [HttpPost]
        [Route("create_user")]
        public async Task<IActionResult> Create([FromBody]User user) 
        {
            try
            {
                if (user == null) 
                {
                    return Ok(await GetBadResponse());
                }
                var response=await _serviceUser.Create(user);  
                return Ok(response);
            }
            catch(Exception e) 
            {
                return Ok(await GetBadResponse());
            }
        }

        public async Task<ObjResponse> GetBadResponse()
        {
            _response.Message = "Bad Request";
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Code = 400;
            return await Task.FromResult(_response);
        }

    }
}
