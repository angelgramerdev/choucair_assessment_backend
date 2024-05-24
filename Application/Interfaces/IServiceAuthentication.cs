using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IServiceAuthentication
    {
        Task<ObjResponse> CreateIdentity(ObjIdentity identity);
        Task<ObjResponse> SignIn(ObjIdentity identity);
    }
}
