using domain.Interfaces.Repositories;
using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructure.Context;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class UserRepository:IBaseRepository<User>
    {
    
        private readonly TaskContext _taskContext;
        private readonly ObjResponse _response;
        private readonly SignInManager<IdentityUser> _signInManager;
       
        public UserRepository(TaskContext taskContext, ObjResponse response, 
            SignInManager<IdentityUser> signInManager) 
        { 
            _response = response;
            _taskContext = taskContext; 
            _signInManager = signInManager;
        }

        public async Task<ObjResponse> Create(User entity) 
        {
            try 
            { 
                await _taskContext.Users.AddAsync(entity);
                var res= await _taskContext.SaveChangesAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Code = 200;
                _response.Message = "SUCCESSFUL";
                return _response;
            }
            catch(Exception e) 
            { 
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "Bad Request";
                return _response;   
            }   
        }

        public async Task<ObjResponse> SignIn(User user)
        {
            try
            {
                var identityUser = new IdentityUser
                {
                    UserName = user.Name,
                    PasswordHash = user.Password
                };

                var signResult = await _signInManager.PasswordSignInAsync(identityUser, user.Password, false, lockoutOnFailure: false);
                var response = await GetGoodResponse();
                response.IsSign = signResult.Succeeded;
                return response;
            }
            catch (Exception e)
            {
                return await GetBadResponse();
            }

        }
        public async Task<ObjResponse> GetUserByName(string userName) 
        {
            try
            {
              var user =await _taskContext.Users.FirstOrDefaultAsync(f=> f.Name==userName);
              var response =await GetGoodResponse();
              response.user = user;
            return response;    
            
            }
            catch (Exception e) 
            {
                return await GetBadResponse();
            }
        }
        public async Task<ObjResponse> GetGoodResponse()
        {
            _response.StatusCode = HttpStatusCode.OK;
            _response.Code = 200;
            return await Task.FromResult(_response);
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
