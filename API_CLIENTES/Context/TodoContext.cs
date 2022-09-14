using API_CLIENTES.Models;
using Microsoft.EntityFrameworkCore;

namespace API_CLIENTES.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {

        }
        public DbSet<Clientes> Clientes => Set<Clientes>();
        public DbSet<Ciudades> Ciudades => Set<Ciudades>();
    }
}
