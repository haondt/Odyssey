using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odyssey.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class OrleansBaseMigration : Migration
    {
        private const string _pathToOrleansMigrations = "Migrations/Orleans";
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var migrationFiles = new List<string> {
                Path.Combine(AppContext.BaseDirectory, _pathToOrleansMigrations, "PostgreSQL-Main.sql"),
                Path.Combine(AppContext.BaseDirectory, _pathToOrleansMigrations, "PostgreSQL-Clustering.sql"),
                Path.Combine(AppContext.BaseDirectory, _pathToOrleansMigrations, "PostgreSQL-Clustering-3.6.0.sql"),
                Path.Combine(AppContext.BaseDirectory, _pathToOrleansMigrations, "PostgreSQL-Clustering-3.7.0.sql"),
                Path.Combine(AppContext.BaseDirectory, _pathToOrleansMigrations, "PostgreSQL-Persistence.sql"),
                Path.Combine(AppContext.BaseDirectory, _pathToOrleansMigrations, "PostgreSQL-Persistence-3.6.0.sql"),
            };
            foreach (var migrationFile in migrationFiles)
            {
                var sql = File.ReadAllText(migrationFile);
                migrationBuilder.Sql(sql);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotSupportedException("I am too lazy to implement this.");
        }
    }
}
