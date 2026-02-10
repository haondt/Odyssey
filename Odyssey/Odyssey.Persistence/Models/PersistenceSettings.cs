namespace Odyssey.Persistence.Models
{
    public class PersistenceSettings
    {
        public StorageDrivers Driver { get; set; } = StorageDrivers.Memory;
        public bool DropDatabaseOnStartup { get; set; } = false;

        public SqliteSettings? Sqlite { get; set; }
        public PostgresSettings? Postgres { get; set; }

        public string FileDataPath { get; set; } = Directory.GetCurrentDirectory();
    }


    public class SqliteSettings
    {
        public string FilePath { get; set; } = "odyssey.db";
    }

    public class PostgresSettings
    {
        public int Port { get; set; } = 5432;
        public string Host { get; set; } = "localhost";
        public string Database { get; set; } = "odyssey";
        public string? Username { get; set; }
        public string? Password { get; set; }
    }


}
