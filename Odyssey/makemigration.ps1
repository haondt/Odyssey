param(
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

$env:PersistenceSettings__DatabaseSettings__Driver = "Postgres"
dotnet ef migrations add $MigrationName `
    --project .\Odyssey.Persistence\Odyssey.Persistence.csproj `
    --context PostgresApplicationDbContext `
    --startup-project .\Odyssey.Silo\Odyssey.Silo.csproj `
    --output-dir .\Migrations\Postgres
