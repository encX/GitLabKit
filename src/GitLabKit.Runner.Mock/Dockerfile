FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GitLabKit.Runner.Mock/GitLabKit.Runner.Mock.csproj", "GitLabKit.Runner.Mock/"]
RUN dotnet restore "GitLabKit.Runner.Mock/GitLabKit.Runner.Mock.csproj"
COPY . .
WORKDIR "/src/GitLabKit.Runner.Mock"
RUN dotnet build "GitLabKit.Runner.Mock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GitLabKit.Runner.Mock.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GitLabKit.Runner.Mock.dll"]
