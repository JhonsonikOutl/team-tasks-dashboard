using Dapper;
using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Interfaces.Services;
using TeamTasks.Application.Services;
using TeamTasks.Infrastructure.Options;
using TeamTasks.Infrastructure.Persistence;
using TeamTasks.Infrastructure.Repositories;
using TeamTasks.Infrastructure.TypeHandlers;

namespace TeamTasks.API.Extensions
{
    static class ServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            services.Configure<DatabaseOptions>(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ISeedRepository, SeedRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IDeveloperService, DeveloperService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ISeedService, SeedService>();

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DevPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}