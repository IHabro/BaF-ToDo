
using BaF_ToDo.Models;
using BaF_ToDo.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace BaF_ToDo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("TaskEntityDbContext") ?? throw new InvalidOperationException("Connection string 'TaskEntityDbContext' not found.");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<TaskEntityDbContext>(options =>
            {
                options.UseInMemoryDatabase("Tasks");
            });

            builder.Services.AddScoped<ITaskEntityRepository, TaskEntityRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                        logger.LogError(exceptionFeature?.Error, "Unhandled exception occurred");

                        await Results.Problem(
                            title: "Nastala neočekávaná chyba.",
                            statusCode: StatusCodes.Status500InternalServerError
                        ).ExecuteAsync(context);
                    });
                });

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
