using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class ObjResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSign { get; set; }
        public User user { get; set; }
        public Etask task { get; set; }
        public List<Etask> tasks { get; set; }
        public List<User> users { get; set; }

    }
}
