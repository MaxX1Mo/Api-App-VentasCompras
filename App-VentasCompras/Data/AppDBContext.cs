using Microsoft.EntityFrameworkCore;
using App_VentasCompras.Models;

namespace App_VentasCompras.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ProductoVenta> ProductoVentas { get; set; }
        public DbSet<Valoracion> Valoraciones { get; set; }
        public DbSet<Categoria> Categorias { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Configuraciones de las tablas
            //PRODUCTO
            modelBuilder.Entity<Producto>(tb =>
            {
                tb.HasKey(c => c.IDProducto);
                tb.Property(c => c.IDProducto)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(p => p.NombreProducto)
                .HasMaxLength(100)
                .IsRequired();

                tb.Property(p => p.Descripcion)
               .HasMaxLength(500);

                tb.Property(p => p.Precio)
               .HasPrecision(18, 2);


            });
            //ProductoVenta
            modelBuilder.Entity<ProductoVenta>(tb =>
            {
                tb.HasKey(c => c.IDProductoVenta);
                tb.Property(c => c.IDProductoVenta)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            });

            //Categoria
            modelBuilder.Entity<Categoria>(tb =>
            {
                tb.HasKey(c => c.IDCategoria);
                tb.Property(c => c.IDCategoria)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(u => u.Nombre)
                .HasMaxLength(50)
                .IsRequired();
            });

            //USUARIO
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(c => c.IDUsuario);
                tb.Property(c => c.IDUsuario)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired();

                tb.Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

                tb.Property(u => u.Password)
               .HasMaxLength(255)
               .IsRequired();

            });
            //PERSONA
            modelBuilder.Entity<Persona>(tb =>
            {
                tb.HasKey(c => c.IDPersona);
                tb.Property(c => c.IDPersona)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(p => p.Nombre)
               .HasMaxLength(50)
               .IsRequired();

                tb.Property(p => p.Apellido)
                .HasMaxLength(50)
                .IsRequired();

                tb.Property(p => p.NroCelular)
                .HasMaxLength(20);
            });

            //Ubicacion
            modelBuilder.Entity<Ubicacion>(tb =>
            {
                tb.HasKey(c => c.IDUbicacion);
                tb.Property(c => c.IDUbicacion)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

                tb.Property(u => u.CodigoPostal)
                .HasMaxLength(50);

            });

            //Status
            modelBuilder.Entity<Status>(tb =>
            {
                tb.HasKey(c => c.IDStatus);
                tb.Property(c => c.IDStatus)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();
            });

            //Valoraciones
            modelBuilder.Entity<Valoracion>(tb =>
            {
                tb.HasKey(c => c.IDValoraciones);
                tb.Property(c => c.IDValoraciones)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            });
            #endregion


            #region Relaciones Tablas
            // Usuario y Persona (1:1)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Persona)
                .WithOne(pe => pe.Usuario)
                .HasForeignKey<Persona>(u => u.IDUsuario)
                .OnDelete(DeleteBehavior.Cascade);  // eliminacion en cascada, cuando se elimna usuario se elimina persona por su relacion 1 a 1

            // Usuario y Producto (1:N)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Productos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.IDUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Usuario y Status (1:1)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Status)
                .WithOne(st => st.Usuario)
                .HasForeignKey<Status>(u => u.IDUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Status y valoracion (1:1)
            modelBuilder.Entity<Status>()
                .HasOne(st => st.Valoracion)
                .WithOne(v => v.Status)
                .HasForeignKey<Valoracion>(st => st.IDStatus)
                .OnDelete(DeleteBehavior.Cascade);

            // Usuario y Ubicacion (1:1)
            modelBuilder.Entity<Persona>()
             .HasOne(u => u.Ubicacion)
             .WithOne()
             .HasForeignKey<Persona>(u => u.IDUbicacion)
             .OnDelete(DeleteBehavior.Cascade); 



            // Producto y Categoria (N:1)
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Producto)
                .HasForeignKey(p => p.IDCategoria);

            // Producto y ProductoVenta(1:1)
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.ProductoVenta)
                .WithOne(pv => pv.Producto)
                .HasForeignKey<ProductoVenta>(pv => pv.IDProducto)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }

    }
}
