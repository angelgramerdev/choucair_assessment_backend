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
    public class TaskRepository : ITaskRepository<Etask>
    {
       
        
        private readonly TaskContext _taskContext;
        private ObjResponse _response;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public TaskRepository(TaskContext context, 
            ObjResponse response,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            ) 
        { 
            _taskContext = context;
            _response = response;
            _userManager = userManager;
            _signInManager = signInManager;
        } 
        public async Task<ObjResponse> Create(Etask entity)
        {
            try
            {
               var task=await _taskContext.Etasks.AddAsync(entity);
               await _taskContext.SaveChangesAsync();
                var response =await GetGoodResponse();
                response.task = task.Entity;
                
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
