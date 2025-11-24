using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceProveedor.Models;


namespace ServiceProveedor.Data
{
    public class ProveedorContext : DbContext
    {
        public ProveedorContext(DbContextOptions<ProveedorContext> options) : base(options) { }

        public DbSet<Proveedor> proveedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proveedor>()
                .Property(p => p.Estado)
                .HasDefaultValue(true);
        }
    }
}