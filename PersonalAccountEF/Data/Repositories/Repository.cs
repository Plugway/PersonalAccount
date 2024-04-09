using Microsoft.EntityFrameworkCore;
using PersonalAccountEF;
using PersonalAccountEF.Data;
using PersonalAccountEF.Data.Models;

namespace PersonalAccountEF.Data.Repositories
{
    internal class Repository<T> where T : class, Interfaces.IEntity
    {
        protected readonly PersonalAccountContext dbContext;

        public Repository(PersonalAccountContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public T? Create(T entity)
        {
            if (entity != null)
            {
                var dataSet = dbContext.Set<T>();
                dataSet.Add(entity);
                dbContext.SaveChanges();
            }
            return entity;
        }

        /// <returns>The entity if one with the given id exists; otherwise, null.</returns>
        public T? ReadById(uint id)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Id == id);
            return searchResult;
        }

        public IEnumerable<T>? ReadAll()
        {
            var dataSet = dbContext.Set<T>();
            return dataSet;
        }

        public T? Update(T entity)
        {
            if (entity != null)
            {
                var dataSet = dbContext.Set<T>();
                dataSet.Update(entity);
                dbContext.SaveChanges();
            }
            return entity;
        }

        /// <returns>The entity if one with the given id exists prior to deletion; otherwise, null.</returns>
        public T? DeleteById(uint id)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Id == id);
            if (searchResult != null)
            {
                dataSet.Remove(searchResult);
                dbContext.SaveChanges();
            }
            return searchResult;
        }

        public T? Exists(uint id)
        {
            T? searchResult = null;
            var dataSet = dbContext.Set<T>();
            searchResult = dataSet.FirstOrDefault(row => row.Id == id);
            return searchResult;
        }

        public IEnumerable<PersonalAccount> GetPersonalAccountsWithResidents()
        {
            return dbContext.PersonalAccounts.Include(pa => pa.Residents).Where(pa => pa.Residents.Count > 0);
        }
        public IEnumerable<PersonalAccount> GetPersonalAccountsByDate(DateOnly date)
        {
            return dbContext.PersonalAccounts.Where(pa => pa.StartDate <= date && pa.EndDate >= date);
        }
        public IEnumerable<PersonalAccount> SearchPersonalAccountsByResidentFullName(string? firstName, string? lastName, string? middleName)
        {
            return dbContext.PersonalAccounts
                .Include(pa => pa.Residents)
                .Where(pa => pa.Residents.Any(r =>
                    (string.IsNullOrEmpty(firstName) || r.FirstName.Contains(firstName)) &&
                    (string.IsNullOrEmpty(lastName) || r.LastName.Contains(lastName)) &&
                    (string.IsNullOrEmpty(middleName) || r.MiddleName.Contains(middleName))));
        }
        public IEnumerable<PersonalAccount> SearchPersonalAccountsByAddress(string? city, string? street, string? houseNumber, int? apartmentNumber)
        {
            return dbContext.PersonalAccounts
                .Where(pa => (string.IsNullOrEmpty(city) || pa.Address.City.Contains(city)) &&
                             (string.IsNullOrEmpty(street) || pa.Address.Street.Contains(street)) &&
                             (string.IsNullOrEmpty(houseNumber) || pa.Address.HouseNumber.Contains(houseNumber)) &&
                             (!apartmentNumber.HasValue || pa.Address.ApartmentNumber == apartmentNumber.Value));
        }
    }
}