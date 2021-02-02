using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> db):base(db)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
