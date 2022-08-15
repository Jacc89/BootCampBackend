using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
           builder.Property(e => e.Id).IsRequired();
           builder.Property(e => e.Nombres).IsRequired().HasMaxLength(100);
           builder.Property(e => e.Apellidos).IsRequired().HasMaxLength(100);
           builder.Property(e => e.Direccion).IsRequired().HasMaxLength(150);
           builder.Property(e => e.Telefono).IsRequired().HasMaxLength(40);
           builder.Property(e => e.Correo).IsRequired().HasMaxLength(100);
           builder.Property(e => e.Sueldo).IsRequired().HasMaxLength(40);
           builder.Property(e => e.Cargo).IsRequired().HasMaxLength(50);
        }
    }
}