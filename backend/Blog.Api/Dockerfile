# ── Build stage ───────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Kopiraj samo .csproj fajlove prvo radi keširanja `dotnet restore` sloja.
COPY Blog.Api/Blog.Api.csproj Blog.Api/
COPY Blog.Application/Blog.Application.csproj Blog.Application/
COPY Blog.Domain/Blog.Domain.csproj Blog.Domain/
COPY Blog.Infrastructure/Blog.Infrastructure.csproj Blog.Infrastructure/
RUN dotnet restore Blog.Api/Blog.Api.csproj

# Kopiraj ostatak izvornog koda i objavi release build.
COPY Blog.Api/ Blog.Api/
COPY Blog.Application/ Blog.Application/
COPY Blog.Domain/ Blog.Domain/
COPY Blog.Infrastructure/ Blog.Infrastructure/
RUN dotnet publish Blog.Api/Blog.Api.csproj -c Release -o /app/publish --no-restore

# ── Runtime stage ─────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Blog.Api.dll"]
