$env:DOTNET_WATCH_SUPPRESS_EMOJIS = 1
dotnet watch --project .\Odyssey.Silo\Odyssey.Silo.csproj --no-hot-reload
Remove-Item Env:DOTNET_WATCH_SUPPRESS_EMOJIS
