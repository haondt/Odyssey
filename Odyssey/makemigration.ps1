param(
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

$env:PersistenceSettings__Driver = "Sqlite"
dotnet ef migrations add $MigrationName `
    --project .\Odyssey.Persistence\Odyssey.Persistence.csproj `
    --context SqliteApplicationDbContext `
    --startup-project .\Odyssey\Odyssey.csproj `
    --output-dir .\Migrations\Sqlite

$env:PersistenceSettings__Driver = "Postgres"
dotnet ef migrations add $MigrationName `
    --project .\Odyssey.Persistence\Odyssey.Persistence.csproj `
    --context PostgresApplicationDbContext `
    --startup-project .\Odyssey\Odyssey.csproj `
    --output-dir .\Migrations\Postgres