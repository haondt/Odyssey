namespace Odyssey.Persistence.Models
{
    public class PersistenceSettings
    {
        public DatabaseSettings DatabaseSettings { get; set; } = new();
        public BlobStorageSettings BlobStorageSettings { get; set; } = new();
    }

    public class BlobStorageSettings
    {
        public BlobStorageDriver Driver { get; set; } = BlobStorageDriver.Filesystem;
        public FilesystemBlobStorageSettings? Filesystem { get; set; }
    }

    public class FilesystemBlobStorageSettings
    {
        public required string FilePath { get; set; }
    }

    public class DatabaseSettings
    {
        public DatabaseDriver Driver { get; set; } = DatabaseDriver.Sqlite;
        public bool DropDatabaseOnStartup { get; set; } = false;
        public SqliteSettings? Sqlite { get; set; }
        public PostgresSettings? Postgres { get; set; }
    }


    public class SqliteSettings
    {
        public required string FilePath { get; set; }
    }

    public class PostgresSettings
    {
        public required int Port { get; set; }
        public required string Host { get; set; }
        public required string Database { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
