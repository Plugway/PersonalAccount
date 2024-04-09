using PersonalAccountEF.Data.Models;
using PersonalAccountEF.Utility;

namespace PersonalAccountEF.DBAccess.Interfaces
{
    public interface IDatabaseController
    {
        public Result TryCreateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity;
        public Result TryReadEntityById<T>(uint id, out T entity) where T : class, Data.Interfaces.IEntity, new();
        public Result TryReadAllEntities<T>(out List<T> entities) where T : class, Data.Interfaces.IEntity, new();
        public Result TryUpdateEntity<T>(T entity) where T : class, Data.Interfaces.IEntity;
        public Result TryDeleteEntityById<T>(uint id) where T : class, Data.Interfaces.IEntity;
        public Result IsEntityExists<T>(out bool exists, uint id) where T : class, Data.Interfaces.IEntity;
        public Result IsUniquePANumber(out bool isUniquePANumber, string PANumber);
        public Result GetPersonalAccountsWithResidents(out IEnumerable<PersonalAccount> personalAccounts);
        public Result GetPersonalAccountsByDate(out IEnumerable<PersonalAccount> personalAccounts, DateOnly date);
        public Result SearchPersonalAccountsByResidentFullName(out IEnumerable<PersonalAccount> personalAccounts, string? firstName = null, string? lastName = null, string? middleName = null);
        public Result SearchPersonalAccountsByAddress(out IEnumerable<PersonalAccount> personalAccounts, string? city = null, string? street = null, string? houseNumber = null, int? apartmentNumber = null);
    }
}
