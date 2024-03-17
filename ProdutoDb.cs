using apiMinima;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Produtos> Produtos { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=ProdutosDb;Trusted_Connection=True;TrustServerCertificate=true;");
    }

}