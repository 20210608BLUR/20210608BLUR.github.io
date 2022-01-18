#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HolyShong.BackStage/HolyShong.BackStage.csproj", "HolyShong.BackStage/"]
RUN dotnet restore "HolyShong.BackStage/HolyShong.BackStage.csproj"
COPY . .
WORKDIR "/src/HolyShong.BackStage"
RUN dotnet build "HolyShong.BackStage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HolyShong.BackStage.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HolyShong.BackStage.dll"]