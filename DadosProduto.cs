using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace apiMinima
{
    public class Produtos
    {
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
        public Categoria? Categoria { get; set; }
        public bool TemCadastro { get; set; }
    }

    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }

        [JsonIgnore]
        public ICollection<Produtos>? Produtos { get; set; }
    }
}
