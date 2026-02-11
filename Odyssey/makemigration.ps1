param(
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

$env:PersistenceSettings__DatabaseSettings__Driver = "Postgres"
dotnet ef migrations add $MigrationName `
    --project .\Odyssey.Persistence\Odyssey.Persistence.csproj `
    --context PostgresApplicationDbContext `
    --startup-project .\Odyssey\Odyssey.csproj `
    --output-dir .\Migrations\Postgres

$env:PersistenceSettings__DatabaseSettings__Driver = "Sqlite"
dotnet ef migrations add $MigrationName `
    --project .\Odyssey.Persistence\Odyssey.Persistence.csproj `
    --context SqliteApplicationDbContext `
    --startup-project .\Odyssey\Odyssey.csproj `
    --output-dir .\Migrations\Sqlite
