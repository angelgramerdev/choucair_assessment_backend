using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Entities
{
    public class Etask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public bool IsCompleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public virtual User? User { get; set; }
        public int UserId { get; set;}
        
    
    }
}
