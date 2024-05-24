using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using domain.Entities;
using Application.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IServiceAuthentication _serviceAuthentication;
        private readonly ObjResponse _response;  
        private readonly IConfiguration _config;

        public AccountController(IServiceAuthentication serviceAuthentication, 
            ObjResponse response,
            IConfiguration config
            
            ) 
        { 
            _serviceAuthentication=serviceAuthentication;
            _response=response;
            _config=config;
        }

        [HttpPost]
        [Route("register_user")]
        [AllowAnonymous]
        public async Task<IActionResult>Register([FromBody]ObjIdentity identity) 
        {
            try
            {
                var response=await _serviceAuthentication.CreateIdentity(identity);
                return Ok(response); 
            }
            catch(Exception e) 
            {
                var response= await GetBadResponse();
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("login_user")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ObjIdentity identity) 
        {
            try
            {
                string token = string.Empty;
                var response = await _serviceAuthentication.SignIn(identity);
                if (response.IsSign)                  
                     token = GenerateToken(identity);

                return Ok(new {token=token });
            }
            catch (Exception e) 
            {
                return BadRequest(await GetBadResponse());   
            }
        }

        public async Task<ObjResponse> GetBadResponse()
        {
            _response.Message = "Bad Request";
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Code = 400;
            return await Task.FromResult(_response);
        }

        public string GenerateToken(ObjIdentity identity) 
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, identity.Name),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("jwt:key").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var securityToken =new JwtSecurityToken(claims:claims, expires:DateTime.Now.AddMinutes(60), signingCredentials:credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
