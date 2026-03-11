-- ========================================
-- Autor: Jonathan Aldana
-- Fecha: 11/03/2026
-- Desc: Base inicial de TeamTasksSample
-- ========================================

use master
go

if not exists (select name from sys.databases where name = 'TeamTasksSample')
    create database TeamTasksSample
go

use TeamTasksSample
go

-- crear usuario con los permisos necesarios de la aplicación
if not exists (select name from sys.server_principals where name = 'TeamTasksUser')
    create login TeamTasksUser with password = 'TeamTasks2024!'
go

use TeamTasksSample
go

if not exists (select name from sys.database_principals where name = 'TeamTasksUser')
begin
    create user TeamTasksUser for login TeamTasksUser
    alter role db_datareader add member TeamTasksUser
    alter role db_datawriter add member TeamTasksUser
    grant execute to TeamTasksUser
end
go

--Crear tablas
-- ProjectStatuses
if not exists (select * from sysobjects where name = 'ProjectStatuses' and xtype = 'U')
create table ProjectStatuses (
    Id int identity(1,1) primary key,
    Description nvarchar(50) not null unique
)

-- TaskStatuses
if not exists (select * from sysobjects where name = 'TaskStatuses' and xtype = 'U')
create table TaskStatuses (
    Id int identity(1,1) primary key,
    Description nvarchar(50) not null unique
)

-- TaskPriorities
if not exists (select * from sysobjects where name = 'TaskPriorities' and xtype = 'U')
create table TaskPriorities (
    Id int identity(1,1) primary key,
    Description nvarchar(50) not null unique
)

-- Developers
if not exists (select * from sysobjects where name = 'Developers' and xtype = 'U')
create table Developers (
    DeveloperId int identity(1,1) primary key,
    FirstName nvarchar(100) not null,
    LastName nvarchar(100) not null,
    Email nvarchar(255) not null unique,
    IsActive bit not null default 1,
    CreatedAt datetime not null default getdate()
)

-- Projects
if not exists (select * from sysobjects where name = 'Projects' and xtype = 'U')
create table Projects (
    ProjectId int identity(1,1) primary key,
    Name nvarchar(200) not null,
    ClientName nvarchar(200) not null,
    StartDate date not null,
    EndDate date,
    StatusId int not null references ProjectStatuses(Id)
)

-- Tasks
if not exists (select * from sysobjects where name = 'Tasks' and xtype = 'U')
create table Tasks (
    TaskId int identity(1,1) primary key,
    ProjectId int not null references Projects(ProjectId),
    Title nvarchar(300) not null,
    Description nvarchar(max),
    AssigneeId int references Developers(DeveloperId),
    StatusId int not null references TaskStatuses(Id),
    PriorityId int not null references TaskPriorities(Id),
    EstimatedComplexity int not null default 1 check (EstimatedComplexity between 1 and 5),
    DueDate date not null,
    CompletionDate date,
    CreatedAt datetime not null default getdate()
)
go

--Crear SP's
-- sp_insert_task
create or alter procedure sp_insert_task
    @ProjectId int,
    @Title nvarchar(300),
    @Description nvarchar(max),
    @AssigneeId int,
    @StatusId int,
    @PriorityId int,
    @EstimatedComplexity int,
    @DueDate date
as
begin
    set nocount on

    insert into Tasks (ProjectId, Title, Description, AssigneeId, StatusId, PriorityId, EstimatedComplexity, DueDate)
    output inserted.TaskId
    values (@ProjectId, @Title, @Description, @AssigneeId, @StatusId, @PriorityId, @EstimatedComplexity, @DueDate)
end
go

-- sp_get_all_projects
create or alter procedure sp_get_all_projects
as
begin
    set nocount on

    declare @CompletedStatusId int = (select Id from TaskStatuses where Description = 'Completed')

    select
        p.ProjectId,
        p.Name,
        p.ClientName,
        ps.Description as Status,
        count(t.TaskId) as TotalTasks,
        count(case when t.StatusId <> @CompletedStatusId then 1 end) as OpenTasks,
        count(case when t.StatusId = @CompletedStatusId then 1 end) as CompletedTasks
    from Projects p
    inner join ProjectStatuses ps on ps.Id = p.StatusId
    left join Tasks t on t.ProjectId = p.ProjectId
    group by p.ProjectId, p.Name, p.ClientName, ps.Description
end
go

