using Application.Interfaces;
using domain.Entities;
using domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceAuthentication : IServiceAuthentication
    {

        private readonly IBaseRepository<ObjIdentity> _baseIdentityRepository;
        private readonly IBaseRepository<User> _baseUserRepository;
        private readonly ObjResponse _response;
        
        
        public ServiceAuthentication(IBaseRepository<ObjIdentity> baseIdentityRepository, 
            IBaseRepository<User> baseUserRepository ,
            ObjResponse response) 
        { 
            _baseIdentityRepository = baseIdentityRepository;
            _baseUserRepository = baseUserRepository;   
            _response = response;
        }
        
        public async Task<ObjResponse> CreateIdentity(ObjIdentity identity)
        {
            try 
            {
                if(identity==null) 
                { 
                    return await GetBadResponse();    
                }
                var respose=await _baseIdentityRepository.Create(identity);
                if(respose.Code==200) 
                {
                    User user = new User
                    {
                        CreationDate = DateTime.Now,
                        Name = identity.Name,  
                        Password  =  identity.Password
                    };
                   await _baseUserRepository.Create(user);
                }
               return await GetGoodResponse();   
            }
            catch(Exception e) 
            { 
                return await GetBadResponse();    
            }
        }

        public async Task<ObjResponse> SignIn(ObjIdentity identity) 
        {
            try
            {
             var response=await _baseIdentityRepository.SignIn(identity);
             if (!response.IsSign) 
                {
                    response.Message = "Invalid user or password";
                    response.StatusCode=HttpStatusCode.Unauthorized;
                    response.Code = 405;
                }
             return response;
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

    }
}
