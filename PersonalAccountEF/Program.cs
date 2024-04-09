using PersonalAccountEF;
using PersonalAccountEF.Data.Models;
using PersonalAccountEF.Data;
using PersonalAccountEF.Utility;
using PersonalAccountEF.DBAccess.Controllers;
using Microsoft.Extensions.Logging;
using System;
using Serilog;
using System.Security.Principal;

internal class Program
{
    private static void Main(string[] args)
    {
        using var db = new PersonalAccountContext();

        Console.WriteLine($"Database path: {db.DbPath}.");

        var logger = LoggerCreator.CreateLogger();
        try
        {
            logger.LogInformation("PAEF started");
            var databaseController = new DatabaseController(logger, new PersonalAccountContext());
            
            Console.WriteLine("Inserting a new PA");
            var personalAccount = new PersonalAccount
            {
                Address = new Address
                {
                    City = "Екб",
                    Street = "хехв",
                    HouseNumber = "5/8",
                    ApartmentNumber = 3
                },
                ApartmentArea = 32.4,
                StartDate = new DateOnly(2024, 4, 2),
                EndDate = new DateOnly(2024, 6, 7),
                PersonalAccountNumber = "123 456 789",
                Residents = new List<Resident> {
            new Resident { DateOfBirth = new DateOnly(2002, 7, 15), FirstName = "Владимир", LastName = "Путин"}
            }
            };
            databaseController.TryCreateEntity(personalAccount);

            Console.WriteLine("Querying for a PA");
            databaseController.TryReadEntityById(personalAccount.Id, out PersonalAccount result);

            Console.WriteLine("Updating the PA");
            result.Residents.Add(new Resident { DateOfBirth = new DateOnly(1997, 7, 25), FirstName = "Олег", LastName = "Олег2" });
            databaseController.TryUpdateEntity(result);

            Console.WriteLine("Delete the PA");
            //databaseController.TryDeleteEntityById<PersonalAccount>(result.Id);

        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Unhandled exception");
        }
        finally
        {
            logger.LogInformation("Closing PAEF");
            Log.CloseAndFlush();
        }
    }
}