-- sp_get_project_by_id
create or alter procedure sp_get_project_by_id
    @ProjectId int
as
begin
    set nocount on

    select
        p.ProjectId,
        p.Name,
        p.ClientName,
        ps.Description as Status,
        p.StartDate,
        p.EndDate
    from Projects p
    inner join ProjectStatuses ps on ps.Id = p.StatusId
    where p.ProjectId = @ProjectId
end
go

-- sp_get_tasks_by_project
create or alter procedure sp_get_tasks_by_project
    @ProjectId int,
    @StatusId int = null,
    @AssigneeId int = null,
    @Page int = 1,
    @PageSize int = 10
as
begin
    set nocount on

    select
        t.TaskId,
        t.ProjectId,
        t.Title,
        t.Description,
        t.AssigneeId,
        d.FirstName + ' ' + d.LastName as AssigneeName,
        ts.Description as Status,
        tp.Description as Priority,
        t.EstimatedComplexity,
        t.DueDate,
        t.CompletionDate,
        t.CreatedAt
    from Tasks t
    inner join TaskStatuses ts on ts.Id = t.StatusId
    inner join TaskPriorities tp on tp.Id = t.PriorityId
    left join Developers d on d.DeveloperId = t.AssigneeId
    where t.ProjectId = @ProjectId
        and (@StatusId is null or t.StatusId = @StatusId)
        and (@AssigneeId is null or t.AssigneeId = @AssigneeId)
    order by t.CreatedAt desc
    offset (@Page - 1) * @PageSize rows
    fetch next @PageSize rows only
end
go

-- sp_get_task_by_id
create or alter procedure sp_get_task_by_id
    @TaskId int
as
begin
    set nocount on

    select
        t.TaskId,
        t.ProjectId,
        t.Title,
        t.Description,
        t.AssigneeId,
        d.FirstName + ' ' + d.LastName as AssigneeName,
        ts.Description as Status,
        tp.Description as Priority,
        t.EstimatedComplexity,
        t.DueDate,
        t.CompletionDate,
        t.CreatedAt
    from Tasks t
    inner join TaskStatuses ts on ts.Id = t.StatusId
    inner join TaskPriorities tp on tp.Id = t.PriorityId
    left join Developers d on d.DeveloperId = t.AssigneeId
    where t.TaskId = @TaskId
end
go

-- sp_update_task_status
create or alter procedure sp_update_task_status
    @TaskId int,
    @StatusId int,
    @PriorityId int = null,
    @EstimatedComplexity int = null
as
begin
    set nocount on

    declare @CompletedStatusId int = (select Id from TaskStatuses where Description = 'Completed')

    update Tasks
    set
        StatusId = @StatusId,
        PriorityId = isnull(@PriorityId, PriorityId),
        EstimatedComplexity = isnull(@EstimatedComplexity, EstimatedComplexity),
        CompletionDate = case when @StatusId = @CompletedStatusId then cast(getdate() as date) else CompletionDate end
    where TaskId = @TaskId
end
go

-- sp_get_active_developers
create or alter procedure sp_get_active_developers
as
begin
    set nocount on

    select
        DeveloperId,
        FirstName + ' ' + LastName as FullName,
        Email
    from Developers
    where IsActive = 1
end
go

-- sp_get_developer_workload
create or alter procedure sp_get_developer_workload
as
begin
    set nocount on

    declare @CompletedStatusId int = (select Id from TaskStatuses where Description = 'Completed')

    select
        d.DeveloperId,
        d.FirstName + ' ' + d.LastName as DeveloperName,
        count(t.TaskId) as OpenTasksCount,
        isnull(avg(cast(t.EstimatedComplexity as float)), 0) as AverageEstimatedComplexity
    from Developers d
    left join Tasks t on t.AssigneeId = d.DeveloperId
        and t.StatusId <> @CompletedStatusId
    where d.IsActive = 1
    group by d.DeveloperId, d.FirstName, d.LastName
end
go

-- sp_get_project_health
create or alter procedure sp_get_project_health
as
begin
    set nocount on

    declare @CompletedStatusId int = (select Id from TaskStatuses where Description = 'Completed')

    select
        p.ProjectId,
        p.Name as ProjectName,
        p.ClientName,
        count(t.TaskId) as TotalTasks,
        count(case when t.StatusId <> @CompletedStatusId then 1 end) as OpenTasks,
        count(case when t.StatusId = @CompletedStatusId then 1 end) as CompletedTasks
    from Projects p
    left join Tasks t on t.ProjectId = p.ProjectId
    group by p.ProjectId, p.Name, p.ClientName
