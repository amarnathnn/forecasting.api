#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-nanoserver-sac2016 AS build
WORKDIR /src
COPY ["PS.CTS.Allocation/PS.CTS.Allocation.csproj", "PS.CTS.Allocation/"]
RUN dotnet restore "PS.CTS.Allocation/PS.CTS.Allocation.csproj"
COPY . .
WORKDIR "/src/PS.CTS.Allocation"
RUN dotnet build "PS.CTS.Allocation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PS.CTS.Allocation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PS.CTS.Allocation.dll"]