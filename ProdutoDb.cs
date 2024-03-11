using Microsoft.EntityFrameworkCore;

namespace apiMinima
{
    public class ProdutoDb : DbContext
    {
        public ProdutoDb(DbContextOptions<ProdutoDb> options) :
            base(options) { }

        public DbSet<DadosProduto> DadosProduto => Set<DadosProduto>();
    }
}
