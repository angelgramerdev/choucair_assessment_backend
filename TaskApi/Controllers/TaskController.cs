using Application.Interfaces;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IServiceTask _serviceTask;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;    
        private ObjResponse _response;
        
        public TaskController(IServiceTask serviceTask, ObjResponse response, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            ) 
        { 
            _serviceTask = serviceTask;
            _response = response;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("create_task")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]Etask task) 
        {
            try
            {
               
                _response =await _serviceTask.Create(task);
                return Ok(_response);
            }
            catch (Exception e) 
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
