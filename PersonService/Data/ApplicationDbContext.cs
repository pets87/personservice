using Microsoft.EntityFrameworkCore;
using PersonService.Models;

namespace PersonService.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonChangeLog> PersonChangeLogs { get; set; }

    }
}
