using domain.Entities;
using domain.Interfaces.Repositories;
using Infraestructure.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class IdentityRepository:IBaseRepository<ObjIdentity>
    {
        private readonly TaskContext _taskContext;
        private  ObjResponse _response;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityRepository(TaskContext taskContext, ObjResponse response,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            ) 
        {
            _response = response;
            _taskContext = taskContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<ObjResponse> Create(ObjIdentity identity)
        {
            try
            {              
                var objIdentity = new IdentityUser
                {
                    UserName = identity.Name                 
                };
               
                var result= await _userManager.CreateAsync(objIdentity, identity.Password);
                string errors=string.Empty;
                if (result.Errors.ToList().Count > 0)
                {
                    foreach (var error in result.Errors.ToList())
                    {
                        errors += error.Description + ";";
                    }
                    _response = await GetBadResponse();
                    _response.Code = 400;
                    _response.Message = errors;
                    return _response;
                }
                else 
                {
                    return await GetGoodResponse();
                }               
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.BadGateway;
                _response.Code = 400;
                _response.Message = "Bad Request";
                return _response;
            }
        }

        public async Task<ObjResponse> SignIn(ObjIdentity identity) 
        {
            try 
            {
                ObjResponse res = new ObjResponse();
                var user =await _userManager.FindByNameAsync(identity.Name);
                var identityUser=new IdentityUser 
                 { 
                  UserName=user.UserName,
                  PasswordHash=user.PasswordHash
                 };
                //byte[] passwordInByte = Encoding.UTF8.GetBytes(identity.Password);
                //var response=await _signInManager.PasswordSignInAsync(identityUser,Convert.ToBase64String(passwordInByte), false, lockoutOnFailure:false);
                
                var response = await _signInManager.PasswordSignInAsync(user.UserName, identity.Password, true, true);
                
                if (response.Succeeded) 
                {
                    res.IsSign = true;
                    res.IsSign = response.Succeeded;
                }

                return res;
            }
            catch (Exception e) 
            {
                return await GetBadResponse();
            }
        }

        public async Task<ObjResponse> GetBadResponse()
        {
            _response.Message = "Bad Request";
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Code = 400;
            return await Task.FromResult(_response);
        }

        public async Task<ObjResponse> GetGoodResponse()
        {
            _response.StatusCode = HttpStatusCode.OK;
            _response.Code = 200;
            return await Task.FromResult(_response);
        }

        public Task<ObjResponse> GetUserByName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
