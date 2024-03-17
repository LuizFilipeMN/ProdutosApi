using apiMinima;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/produtos", async (ApplicationContext db) => await db.DadosProduto.ToListAsync());

app.MapGet("/produtos/cadastro", async (ApplicationContext db) => await db.DadosProduto.Where(t => t.TemCadastro).ToListAsync());

app.MapGet("/produtos/semcadastro", async (ApplicationContext db) => await db.DadosProduto.Where(t => !t.TemCadastro).ToListAsync());

app.MapGet("/produtos/{id}", async (ApplicationContext db, int id) =>
{
    var produto = await db.DadosProduto.FindAsync(id);
    return produto is not null ? Results.Ok(produto) : Results.NotFound();
});

app.MapGet("/produtos/categoria/{categoria}", async (ApplicationContext db, string categoria) =>
{
    var produtos = await db.DadosProduto.Where(p => p.Categoria == categoria).ToListAsync();
    return produtos.Any() ? Results.Ok(produtos) : Results.NotFound();
});

app.MapGet("/produtos/codigo/{codigo}", async (ApplicationContext db, int codigo) =>
{
    var produto = await db.DadosProduto.FirstOrDefaultAsync(p => p.Codigo == codigo);
    return produto is not null ? Results.Ok(produto) : Results.NotFound();
});

app.MapPost("/produtos/adicionar", async (DadosProduto produto, ApplicationContext db) =>
{
    db.DadosProduto.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.Id}", produto);

});

app.MapPut("/produtos/editar/{id}", async (ApplicationContext db, int id, DadosProduto inputProduto) =>
{
    var produto = await db.DadosProduto.FindAsync(id);

    if (produto is null)
        return Results.NotFound();

    produto.Nome = inputProduto.Nome;
    produto.Codigo = inputProduto.Codigo;
    produto.Preco = inputProduto.Preco;
    produto.Quantidade = inputProduto.Quantidade;
    produto.Descricao = inputProduto.Descricao;
    produto.Avaliacao = inputProduto.Avaliacao;
    produto.CategoriaId = inputProduto.CategoriaId;
    produto.Categoria = inputProduto.Categoria;
    produto.TemCadastro = inputProduto.TemCadastro;

    await db.SaveChangesAsync();

    return Results.NoContent();
});


app.MapDelete("/produtos/{id}", async (ApplicationContext db, int id) =>
{
    if (await db.DadosProduto.FindAsync(id) is DadosProduto produto)
    {
        db.DadosProduto.Remove(produto);
        await db.SaveChangesAsync();
        return Results.Ok(produto);
    }
    return Results.NotFound();
});

app.Run();