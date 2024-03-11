namespace apiMinima
{
    public class DadosProduto
    {
        //Nome do produto;
        //Código do produto;
        //Preço;
        //Descrição;
        //Quantidade em estoque;
        //Avaliação;
        //Categoria.

        public int Id { get; set; }
        public string? Nome { get; set; }
        public int Codigo { get; set; }
        public float Preco { get; set; }
        public float Quantidade { get; set; }
        public string? Descricao { get; set; }
        public int Avaliacao { get; set; }
        public string? Categoria { get; set; }
        public bool TemCadastro { get; set; }





    }
}
