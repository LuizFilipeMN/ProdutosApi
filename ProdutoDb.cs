using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace apiMinima
{
    public class ProdutoDb : DbContext
    {
        public ProdutoDb(DbContextOptions<ProdutoDb> options) : base(options)
        {
        }

        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
    }
}