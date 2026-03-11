# Team Tasks Dashboard
Prueba tecnica — Gestion de proyectos, tareas y desarrolladores con dashboard de carga de trabajo y prediccion de riesgo de retraso.
---

## Como correr el proyecto

### Prerrequisitos
- .NET 8 SDK
- Node.js 20+ y Angular CLI 17 (`npm install -g @angular/cli@17`)
- SQL Server 2022 instalado y corriendo

### Base de datos
El script `DBSetup_TeamTasks.sql` crea la base de datos `TeamTasksSample`, las tablas, constraints, stored procedures y los datos base minimos (estados, prioridades, 5 desarrolladores, 3 proyectos y 20 tareas).

Para ejecutar el script, conectarse primero a la base de datos `master`.
El script crea `TeamTasksSample` automaticamente.

```bash
sqlcmd -S localhost -U sa -P "TeamTasks2024!" -i database/DBSetup_TeamTasks.sql
```

o ejecutarlo directamente desde SSMS o VS Code conectado a `master`.

### API
```bash
cd src/TeamTasks.API
```

Actualizar la cadena de conexion en `appsettings.Development.json`:

```json
"DefaultConnection": "Server=localhost;Database=TeamTasksSample;User Id=TeamTasksUser;Password=TeamTasks2024!;TrustServerCertificate=True"
```

```bash
dotnet run
```

La API queda disponible en `http://localhost:5000`. Swagger en `http://localhost:5000/swagger`.

### Cargar datos de prueba
El script SQL ya incluye los datos base. Como alternativa, si se prefiere cargar los datos desde la API:

```
POST http://localhost:5000/api/seed
```

El endpoint usa MERGE, por lo que es seguro ejecutarlo varias veces sin generar duplicados. Si los datos ya existen retorna `409 Conflict`.

### SPA
```bash
cd src/team-tasks-spa
npm install
ng serve
```

La SPA queda disponible en `http://localhost:4200`.

---

### Alternativa — Docker
Si tienes Docker Desktop instalado:

```bash
docker-compose up -d
```

La contraseña del usuario `TeamTasksUser` en el contenedor es `TeamTasks2024!`. Actualizar el `appsettings.Development.json` con esa contraseña y luego seguir los pasos de API y SPA indicados arriba.
---

## Decisiones que tome
- Use SQL Server por ser el motor de base de datos con el que tengo mayor experiencia y donde me siento mas comodo para una prueba tecnica.
- Las tablas se crean desde el script SQL, que es el origen de la estructura. EF Core mapea las tablas existentes.
- Todas las consultas estan implementadas como SP en la base de datos. Los repositorios invocan el SP por nombre.
- Para el acceso a datos use EF Core en operaciones CRUD simples y Dapper para ejecutar los SP del dashboard.
- El backend sigue Clean Architecture dividido en cuatro capas: API, Application, Domain e Infrastructure, mas un proyecto separado para los tests con xUnit.
- El frontend esta en Angular 17 con componentes standalone.
- Implemente un componente `datatable` reutilizable que uso en todas las vistas tabulares, y un pipe `statusbadge` para los estados y prioridades.
- Para el grafico opcional usamos Chart.js.

Los datos base se incluyen directamente en el script SQL. El endpoint `POST /api/seed` es una alternativa que usa MERGE para evitar duplicados.
---

## Paquetes utilizados
Backend (.NET 8)

  Entity Framework Core 8.0.x
  Microsoft.EntityFrameworkCore.SqlServer 8.0.x
  Dapper 2.1.x
  xUnit 2.9.x
  Moq 4.20.x
  Swashbuckle (Swagger) 6.x

Frontend (Angular 17)

  Angular 17.x
  TypeScript 5.x
  RxJS 7.x
  Chart.js 4.x
  ng2-charts 6.x
---

## Logica del calculo de riesgo de retraso
Para cada desarrollador activo con tareas abiertas calculo el promedio de dias que se demoro por encima de la fecha limite en sus tareas ya completadas (`MAX(0, CompletionDate - DueDate)`).
Si no tiene historial, asumo cero dias de retraso.

Con ese promedio proyecto la fecha estimada de cierre de su tarea mas lejana (`LatestDueDate + AvgDelayDays`) y marco como alto riesgo (`HighRiskFlag = 1`) si esa fecha supera el vencimiento o si el promedio de retraso historico es de 3 dias o mas.