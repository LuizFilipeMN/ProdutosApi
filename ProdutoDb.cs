using Microsoft.EntityFrameworkCore;

namespace apiMinima
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<DadosProduto> Produtos { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
    }
}
