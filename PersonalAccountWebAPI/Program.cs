
using PersonalAccountEF.Data;
using PersonalAccountEF.DBAccess.Controllers;
using PersonalAccountEF.DBAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using PersonalAccountEF.Utility;
using Serilog;
using PersonalAccountWebAPI.Interfaces;
using PersonalAccountWebAPI.Utility;

namespace PersonalAccountWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var logger = LoggerCreator.CreateLogger();

            // Add services to the container.
            builder.Host.UseSerilog();
            builder.Services.AddSingleton(logger);

            builder.Services.AddControllers();
            builder.Services.AddTransient<PersonalAccountContext>();

            /*var solutionPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
            var databasePath = Path.Combine(solutionPath, "personalAccount.db");*/

            builder.Services.AddTransient<IValidationService, ValidationService>();
            builder.Services.AddTransient<IDatabaseController, DatabaseController>();

            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
