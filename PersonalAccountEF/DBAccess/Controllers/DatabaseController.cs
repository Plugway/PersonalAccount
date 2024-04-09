using PersonalAccountEF.Data;
using PersonalAccountEF.DBAccess.Interfaces;
using PersonalAccountEF.Utility;
using Microsoft.Extensions.Logging;
using PersonalAccountEF.Data.Models;

namespace PersonalAccountEF.DBAccess.Controllers
{
    public class DatabaseController : IDatabaseController
    {
        public DatabaseController(ILogger logger, PersonalAccountContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        private ILogger Logger { get; }
        private PersonalAccountContext DbContext { get; set; }

        public Result TryCreateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var createdEntity = repository.Create(entity);
                DbContext = new PersonalAccountContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to create entity of type {EntityType}", typeof(T).Name);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to create entity: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }

        public Result TryReadEntityById<T>(uint id, out T entity) where T : class, Data.Interfaces.IEntity, new()
        {
            entity = new T();

            try
            {
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var readEntity = repository.ReadById(id);
                if (readEntity == null)
                    return new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = $"Couldn't find entity of type {typeof(T).Name} with id {id}."
                    };

                entity = readEntity;
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to read entity of type {EntityType} by id {Id}", typeof(T).Name, id);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to read entity: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }
        public Result TryReadAllEntities<T>(out List<T> entities) where T : class, Data.Interfaces.IEntity, new()
        {
            entities = new List<T>();

            try
            {
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var readEntities = repository.ReadAll();
                if (readEntities == null)
                    return new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = $"Couldn't find any entity of type {typeof(T).Name}."
                    };

                entities = readEntities.ToList();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to read entities of type {EntityType}", typeof(T).Name);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to read entities: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }

        public Result TryUpdateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var updatedEntity = repository.Update(entity);
                DbContext = new PersonalAccountContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to update entity of type {EntityType}", typeof(T).Name);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to update entity: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }

        public Result TryDeleteEntityById<T>(uint id) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(DbContext);
                repository.DeleteById(id);
                DbContext = new PersonalAccountContext();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to delete entity of type {EntityType} by id {Id}", typeof(T).Name, id);
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to delete entity: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }
        public Result IsEntityExists<T>(out bool exists, uint id) where T : class, Data.Interfaces.IEntity
        {
            try
            {
                var repository = new Data.Repositories.Repository<T>(DbContext);
                var res = repository.Exists(id);
                exists = res != null;
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to find something.");
                exists = false;
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to find something. See the log file for the detailed exception and stack trace."
                };
            }
        }
        public Result IsUniquePANumber(out bool isUniquePANumber, string PANumber)
        {
            try
            {
                var repository = new Data.Repositories.Repository<PersonalAccount>(DbContext);
                var paList = repository.ReadAll();
                isUniquePANumber = !paList.Any(account => account.PersonalAccountNumber == PANumber);
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to find something.");
                isUniquePANumber = false;
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to find something. See the log file for the detailed exception and stack trace."
                };
            }
        }
        public Result GetPersonalAccountsWithResidents(out IEnumerable<PersonalAccount> personalAccounts)
        {
            personalAccounts = null;

            try
            {
                var repository = new Data.Repositories.Repository<PersonalAccount>(DbContext);
                personalAccounts = repository.GetPersonalAccountsWithResidents();
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to get personal accounts with residents.");
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to get personal accounts with residents: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }
        public Result GetPersonalAccountsByDate(out IEnumerable<PersonalAccount> personalAccounts, DateOnly date)
        {
            personalAccounts = null;

            try
            {
                var repository = new Data.Repositories.Repository<PersonalAccount>(DbContext);
                personalAccounts = repository.GetPersonalAccountsByDate(date);
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to get personal accounts by date.");
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to get personal accounts by date: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }
        public Result SearchPersonalAccountsByResidentFullName(out IEnumerable<PersonalAccount> personalAccounts, string? firstName = null, string? lastName = null, string? middleName = null)
        {
            personalAccounts = null;

            try
            {
                var repository = new Data.Repositories.Repository<PersonalAccount>(DbContext);
                personalAccounts = repository.SearchPersonalAccountsByResidentFullName(firstName, lastName, middleName);
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to search personal accounts by resident full name");
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to search personal accounts by resident full name: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }
        public Result SearchPersonalAccountsByAddress(out IEnumerable<PersonalAccount> personalAccounts, string? city = null, string? street = null, string? houseNumber = null, int? apartmentNumber = null)
        {
            personalAccounts = null;

            try
            {
                var repository = new Data.Repositories.Repository<PersonalAccount>(DbContext);
                personalAccounts = repository.SearchPersonalAccountsByAddress(city, street, houseNumber, apartmentNumber);
                return new Result() { IsSuccess = true };
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to search personal accounts by address");
                return new Result()
                {
                    IsSuccess = false,
                    ErrorDescription = $"Failed to search personal accounts by address: {e.Message} See the log file for the detailed exception and stack trace."
                };
            }
        }
    }
}