end
go

-- sp_get_delay_risk
create or alter procedure sp_get_delay_risk
as
begin
    set nocount on

    declare @CompletedStatusId int = (select Id from TaskStatuses where Description = 'Completed')
    declare @RiskThreshold int = 3

    ;with Completed as (
        select
            t.AssigneeId,
            avg(cast(isnull(datediff(day, t.DueDate, t.CompletionDate), 0) as float)) as AvgDelayDays
        from Tasks t
        where t.StatusId = @CompletedStatusId
            and t.CompletionDate is not null
        group by t.AssigneeId
    ),
    OpenTasks as (
        select
            t.AssigneeId,
            count(*) as OpenTasksCount,
            min(t.DueDate) as NearestDueDate,
            max(t.DueDate) as LatestDueDate
        from Tasks t
        where t.StatusId <> @CompletedStatusId
        group by t.AssigneeId
    )
    select
        d.FirstName + ' ' + d.LastName as DeveloperName,
        isnull(ot.OpenTasksCount, 0) as OpenTasksCount,
        isnull(round(c.AvgDelayDays, 2), 0) as AvgDelayDays,
        ot.NearestDueDate,
        ot.LatestDueDate,
        dateadd(day, isnull(c.AvgDelayDays, 0), ot.LatestDueDate) as PredictedCompletionDate,
        case
            when dateadd(day, isnull(c.AvgDelayDays, 0), ot.LatestDueDate) > ot.LatestDueDate
                or isnull(c.AvgDelayDays, 0) >= @RiskThreshold
            then 1 else 0
        end as HighRiskFlag
    from Developers d
    inner join OpenTasks ot on ot.AssigneeId = d.DeveloperId
    left join Completed c on c.AssigneeId = d.DeveloperId
    where d.IsActive = 1
    order by HighRiskFlag desc, d.FirstName
end
go

-- ===================
-- Seed datos mínimos
-- ===================
SET NOCOUNT ON;
merge projectstatuses as target
using
(
    values
        ('Planned'),
        ('In Progress'),
        ('Completed')
) as source (description)
on target.description = source.description
when not matched then
insert (description)
values (source.description);


merge taskstatuses as target
using
(
    values
        ('To Do'),
        ('In Progress'),
        ('Blocked'),
        ('Completed')
) as source (description)
on target.description = source.description
when not matched then
insert (description)
values (source.description);


merge taskpriorities as target
using
(
    values
        ('Low'),
        ('Medium'),
        ('High')
) as source (description)
on target.description = source.description
when not matched then
insert (description)
values (source.description);


declare @statusplanned int = (select id from projectstatuses where description = 'Planned');
declare @statusinprogress int = (select id from projectstatuses where description = 'In Progress');
declare @statuscompleted int = (select id from projectstatuses where description = 'Completed');
declare @tasktodo int = (select id from taskstatuses where description = 'To Do');
declare @taskinprogress int = (select id from taskstatuses where description = 'In Progress');
declare @taskblocked int = (select id from taskstatuses where description = 'Blocked');
declare @taskcompleted int = (select id from taskstatuses where description = 'Completed');
declare @prioritylow int = (select id from taskpriorities where description = 'Low');
declare @prioritymedium int = (select id from taskpriorities where description = 'Medium');
declare @priorityhigh int = (select id from taskpriorities where description = 'High');


merge developers as target
using
(
    values
        ('Carlos', 'Ramirez', 'carlos.ramirez@teamtasks.com', 1),
        ('Laura', 'Gomez', 'laura.gomez@teamtasks.com', 1),
        ('Andres', 'Torres', 'andres.torres@teamtasks.com', 1),
        ('Valeria', 'Mendoza', 'valeria.mendoza@teamtasks.com', 1),
        ('Diego', 'Herrera', 'diego.herrera@teamtasks.com', 1)
) as source (firstname, lastname, email, isactive)
on target.email = source.email
when not matched then
insert (firstname, lastname, email, isactive)
values (source.firstname, source.lastname, source.email, source.isactive);


