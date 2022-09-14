using API_CLIENTES.Context;
using API_CLIENTES.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddDbContext<TodoContext>(options =>
options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Agregar Clientes
app.MapPost("api/clientes/agregar/", async (Clientes cliente, TodoContext db) =>
{
    db.Clientes.Add(cliente);
    await db.SaveChangesAsync();

    return Results.Created($"api/clientes/agregar/{cliente.Id}", cliente);
});

// Obtener Cliente por id
app.MapGet("api/clientes/obtener/{id:int}", (int id, TodoContext db) =>
{
    var query = db.Clientes
                           .Where(s => s.Id == id)
                           .Include(s => s.Ciudades)
                           .FirstOrDefault();

    return Task.FromResult(query);
});

// Editar Cliente
app.MapPut("api/clientes/editar/{id:int}", async (int id, Clientes inputTodo, TodoContext db) =>
{
    var todo = await db.Clientes.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.IdCiudad = inputTodo.IdCiudad;
    todo.Nombres = inputTodo.Nombres;
    todo.Apellidos = inputTodo.Apellidos;
    todo.Documento = inputTodo.Documento;
    todo.Telefono = inputTodo.Telefono;
    todo.Email = inputTodo.Email;
    todo.FechaNacimiento = inputTodo.FechaNacimiento;
    todo.Ciudad = inputTodo.Ciudad;
    todo.Nacionalidad = inputTodo.Nacionalidad;

    await db.SaveChangesAsync();

    return Results.NoContent();
});


// Eliminar Cliente
app.MapDelete("api/clientes/eliminar/{id}", async (int id, TodoContext db) =>
{
    if (await db.Clientes.FindAsync(id) is Clientes todo)
    {
        db.Clientes.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

// Agregar Ciudades
app.MapPost("api/ciudades/agregar/", async (Ciudades ciudades, TodoContext db) =>
{
    db.Ciudades.Add(ciudades);
    await db.SaveChangesAsync();

    return Results.Created($"api/ciudades/agregar/{ciudades.Id}", ciudades);
});

// Obtener Ciudades por id
app.MapGet("api/ciudades/obtener/{id:int}", (int id, TodoContext db) =>
{


    var ciudad = db.Ciudades
                           .Where(s => s.Id == id)
                           .Include(s => s.Cliente)
                           .FirstOrDefault();

    return Task.FromResult(ciudad);
});

// Editar Ciudades
app.MapPut("api/ciudades/editar/{id:int}", async (int id, Ciudades inputTodo, TodoContext db) =>
{
    var todo = await db.Ciudades.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Ciudad = inputTodo.Ciudad;
    todo.Estado = inputTodo.Estado;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Eliminar Ciudades
app.MapDelete("api/ciudades/eliminar/{id}", async (int id, TodoContext db) =>
{
    if (await db.Ciudades.FindAsync(id) is Ciudades todo)
    {
        db.Ciudades.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}