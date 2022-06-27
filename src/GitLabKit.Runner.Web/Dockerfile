FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.sln ./
COPY ["GitLabKit.Runner.Web/GitLabKit.Runner.Web.csproj", "GitLabKit.Runner.Web/"]
COPY ["GitLabKit.Runner.Core/GitLabKit.Runner.Core.csproj", "GitLabKit.Runner.Core/"]
RUN dotnet restore "GitLabKit.Runner.Web/GitLabKit.Runner.Web.csproj"
COPY . .
WORKDIR /src/GitLabKit.Runner.Web
RUN dotnet build "GitLabKit.Runner.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GitLabKit.Runner.Web.csproj" -c Release -o /app

FROM node:16-alpine AS build-client
WORKDIR /client
COPY clientside/package.json clientside/yarn.lock ./
RUN npm i
COPY clientside ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .
COPY --from=build-client /client/build ./wwwroot
ENTRYPOINT ["dotnet", "GitLabKit.Runner.Web.dll"]
