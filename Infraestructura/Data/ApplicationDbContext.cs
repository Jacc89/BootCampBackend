using Core.Models;
using Microsoft.EntityFrameworkCore;
using Infraestructura.Data.Config;

namespace Infraestructura.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new EmpleadoConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());
            modelBuilder.ApplyConfiguration(new RemisionConfiguration());

        }

        public DbSet<Cliente> TbCliente { get; set; }
        public DbSet<Empleado> TbEmpleado { get; set; }
        public DbSet<Producto> TbProducto { get; set; }
        public DbSet<Remision> TbRemision { get; set; }
    }
}