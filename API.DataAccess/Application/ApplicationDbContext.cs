using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Application
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ConnectionList> ConnectionLists { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ConnectionDetail> ConnectionDetails { get; set; }
        public DbSet<ConnectionDetailFilter> ConnectionDetailFilters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConnectionList>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.ImageUrl)
                    .HasColumnType("text");

                entity.Property(c => c.Server)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Connection>(entity =>
            {
                entity.HasIndex(c => c.UserId);

                entity.Property(c => c.UserId)
                    .IsRequired();

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.Path)
                    .IsRequired()
                    .HasColumnType("varchar(500)")
                    .HasMaxLength(500);

                entity.Property(c => c.CreatedBy)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.LastModifiedBy)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.HasIndex(c => c.Path)
                    .IsUnique();
            });

            modelBuilder.Entity<ConnectionDetail>(entity =>
            {
                entity.HasIndex(c => c.UserId);

                entity.HasIndex(c => c.ConnectionId);

                entity.HasIndex(c => c.ConnectionListId);

                entity.Property(c => c.UserId)
                    .IsRequired();

                entity.Property(c => c.ConnectionId)
                    .IsRequired();

                entity.Property(c => c.ConnectionListId)
                    .IsRequired();

                entity.Property(c => c.Email)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.Password)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(c => c.ConnectionType)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(60);

                entity.Property(c => c.Server)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.Port)
                    .IsRequired();

                entity.Property(c => c.CreatedBy)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.LastModifiedBy)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ConnectionDetailFilter>(entity =>
            {
                entity.HasIndex(c => c.UserId);

                entity.HasIndex(c => c.ConnectionDetailId);

                entity.Property(c => c.UserId)
                    .IsRequired();

                entity.Property(c => c.ConnectionDetailId)
                    .IsRequired();

                entity.Property(c => c.Key)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.Value)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.CreatedBy)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);

                entity.Property(c => c.LastModifiedBy)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasMaxLength(256);
            });
        }
    }
}
