# Odyssey

A self-hosted, real-time gameshow platform

### Tech stack

- ASP.NET Runtime
- Blazor SSR
- SignalR (Websockets)
- HTMX + Hyperscript
- .NET Orleans
- Entity Framework Core
- PostgreSQL

# Development Guide

### Preqrequisites

- .NET 10
- [dcdn](https://github.com/haondt/dcdn)
- [bun](https://bun.com/)
- [podman](https://podman.io/)
- [postgresql client (`psql`)](https://www.postgresql.org/)

## Running tests

- For persistence tests (`Odyssey.Persistence.Tests`), you must have the podman daemon running. You can start it temporarily with `just pm`.

## Running the app

1. Install (dcdn) deps

```sh
just install
```

2. Stand up the db

```sh
just db-init
```

3. Start the silo

```sh
just watch silo
```

4. Start the client

```sh
just watch client -r
```

5. Visit http://localhost:5044

### Database

- For development, the app assumes you have a postgres db available at `192.168.1.213`. See/change `appsettings.Development.json` accordingly, including changing to in-memory storage if preferred.
- The db can be managed with the justfile - `just db-init`, `just db-drop`, `just db-reset`.

### `Haondt.Web.UI` Demo

There is an included demo site for the alpha `Haondt.Web.UI` components. You can start it with

```
just watch demo -r
```

And view it at http://localhost:5062.

\[WIP\]
