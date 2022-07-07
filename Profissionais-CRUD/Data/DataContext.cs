using Microsoft.EntityFrameworkCore;
using Profissionais_CRUD.Models;

namespace Profissionais_CRUD.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Profissional> Profissionais { get; set; }
    }
}
