using Dapper;
using TeamTasks.Infrastructure.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.Interfaces.Repositories;

namespace TeamTasks.Infrastructure.Repositories
{
    public class SeedRepository : ISeedRepository
    {
        private readonly string _connectionString;

        public SeedRepository(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<bool> HasDataAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var count = await connection.ExecuteScalarAsync<int>("select count(1) from Developers");
            return count > 0;
        }

        public async Task SeedReferenceDataAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"
        merge ProjectStatuses as target
        using (
            values
                ('Planned',     'Planificado'),
                ('In Progress', 'En progreso'),
                ('Completed',   'Completado')
        ) as source (Description, DisplayName)
        on target.Description = source.Description
        when not matched then
            insert (Description, DisplayName)
            values (source.Description, source.DisplayName)
        when matched then
            update set DisplayName = source.DisplayName;

        merge TaskStatuses as target
        using (
            values
                ('To Do',       'Por hacer',   'secondary'),
                ('In Progress', 'En progreso', 'primary'),
                ('Blocked',     'Bloqueado',   'danger'),
                ('Completed',   'Completado',  'success')
        ) as source (Description, DisplayName, ColorClass)
        on target.Description = source.Description
        when not matched then
            insert (Description, DisplayName, ColorClass)
            values (source.Description, source.DisplayName, source.ColorClass)
        when matched then
            update set DisplayName = source.DisplayName, ColorClass = source.ColorClass;

        merge TaskPriorities as target
        using (
            values
                ('Low',    'Baja',  'success'),
                ('Medium', 'Media', 'warning'),
                ('High',   'Alta',  'danger')
        ) as source (Description, DisplayName, ColorClass)
        on target.Description = source.Description
        when not matched then
            insert (Description, DisplayName, ColorClass)
            values (source.Description, source.DisplayName, source.ColorClass)
        when matched then
            update set DisplayName = source.DisplayName, ColorClass = source.ColorClass;
    ");
        }

