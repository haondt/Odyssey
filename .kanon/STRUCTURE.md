# Odyssey

A self-hosted, real-time gameshow platform built on .NET Orleans, Blazor SSR, and HTMX.

- **Distributed actor runtime** — .NET Orleans grains manage sessions, parties, hosts, and devices across a silo cluster.
- **Server-rendered UI** — Blazor SSR + HTMX + Hyperscript for interactive pages without a heavy SPA client.
- **Event-driven communication** — SignalR bridges real-time events between the web client and Orleans grains.
- **Pluggable game system** — `IGame` / `IGameRegistry` abstractions let new games be dropped in via `Odyssey.Games.*`.
- **PostgreSQL persistence** — EF Core with Npgsql for app data; Orleans ADO.NET for clustering and grain storage.

## Directory layout

```
.
├── README.md                 # Human-readable quickstart
├── opencode.json             # opencode agent config
├── .kanon/                   # kanon session / plan files
├── .opencode/rules/kanon/    # kanon rule definitions
├── docs/                     # Extra docs (dependency graph, templates)
│
└── Odyssey/                  # Main codebase (solution root)
    ├── Odyssey.sln
    ├── justfile              # Task runner (install, build, watch, db-*)
    ├── Dockerfile            # Multi-stage build for the web app
    ├── dcdn.json             # Vendor npm deps into wwwroot/vendored
    ├── package.json          # Frontend deps (HTMX, SignalR, fonts, etc.)
    ├── postcss.config.mjs    # PostCSS pipeline (custom-media, mixins)
    ├── Directory.Build.props # Global MSBuild props (BL0005 suppressed)
    ├── nuget.config          # Custom GitLab NuGet source
    │
    ├── Odyssey/              # ASP.NET Core web entrypoint
    ├── Odyssey.Silo/         # Orleans silo host (console app)
    │
    ├── Odyssey.Domain/       # Business logic, events, repositories, registries
    ├── Odyssey.Core/         # Shared primitives (references Haondt.Core)
    ├── Odyssey.Client/       # Client-side services (auth, device, display, host)
    ├── Odyssey.UI/           # Razor/Blazor UI components (postcss pre-build)
    ├── Odyssey.Persistence/  # EF Core DbContext + migrations (Postgres)
    │
    ├── Odyssey.GrainInterfaces/   # Orleans grain contracts + shared models
    ├── Odyssey.Grains/            # Grain implementations
    │
    ├── Odyssey.Games.Domain/      # Game-specific domain logic
    ├── Odyssey.Games.Client/      # Game-specific Razor components
    ├── Odyssey.Games.Server/      # Game-specific grain wiring
    │
    ├── Haondt.Web.UI/        # Reusable Razor component library
    ├── Haondt.Web.UI.Demo/   # Demo site for the component library
    │
    ├── Haondt.Orleans.Core/  # Shared Orleans primitives
    ├── Haondt.Orleans/       # Shared Orleans runtime utilities
    ├── Haondt.Orleans.Testing/    # Test helpers for Orleans
    │
    ├── Odyssey.Tests.Core/          # Shared test fixtures/models
    ├── Odyssey.Grains.Tests/        # Grain unit tests (Orleans.TestingHost)
    ├── Odyssey.Persistence.Tests/   # EF / Postgres tests (Testcontainers)
    └── Odyssey.IntegrationTests/    # Full-stack integration tests
```

## Key packages and how they relate

