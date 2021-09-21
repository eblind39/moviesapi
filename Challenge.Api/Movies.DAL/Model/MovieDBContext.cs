using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Movies.EL.Configurations;
using Movies.EL.Model;
using Movies.EL.Model.Auxiliar;

namespace Movies.DAL.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public partial class MovieDBContext : DbContext
    {
        /// <summary>DB's Connection string</summary>
        private string ConnectionString;

        public MovieDBContext()
        {
        }


        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MovieDBContext" />.
        /// </summary>
        /// <param name="configuration">La configuracion.</param>
        public MovieDBContext(IOptions<BackEndConfiguration> configuration)
        {
            ConnectionString = configuration.Value.DBConnection;
        }

        public MovieDBContext(DbContextOptions<MovieDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<Rental> Rental { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                // optionsBuilder.UseSqlite(ConnectionString);
                optionsBuilder.UseInMemoryDatabase(databaseName: "InMemoryDB").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.movieId)
                    .HasName("movieId");

                entity.Property(e => e.title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.stock)
                    .IsRequired()
                    .HasColumnType("integer");

                entity.Property(e => e.rentalPrice)
                    .IsRequired()
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.salePrice)
                    .IsRequired()
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.available)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                //entity.HasMany(d => d.Sales)
                //    .WithOne(p => p.movieIdNavigation)
                //    .HasForeignKey(d => d.movieId)
                //    .OnDelete(DeleteBehavior.Cascade)
                //    .HasConstraintName("Movies_FK_Sales");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.movieId)
                    .IsRequired()
                    .HasColumnType("integer");

                entity.Property(e => e.customerEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.price)
                    .IsRequired()
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                //entity.HasOne(d => d.movieIdNavigation)
                //    .WithMany(p => p.Sales)
                //    .HasForeignKey(d => d.movieId)
                //    .OnDelete(DeleteBehavior.Cascade)
                //    .HasConstraintName("Sales_FK_Movies");
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.movieId)
                    .IsRequired()
                    .HasColumnType("integer");

                entity.Property(e => e.customerEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.price)
                    .IsRequired()
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                //entity.HasOne(d => d.movieIdNavigation)
                //    .WithMany(p => p.Sales)
                //    .HasForeignKey(d => d.movieId)
                //    .OnDelete(DeleteBehavior.Cascade)
                //    .HasConstraintName("Sales_FK_Movies");
            });

            modelBuilder.Entity<Likes>(entity =>
            {
                entity.HasKey(e => e.movieId);

                entity.Property(e => e.likes)
                    .IsRequired()
                    .HasColumnType("integer");

                //entity.Property(e => e._customers)
                //    .HasMaxLength(36000)
                //    .IsUnicode(false);

                //entity.HasOne(d => d.movieIdNavigation)
                //    .WithMany(p => p.Sales)
                //    .HasForeignKey(d => d.movieId)
                //    .OnDelete(DeleteBehavior.Cascade)
                //    .HasConstraintName("Sales_FK_Movies");
            });

            //modelBuilder.Entity<Sale>()
            //            .HasOne(i => i.movieIdNavigation)
            //            .WithMany(c => c.Sales)
            //            .HasForeignKey(f => f.movieId)
            //            .IsRequired()
            //            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasMany(c => c.Movies)
                .WithOne(s => s.Sale)
                .HasForeignKey(f => f.movieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rental>()
                .HasMany(c => c.Movies)
                .WithOne(s => s.Rental)
                .HasForeignKey(f => f.movieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Likes>()
            //    .HasMany(c => c.Movies)
            //    .WithOne(s => s.Likes)
            //    .HasForeignKey(f => f.movieId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Cascade);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added ||
                    auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.Modified = DateTime.Now;

                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.Created = DateTime.Now;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
