$env:DOTNET_WATCH_SUPPRESS_EMOJIS = 1
dotnet watch --project .\Odyssey\Odyssey.csproj --no-hot-reload
Remove-Item Env:DOTNET_WATCH_SUPPRESS_EMOJIS