| Project | Role | Key dependencies |
|---|---|---|
| `Odyssey` | Web host | `Odyssey.UI`, `Haondt.Web`, ASP.NET Core OpenAPI |
| `Odyssey.Silo` | Silo host | `Odyssey.Games.Server`, Orleans Server, Npgsql |
| `Odyssey.Domain` | Business rules | `Odyssey.GrainInterfaces`, `Odyssey.Persistence`, Orleans Clustering |
| `Odyssey.Client` | Client services | `Odyssey.Domain`, Orleans Client, `Haondt.Web.Core` |
| `Odyssey.UI` | Razor components | `Odyssey.Client`, `Odyssey.Games.Client`, `Haondt.Web.UI` |
| `Odyssey.Persistence` | Data layer | `Odyssey.Core`, EF Core, Npgsql, `Haondt.Persistence.EFCore` |
| `Odyssey.GrainInterfaces` | Grain contracts | `Odyssey.Core`, `Haondt.Orleans.Core`, Orleans SDK |
| `Odyssey.Grains` | Grain impls | `Odyssey.Domain`, `Odyssey.GrainInterfaces`, `Haondt.Orleans` |
| `Odyssey.Games.*` | Game plugins | Domain → Client → Server chain, stays separate from core |
| `Haondt.Web.UI` | Shared UI lib | `Haondt.Web`, ASP.NET Components Web, PostCSS build step |
| `Haondt.Orleans.*` | Shared Orleans utils | SDK / Runtime primitives used by grains and tests |

## Tech stack

- **Runtime**: .NET 10 (ASP.NET Core, Blazor SSR)
- **Actor framework**: .NET Orleans 10.1.0
- **Frontend**: HTMX 2, Hyperscript, SignalR, Idiomorph, Lucide icons
- **Styling**: PostCSS (custom-media, mixins) + `*.pcss` → `*.css` pre-build
- **Persistence**: EF Core 10 + Npgsql 10 (PostgreSQL)
- **Testing**: xUnit, FluentAssertions, Coverlet, Testcontainers.PostgreSql, Orleans.TestingHost
- **Build / run**: `just` (justfile), `bun` (package manager), `dcdn` (vendor deps)
- **Containers**: Dockerfile (production web image), podman (dev DB)

## Architectural patterns

- **Orleans virtual actors** — Session, Party, Host, Device, Display, JoinCode, etc. are modeled as grains with string/GUID keys.
- **Event transformers** — `IEventTransformer<T>` implementations route domain events (`PartyEvent`, `SignalROutboundEvent`, etc.) to the correct transport layer (SignalR, grain observers).
- **Registry pattern** — `IGameRegistry`, `IEventTransformerRegistry`, `ISignalRConnectionRegistry` decouple discovery from usage.
- **Razor class libraries** — `Haondt.Web.UI` and `Odyssey.UI` / `Odyssey.Games.Client` publish static assets under `shared` and `haondt/Haondt.Web.UI` base paths.
- **PostCSS build integration** — `Haondt.Web.UI` and `Odyssey.UI` run `bun run postcss` as a PreBuild target to compile `.pcss` files.

## Domain concepts

- **Session** — A grain-backed multiplayer room identified by a join code.
- **Party** — A group of members (hosts, devices, displays) inside a session.
- **Host** — The privileged member who controls game flow.
- **Device / Display** — Two client roles: a player device and a shared display screen.
- **Game** — Pluggable implementation of `IGame` with parameters, state, and event handling.
- **Board** — Persistent metadata about a game instance.
- **Grain lease** — A distributed lease mechanism (`IGrainLeaseGrain`) for coordinating exclusive grain access.

## Non-obvious conventions

- **Separate Silo process** — The web app (`Odyssey`) and the Orleans silo (`Odyssey.Silo`) are built and run independently in development (`just watch client` vs `just watch silo`).
- **dcdn vendors npm deps** — Frontend libraries are copied into `Odyssey.UI/wwwroot/vendored` by `dcdn` (not served from node_modules).
- **PostCSS global data** — `postcss.config.mjs` references `Haondt.Web.UI/wwwroot/css/variables.css` for mixins and custom media queries; any `.pcss` file in the watched tree can use them.
- **EF migrations are Postgres-only** — Migrations live in `Odyssey.Persistence/Migrations/Postgres` and the justfile hardcodes `PostgresApplicationDbContext`.
- **Orleans SQL scripts are vendored** — `Odyssey.Persistence` copies raw `PostgreSQL-*.sql` scripts to output for Orleans clustering / persistence setup.
- **No CI config** — No `.github/workflows` or `.gitlab-ci.yml` found; builds appear to be local or manual at this time.
