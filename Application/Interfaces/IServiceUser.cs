using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain.Entities;

namespace Application.Interfaces
{
    public interface IServiceUser
    {
        Task<ObjResponse> Create(User user);
        Task<ObjResponse> GetUserByName(string userName);
    }
}
