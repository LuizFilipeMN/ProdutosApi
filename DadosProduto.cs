using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiMinima
{
    public class Produtos
    {
        //Nome do produto;
        //Código do produto;
        //Preço;
        //Descrição;
        //Quantidade em estoque;
        //Avaliação;
        //Categoria;

        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int Codigo { get; set; }
        public float Preco { get; set; }
        public float Quantidade { get; set; }
        public string? Descricao { get; set; }
        public int Avaliacao { get; set; }
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public string? Categoria { get; set; }
        public bool TemCadastro { get; set; }

    }

    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<Produtos>? Produtos { get; set; }
    }

}