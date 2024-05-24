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
    public class ServiceTask : IServiceTask
    {

        private readonly ITaskRepository<Etask> _taskRepository;
        private ObjResponse _response;
        private readonly IBaseRepository<User> _baseRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ServiceTask(ITaskRepository<Etask> taskRepository, 
            ObjResponse response, IBaseRepository<User> baseRepository,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager

            ) 
        { 
            _response = response;
            _taskRepository = taskRepository;
            _baseRepository = baseRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }    
        
        public async Task<ObjResponse> Create(Etask task)
        {
            try
            {
                _response=await _taskRepository.Create(task);
                return _response;    
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
