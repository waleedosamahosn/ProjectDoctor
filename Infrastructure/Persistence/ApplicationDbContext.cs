
using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {  
        }


        public DbSet<Patient> Patients {  get; set; }
    }
}
