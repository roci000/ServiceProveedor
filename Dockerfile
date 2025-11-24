# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el contenido del proyecto
COPY . ./

# Restaurar dependencias
RUN dotnet restore ServiceProveedor.csproj

# Compilar
RUN dotnet build ServiceProveedor.csproj -c Release -o /app/build

# Etapa 2: Publish
FROM build AS publish
RUN dotnet publish ServiceProveedor.csproj -c Release -o /app/publish

# Etapa 3: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceProveedor.dll"]

#Es para crear una imgane en el contenedor segun todas las 
#tareas que tengas configuradas, saca una imagen de todo y 
#lo lleva al contenedor