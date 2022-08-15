using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Config
{
    public class RemisionConfiguration : IEntityTypeConfiguration<Remision>
    {
        public void Configure(EntityTypeBuilder<Remision> builder)
        {
            builder.Property(r => r.Id).IsRequired();
            builder.Property(r => r.NumRemision).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Fecha).IsRequired().HasMaxLength(100);
            builder.Property(r => r.EncargadoId).IsRequired().HasMaxLength(100);
            builder.Property(r => r.ClienteId).IsRequired().HasMaxLength(150);
            builder.Property(r => r.Cantidad).IsRequired().HasMaxLength(40);
            builder.Property(r => r.Presentacion).IsRequired().HasMaxLength(100);
            builder.Property(r => r.ProductoId).IsRequired().HasMaxLength(40);
            builder.Property(r => r.Observaciones).IsRequired().HasMaxLength(500);


            // Todo Relaciones
            builder.HasOne(r => r.Empleado).WithMany()
                    .HasForeignKey(r => r.EncargadoId);

            builder.HasOne(r => r.Cliente).WithMany()
                    .HasForeignKey(r => r.ClienteId); 

            builder.HasOne(r => r.Producto).WithMany()
                    .HasForeignKey(r => r.ProductoId);       
        }
    }
}