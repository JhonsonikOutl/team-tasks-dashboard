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
    values (@ProjectId, @Title, @Description, @AssigneeId, @StatusId, @PriorityId, @EstimatedComplexity, @DueDate)
end
go