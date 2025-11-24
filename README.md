1. Crear el proyecto de API
dotnet new webapi -n ServiceProveedor

2. Entrar a la carpeta del proyecto
cd ServiceProveedor

3. Agregar paquetes de Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.10
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.10

4. Verificamos la instalacion de los paquetes
   dotnet list package

5. Construir las imágenes
docker compose build

6. Levantar los contenedores
docker compose up

7. Probar el funncionamiento del Microservicio de Proveedor (ServiceProveedor) en POSTMAN
   - Listar: http://localhost:8001/api/ProveedorApi/Listar
   - Buscar: http://localhost:8001/api/ProveedorApi/buscar?nombre=SATURNO%20S.R.L 
   - Crear: http://localhost:8001/api/ProveedorApi/
   - Editar: http://localhost:8001/api/ProveedorApi?nombre=SATURNO%20S.R.L 
   - Eliminar: http://localhost:8001/api/ProveedorApi?nombre=SATURNO%20S.R.L 
