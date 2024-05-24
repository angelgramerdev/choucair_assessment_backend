using Microsoft.EntityFrameworkCore;
using domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace Infraestructure.Context
{
    public class TaskContext: IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public TaskContext(DbContextOptions<TaskContext> options)
            :base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Etask> Etasks { get; set; }
    }
}
