using apiMinima;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;

});

builder.Services.AddDbContext<ProdutoDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/produtos", async (HttpContext context) => {
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    return await db.Produtos.ToListAsync();
});

app.MapGet("/produtos/cadastro", async (HttpContext context) => {
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    return await db.Produtos.Where(t => t.TemCadastro).ToListAsync();
});

app.MapGet("/produtos/semcadastro", async (HttpContext context) => {
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    return await db.Produtos.Where(t => !t.TemCadastro).ToListAsync();
});

app.MapGet("/produtos/{id}", async (HttpContext context) => {
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var produto = await db.Produtos.FindAsync(id);
    return produto is not null ? Results.Ok(produto) : Results.NotFound();
});

app.MapGet("/produtos/categoria/{categoria}", async (HttpContext context) => {
    var categoria = context.Request.RouteValues["categoria"].ToString();
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    if (int.TryParse(categoria, out int categoriaId))
    {
        var produtos = await db.Produtos.Where(p => p.CategoriaId == categoriaId).ToListAsync();
        return produtos.Any() ? Results.Ok(produtos) : Results.NotFound();
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapGet("/produtos/codigo/{codigo}", async (HttpContext context) => {
    var codigo = int.Parse(context.Request.RouteValues["codigo"].ToString());
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var produto = await db.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo);
    return produto is not null ? Results.Ok(produto) : Results.NotFound();
});

app.MapPost("/produtos/adicionar", async (HttpContext context) =>
{
    var produto = await context.Request.ReadFromJsonAsync<Produtos>();
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();

    if (produto.CategoriaId > 0)
    {
        var categoria = await db.Categoria.FindAsync(produto.CategoriaId);
        if (categoria != null)
        {
            produto.Categoria = categoria;
        }
        else
        {
            throw new InvalidOperationException("Categoria não encontrada.");
        }
    }

    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.Id}", produto);
});

app.MapPut("/produtos/editar/{id}", async (HttpContext context) => {
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    var inputProduto = await context.Request.ReadFromJsonAsync<Produtos>();
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
        return Results.NotFound();

    produto.Nome = inputProduto.Nome;
    produto.Codigo = inputProduto.Codigo;
    produto.Preco = inputProduto.Preco;
    produto.Quantidade = inputProduto.Quantidade;
    produto.Descricao = inputProduto.Descricao;
    produto.Avaliacao = inputProduto.Avaliacao;
    produto.CategoriaId = inputProduto.CategoriaId;
    produto.TemCadastro = inputProduto.TemCadastro;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/produtos/{id}", async (HttpContext context) => {
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
        return Results.NotFound();

    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();
    return Results.Ok(produto);
});

app.MapGet("/categorias", async (HttpContext context) =>
{
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var categorias = await db.Categoria.ToListAsync();
    return Results.Ok(categorias);
});

app.MapGet("/categorias/{id}", async (HttpContext context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var categoria = await db.Categoria.FindAsync(id);
    return categoria is not null ? Results.Ok(categoria) : Results.NotFound();
});

app.MapPost("/categorias", async (HttpContext context) =>
{
    var categoria = await context.Request.ReadFromJsonAsync<Categoria>();
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    db.Categoria.Add(categoria);
    await db.SaveChangesAsync();
    return Results.Created($"/categorias/{categoria.Id}", categoria);
});

app.MapPut("/categorias/{id}", async (HttpContext context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    var inputCategoria = await context.Request.ReadFromJsonAsync<Categoria>();
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var categoria = await db.Categoria.FindAsync(id);
    if (categoria is null)
        return Results.NotFound();

    categoria.Nome = inputCategoria.Nome;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/categorias/{id}", async (HttpContext context) =>
{
    var id = int.Parse(context.Request.RouteValues["id"].ToString());
    await using var db = context.RequestServices.GetRequiredService<ProdutoDb>();
    var categoria = await db.Categoria.FindAsync(id);
    if (categoria is null)
        return Results.NotFound();

    db.Categoria.Remove(categoria);
    await db.SaveChangesAsync();
    return Results.Ok(categoria);
});

app.Run();