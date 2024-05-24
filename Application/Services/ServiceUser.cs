using Application.Interfaces;
using domain.Entities;
using domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceUser:IServiceUser
    {

        
        private readonly IBaseRepository<User> _baseRepository;
        private readonly ObjResponse _response;
        
        public ServiceUser(IBaseRepository<User> baseRepository, ObjResponse response) 
        { 
            _baseRepository = baseRepository;
            _response = response;   
        }

        public async Task<ObjResponse> Create(User user) 
        {
            try
            {
                if (user == null) 
                {
                    return await GetBadResponse();   
                }
                var res=await _baseRepository.Create(user);
                return res;
            }
            catch(Exception e) 
            {
                return GetBadResponse().Result;
            }
        }

        public async Task<ObjResponse> GetUserByName(string userName) 
        {
            try
            {
                var response=await _baseRepository.GetUserByName(userName);
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
