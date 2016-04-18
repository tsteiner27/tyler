using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace TylerSteiner.Cli.Migrations
{
    public partial class InitialDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actor", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Cinematographers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinematographer", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Composers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Composer", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Distributor",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributor", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Budget = table.Column<long>(nullable: true),
                    ImdbRating = table.Column<double>(nullable: true),
                    Length = table.Column<int>(nullable: false),
                    MpaaRating = table.Column<string>(nullable: true),
                    Poster = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: false),
                    SawPremier = table.Column<bool>(nullable: false),
                    TimesWatched = table.Column<int>(nullable: false),
                    TimesWatchedInTheater = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UsBoxOffice = table.Column<long>(nullable: true),
                    WorldBoxOffice = table.Column<long>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producer", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Studios",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studio", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Writers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writer", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "ActorMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    ActorId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMapping", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_ActorMapping_Actor_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActorMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "CinematographerMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    CinematographerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinematographerMapping", x => new { x.MovieId, x.CinematographerId });
                    table.ForeignKey(
                        name: "FK_CinematographerMapping_Cinematographer_CinematographerId",
                        column: x => x.CinematographerId,
                        principalTable: "Cinematographers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CinematographerMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "ComposerMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    ComposerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposerMapping", x => new { x.MovieId, x.ComposerId });
                    table.ForeignKey(
                        name: "FK_ComposerMapping_Composer_ComposerId",
                        column: x => x.ComposerId,
                        principalTable: "Composers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComposerMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "DirectorMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    DirectorId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorMapping", x => new { x.MovieId, x.DirectorId });
                    table.ForeignKey(
                        name: "FK_DirectorMapping_Director_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "Directors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectorMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "DistributorMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    DistributorId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorMapping", x => new { x.MovieId, x.DistributorId });
                    table.ForeignKey(
                        name: "FK_DistributorMapping_Distributor_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DistributorMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "GenreMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    GenreId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMapping", x => new { x.MovieId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GenreMapping_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GenreMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "ProducerMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    ProducerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerMapping", x => new { x.MovieId, x.ProducerId });
                    table.ForeignKey(
                        name: "FK_ProducerMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProducerMapping_Producer_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "StudioMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    StudioId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudioMapping", x => new { x.MovieId, x.StudioId });
                    table.ForeignKey(
                        name: "FK_StudioMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudioMapping_Studio_StudioId",
                        column: x => x.StudioId,
                        principalTable: "Studios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "WriterMapping",
                columns: table => new
                {
                    MovieId = table.Column<string>(nullable: false),
                    WriterId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriterMapping", x => new { x.MovieId, x.WriterId });
                    table.ForeignKey(
                        name: "FK_WriterMapping_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WriterMapping_Writer_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Writers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ActorMapping");
            migrationBuilder.DropTable("CinematographerMapping");
            migrationBuilder.DropTable("ComposerMapping");
            migrationBuilder.DropTable("DirectorMapping");
            migrationBuilder.DropTable("DistributorMapping");
            migrationBuilder.DropTable("GenreMapping");
            migrationBuilder.DropTable("ProducerMapping");
            migrationBuilder.DropTable("StudioMapping");
            migrationBuilder.DropTable("WriterMapping");
            migrationBuilder.DropTable("Actors");
            migrationBuilder.DropTable("Cinematographers");
            migrationBuilder.DropTable("Composers");
            migrationBuilder.DropTable("Directors");
            migrationBuilder.DropTable("Distributor");
            migrationBuilder.DropTable("Genres");
            migrationBuilder.DropTable("Producers");
            migrationBuilder.DropTable("Studios");
            migrationBuilder.DropTable("Movie");
            migrationBuilder.DropTable("Writers");
        }
    }
}
