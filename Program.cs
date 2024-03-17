using apiMinima;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Project")));

var app = builder.Build();

app.MapGet("/produtos", async (ApplicationContext db) => await db.Produtos.ToListAsync());

app.MapGet("/produtos/cadastro", async (ApplicationContext db) => await db.Produtos.Where(t => t.TemCadastro).ToListAsync());

app.MapGet("/produtos/semcadastro", async (ApplicationContext db) => await db.Produtos.Where(t => !t.TemCadastro).ToListAsync());

app.MapGet("/produtos/{id}", async (ApplicationContext db, int id) =>
{
    var produto = await db.Produtos.FindAsync(id);
    return produto is not null ? Results.Ok(produto) : Results.NotFound();
});

app.MapGet("/produtos/categoria/{categoriaId}", async (ApplicationContext db, int categoriaId) =>
{
    var produtos = await db.Produtos.Where(p => p.CategoriaId == categoriaId).ToListAsync();
    return produtos.Any() ? Results.Ok(produtos) : Results.NotFound();
});

app.MapGet("/produtos/codigo/{codigo}", async (ApplicationContext db, int codigo) =>
{
    var produto = await db.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo);
    return produto is not null ? Results.Ok(produto) : Results.NotFound();
});

app.MapPost("/produtos/adicionar", async (Produtos produto, ApplicationContext db) =>
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.Id}", produto);
});

app.MapPut("/produtos/editar/{id}", async (ApplicationContext db, int id, Produtos inputProduto) =>
{
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

app.MapDelete("/produtos/{id}", async (ApplicationContext db, int id) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
        return Results.NotFound();

    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();

    return Results.Ok(produto);
});

app.Run();
