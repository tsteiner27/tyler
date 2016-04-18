using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using TylerSteiner.Cli.EntityFramework;

namespace TylerSteiner.Cli.Migrations
{
    [DbContext(typeof(MoviesDbContext))]
    partial class MoviesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TylerSteiner.Models.Actor", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Actors");
                });

            modelBuilder.Entity("TylerSteiner.Models.ActorMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("ActorId");

                    b.HasKey("MovieId", "ActorId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Cinematographer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Cinematographers");
                });

            modelBuilder.Entity("TylerSteiner.Models.CinematographerMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("CinematographerId");

                    b.HasKey("MovieId", "CinematographerId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Composer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Composers");
                });

            modelBuilder.Entity("TylerSteiner.Models.ComposerMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("ComposerId");

                    b.HasKey("MovieId", "ComposerId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Director", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Directors");
                });

            modelBuilder.Entity("TylerSteiner.Models.DirectorMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("DirectorId");

                    b.HasKey("MovieId", "DirectorId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Distributor", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("TylerSteiner.Models.DistributorMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("DistributorId");

                    b.HasKey("MovieId", "DistributorId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Genre", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Genres");
                });

            modelBuilder.Entity("TylerSteiner.Models.GenreMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("GenreId");

                    b.HasKey("MovieId", "GenreId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Movie", b =>
                {
                    b.Property<string>("Id");

                    b.Property<long?>("Budget");

                    b.Property<double?>("ImdbRating");

                    b.Property<int>("Length");

                    b.Property<string>("MpaaRating");

                    b.Property<string>("Poster");

                    b.Property<double>("Rating");

                    b.Property<bool>("SawPremier");

                    b.Property<int>("TimesWatched");

                    b.Property<int>("TimesWatchedInTheater");

                    b.Property<string>("Title");

                    b.Property<long?>("UsBoxOffice");

                    b.Property<long?>("WorldBoxOffice");

                    b.Property<int>("Year");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("TylerSteiner.Models.Producer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Producers");
                });

            modelBuilder.Entity("TylerSteiner.Models.ProducerMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("ProducerId");

                    b.HasKey("MovieId", "ProducerId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Studio", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Studios");
                });

            modelBuilder.Entity("TylerSteiner.Models.StudioMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("StudioId");

                    b.HasKey("MovieId", "StudioId");
                });

            modelBuilder.Entity("TylerSteiner.Models.Writer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Writers");
                });

            modelBuilder.Entity("TylerSteiner.Models.WriterMapping", b =>
                {
                    b.Property<string>("MovieId");

                    b.Property<string>("WriterId");

                    b.HasKey("MovieId", "WriterId");
                });

            modelBuilder.Entity("TylerSteiner.Models.ActorMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Actor")
                        .WithMany()
                        .HasForeignKey("ActorId");

                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("TylerSteiner.Models.CinematographerMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Cinematographer")
                        .WithMany()
                        .HasForeignKey("CinematographerId");

                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("TylerSteiner.Models.ComposerMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Composer")
                        .WithMany()
                        .HasForeignKey("ComposerId");

                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("TylerSteiner.Models.DirectorMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Director")
                        .WithMany()
                        .HasForeignKey("DirectorId");

                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("TylerSteiner.Models.DistributorMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Distributor")
                        .WithMany()
                        .HasForeignKey("DistributorId");

                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("TylerSteiner.Models.GenreMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Genre")
                        .WithMany()
                        .HasForeignKey("GenreId");

                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("TylerSteiner.Models.ProducerMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");

                    b.HasOne("TylerSteiner.Models.Producer")
                        .WithMany()
                        .HasForeignKey("ProducerId");
                });

            modelBuilder.Entity("TylerSteiner.Models.StudioMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");

                    b.HasOne("TylerSteiner.Models.Studio")
                        .WithMany()
                        .HasForeignKey("StudioId");
                });

            modelBuilder.Entity("TylerSteiner.Models.WriterMapping", b =>
                {
                    b.HasOne("TylerSteiner.Models.Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");

                    b.HasOne("TylerSteiner.Models.Writer")
                        .WithMany()
                        .HasForeignKey("WriterId");
                });
        }
    }
}