        public async Task SeedDevelopersAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"
                merge Developers as target
                using (
                    values
                        ('Carlos', 'Ramírez', 'carlos.ramirez@teamtasks.com', 1),
                        ('Laura', 'Gómez', 'laura.gomez@teamtasks.com', 1),
                        ('Andrés', 'Torres', 'andres.torres@teamtasks.com', 1),
                        ('Valeria', 'Mendoza', 'valeria.mendoza@teamtasks.com', 1),
                        ('Diego', 'Herrera', 'diego.herrera@teamtasks.com', 1)
                ) as source (FirstName, LastName, Email, IsActive)
                on target.Email = source.Email
                when not matched then
                insert (FirstName, LastName, Email, IsActive)
                values (source.FirstName, source.LastName, source.Email, source.IsActive);
            ");
        }

        public async Task SeedProjectsAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"
                declare @StatusPlanned int = (select Id from ProjectStatuses where Description = 'Planned')
                declare @StatusInProgress int = (select Id from ProjectStatuses where Description = 'In Progress')
                declare @StatusCompleted int = (select Id from ProjectStatuses where Description = 'Completed')

                merge Projects as target
                using (
                    values
                        ('Portal de Clientes', 'Banco Nacional', '2026-04-01', '2026-09-30', @StatusPlanned),
                        ('App de Inventario', 'Logística Express', '2026-01-15', '2026-06-30', @StatusInProgress),
                        ('Módulo de Reportes', 'Salud Total', '2025-06-01', '2025-12-31', @StatusCompleted)
                ) as source (Name, ClientName, StartDate, EndDate, StatusId)
                on target.Name = source.Name and target.ClientName = source.ClientName
                when not matched then
                insert (Name, ClientName, StartDate, EndDate, StatusId)
                values (source.Name, source.ClientName, source.StartDate, source.EndDate, source.StatusId);
            ");
        }

        public async Task SeedTasksAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"
                declare @TaskTodo int = (select Id from TaskStatuses where Description = 'To Do')
                declare @TaskInProgress int = (select Id from TaskStatuses where Description = 'In Progress')
                declare @TaskBlocked int = (select Id from TaskStatuses where Description = 'Blocked')
                declare @TaskCompleted int = (select Id from TaskStatuses where Description = 'Completed')
                declare @PriorityLow int = (select Id from TaskPriorities where Description = 'Low')
                declare @PriorityMedium int = (select Id from TaskPriorities where Description = 'Medium')
                declare @PriorityHigh int = (select Id from TaskPriorities where Description = 'High')

                merge Tasks as target
                using (
                    values
                        (1, 'Definir arquitectura del portal', 'Documentar decisiones técnicas', 1, @TaskTodo, @PriorityHigh, 3, '2026-04-15', null),
                        (1, 'Configurar repositorio', 'Crear estructura base del proyecto', 2, @TaskTodo, @PriorityMedium, 1, '2026-04-10', null),
                        (1, 'Diseño de base de datos', 'Modelar entidades del portal', 3, @TaskTodo, @PriorityHigh, 4, '2026-04-20', null),
                        (1, 'Configurar CI/CD', 'Pipeline de integración continua', 4, @TaskTodo, @PriorityMedium, 2, '2026-04-25', null),
                        (1, 'Definir contrato de APIs', 'Especificación OpenAPI', 5, @TaskTodo, @PriorityMedium, 2, '2026-04-18', null),
                        (1, 'Configurar entornos', 'Dev, QA y Prod', 1, @TaskBlocked, @PriorityLow, 1, '2026-04-12', null),
                        (2, 'Módulo de entrada de inventario', 'CRUD de productos entrantes', 2, @TaskInProgress, @PriorityHigh, 4, '2026-03-20', null),
                        (2, 'Módulo de salida de inventario', 'CRUD de productos salientes', 3, @TaskInProgress, @PriorityHigh, 4, '2026-03-25', null),
                        (2, 'Alertas de stock mínimo', 'Notificaciones por umbral', 4, @TaskTodo, @PriorityMedium, 3, '2026-03-28', null),
                        (2, 'Reporte de movimientos', 'Exportar a Excel y PDF', 5, @TaskInProgress, @PriorityMedium, 3, '2026-04-05', null),
                        (2, 'Integración con proveedor', 'API REST de proveedor externo', 1, @TaskBlocked, @PriorityHigh, 5, '2026-03-18', null),
                        (2, 'Autenticación de usuarios', 'JWT y roles', 2, @TaskCompleted, @PriorityHigh, 3, '2026-02-28', '2026-03-05'),
                        (2, 'Configuración de base de datos', 'Migraciones y seed inicial', 3, @TaskCompleted, @PriorityMedium, 2, '2026-02-20', '2026-02-22'),
                        (2, 'Diseño de pantallas principales', 'Wireframes aprobados', 4, @TaskCompleted, @PriorityLow, 1, '2026-02-15', '2026-02-25'),
                        (3, 'Reporte de ventas mensual', 'Agrupado por categoría', 5, @TaskCompleted, @PriorityHigh, 3, '2025-08-30', '2025-08-28'),
                        (3, 'Reporte de pacientes activos', 'Filtros por fecha y médico', 1, @TaskCompleted, @PriorityMedium, 2, '2025-09-15', '2025-09-20'),
                        (3, 'Dashboard ejecutivo', 'KPIs del negocio', 2, @TaskCompleted, @PriorityHigh, 4, '2025-10-01', '2025-10-08'),
                        (3, 'Exportación a PDF', 'Todos los reportes', 3, @TaskCompleted, @PriorityMedium, 3, '2025-11-01', '2025-10-30'),
                        (3, 'Envío automático por correo', 'Scheduler semanal', 4, @TaskCompleted, @PriorityMedium, 3, '2025-11-30', '2025-12-05'),
                        (3, 'Pruebas de aceptación', 'UAT con cliente', 5, @TaskCompleted, @PriorityLow, 2, '2025-12-20', '2025-12-18')
                ) as source
                (
                    ProjectId, Title, Description, AssigneeId, StatusId, PriorityId,
                    EstimatedComplexity, DueDate, CompletionDate
                )
                on target.ProjectId = source.ProjectId and target.Title = source.Title
                when not matched then
                insert
                (
                    ProjectId, Title, Description, AssigneeId, StatusId, PriorityId,
                    EstimatedComplexity, DueDate, CompletionDate
                )
                values
                (
                    source.ProjectId, source.Title, source.Description, source.AssigneeId,
                    source.StatusId, source.PriorityId, source.EstimatedComplexity,
                    source.DueDate, source.CompletionDate
                );
            ");
        }
    }
}