merge projects as target
using
(
    values
        ('Portal de Clientes', 'Banco Nacional', '2026-04-01', '2026-09-30', @statusplanned),
        ('App de Inventario', 'Logística Express', '2026-01-15', '2026-06-30', @statusinprogress),
        ('Módulo de Reportes', 'Salud Total', '2025-06-01', '2025-12-31', @statuscompleted)
) as source (name, clientname, startdate, enddate, statusid)
on target.name = source.name
and target.clientname = source.clientname
when not matched then
insert (name, clientname, startdate, enddate, statusid)
values (source.name, source.clientname, source.startdate, source.enddate, source.statusid);


merge tasks as target
using
(
    values
        (1, 'Definir arquitectura del portal', 'Documentar decisiones técnicas', 1, @tasktodo, @priorityhigh, 3, '2026-04-15', null),
        (1, 'Configurar repositorio', 'Crear estructura base del proyecto', 2, @tasktodo, @prioritymedium, 1, '2026-04-10', null),
        (1, 'Diseño de base de datos', 'Modelar entidades del portal', 3, @tasktodo, @priorityhigh, 4, '2026-04-20', null),
        (1, 'Configurar CI/CD', 'Pipeline de integración continua', 4, @tasktodo, @prioritymedium, 2, '2026-04-25', null),
        (1, 'Definir contrato de APIs', 'Especificación OpenAPI', 5, @tasktodo, @prioritymedium, 2, '2026-04-18', null),
        (1, 'Configurar entornos', 'Dev, QA y Prod', 1, @taskblocked, @prioritylow, 1, '2026-04-12', null),
        (2, 'Módulo de entrada de inventario', 'CRUD de productos entrantes', 2, @taskinprogress, @priorityhigh, 4, '2026-03-20', null),
        (2, 'Módulo de salida de inventario', 'CRUD de productos salientes', 3, @taskinprogress, @priorityhigh, 4, '2026-03-25', null),
        (2, 'Alertas de stock mínimo', 'Notificaciones por umbral', 4, @tasktodo, @prioritymedium, 3, '2026-03-28', null),
        (2, 'Reporte de movimientos', 'Exportar a Excel y PDF', 5, @taskinprogress, @prioritymedium, 3, '2026-04-05', null),
        (2, 'Integración con proveedor', 'API REST de proveedor externo', 1, @taskblocked, @priorityhigh, 5, '2026-03-18', null),
        (2, 'Autenticación de usuarios', 'JWT y roles', 2, @taskcompleted, @priorityhigh, 3, '2026-02-28', '2026-03-05'),
        (2, 'Configuración de base de datos', 'Migraciones y seed inicial', 3, @taskcompleted, @prioritymedium, 2, '2026-02-20', '2026-02-22'),
        (2, 'Diseño de pantallas principales', 'Wireframes aprobados', 4, @taskcompleted, @prioritylow, 1, '2026-02-15', '2026-02-25'),
        (3, 'Reporte de ventas mensual', 'Agrupado por categoría', 5, @taskcompleted, @priorityhigh, 3, '2025-08-30', '2025-08-28'),
        (3, 'Reporte de pacientes activos', 'Filtros por fecha y médico', 1, @taskcompleted, @prioritymedium, 2, '2025-09-15', '2025-09-20'),
        (3, 'Dashboard ejecutivo', 'KPIs del negocio', 2, @taskcompleted, @priorityhigh, 4, '2025-10-01', '2025-10-08'),
        (3, 'Exportación a PDF', 'Todos los reportes', 3, @taskcompleted, @prioritymedium, 3, '2025-11-01', '2025-10-30'),
        (3, 'Envío automático por correo', 'Scheduler semanal', 4, @taskcompleted, @prioritymedium, 3, '2025-11-30', '2025-12-05'),
        (3, 'Pruebas de aceptación', 'UAT con cliente', 5, @taskcompleted, @prioritylow, 2, '2025-12-20', '2025-12-18')
) as source
(
    projectid,
    title,
    description,
    assigneeid,
    statusid,
    priorityid,
    estimatedcomplexity,
    duedate,
    completiondate
)
on target.projectid = source.projectid
and target.title = source.title
when not matched then
insert
(
    projectid,
    title,
    description,
    assigneeid,
    statusid,
    priorityid,
    estimatedcomplexity,
    duedate,
    completiondate
)
values
(
    source.projectid,
    source.title,
    source.description,
    source.assigneeid,
    source.statusid,
    source.priorityid,
    source.estimatedcomplexity,
    source.duedate,
    source.completiondate
);