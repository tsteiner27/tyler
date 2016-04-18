using Microsoft.Data.Entity;
using TylerSteiner.Models;

namespace TylerSteiner.Cli.EntityFramework
{
    public class MoviesDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorMapping> ActorMappings { get; set; } 

        public DbSet<Cinematographer> Cinematographers { get; set; }
        public DbSet<CinematographerMapping> CinematographerMappings { get; set; } 

        public DbSet<Composer> Composers { get; set; }
        public DbSet<ComposerMapping> ComposerMappings { get; set; }
         
        public DbSet<Director> Directors { get; set; }
        public DbSet<DirectorMapping> DirectorMappings { get; set; }

        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributorMapping> DistributorMappings { get; set; }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreMapping> GenreMappings { get; set; }
         
        public DbSet<Producer> Producers { get; set; }
        public DbSet<ProducerMapping> ProducerMappings { get; set; } 

        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudioMapping> StudioMappings { get; set; } 

        public DbSet<Writer> Writers { get; set; }
        public DbSet<WriterMapping> WriterMappings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=movies;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movie>().ToTable("Movies");
            builder.Entity<Actor>().ToTable("Actors");
            builder.Entity<Cinematographer>().ToTable("Cinematographers");
            builder.Entity<Composer>().ToTable("Composers");
            builder.Entity<Director>().ToTable("Directors");
            builder.Entity<Genre>().ToTable("Genres");
            builder.Entity<Producer>().ToTable("Producers");
            builder.Entity<Studio>().ToTable("Studios");
            builder.Entity<Writer>().ToTable("Writers");

            builder.Entity<ActorMapping>().HasKey(m => new { m.MovieId, m.ActorId });
            builder.Entity<CinematographerMapping>().HasKey(m => new { m.MovieId, m.CinematographerId });
            builder.Entity<ComposerMapping>().HasKey(m => new { m.MovieId, m.ComposerId });
            builder.Entity<DirectorMapping>().HasKey(m => new { m.MovieId, m.DirectorId });
            builder.Entity<DistributorMapping>().HasKey(m => new { m.MovieId, m.DistributorId });
            builder.Entity<GenreMapping>().HasKey(m => new { m.MovieId, m.GenreId });
            builder.Entity<ProducerMapping>().HasKey(m => new { m.MovieId, m.ProducerId });
            builder.Entity<StudioMapping>().HasKey(m => new { m.MovieId, m.StudioId });
            builder.Entity<WriterMapping>().HasKey(m => new { m.MovieId, m.WriterId });

            base.OnModelCreating(builder);
        }
    }
}