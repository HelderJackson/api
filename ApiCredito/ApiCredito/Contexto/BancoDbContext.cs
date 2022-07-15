using ApiCredito.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiCredito.Contexto
{
    public class BancoDbContext : DbContext
    {
        public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options) { }

       public DbSet<Credito> Creditos { get; set; }
    }
}