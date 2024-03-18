using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace apiMinima // Adicione esse namespace
{
    public class ProdutoDb : DbContext // Renomeie para ProdutoDbContext
    {
        public ProdutoDb(DbContextOptions<ProdutoDb> options) : base(options)
        {
        }

        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
    }
}