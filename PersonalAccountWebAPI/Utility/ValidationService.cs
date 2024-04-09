using PersonalAccountEF.Data.Models;
using PersonalAccountEF.Utility;
using PersonalAccountWebAPI.Interfaces;
using System.Text.RegularExpressions;

namespace PersonalAccountWebAPI.Utility
{
    public class ValidationService : IValidationService
    {
        public ValidationService() { }
        public Result ValidatePersonalAccount(PersonalAccount personalAccount)
        {
            if (string.IsNullOrWhiteSpace(personalAccount.PersonalAccountNumber))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Personal account number is required."
                };
            }

            if (personalAccount.StartDate >= personalAccount.EndDate)
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Start date must be before end date."
                };
            }

            if (personalAccount.Address == null)
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Address is required."
                };
            }
            var addrRes = ValidateAddress(personalAccount.Address);
            if (!addrRes.IsSuccess)
            {
                return addrRes;
            }

            if (personalAccount.ApartmentArea <= 0)
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Apartment area must be greater than zero."
                };
            }

            if (personalAccount.Residents == null)
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Residents list is required."
                };
            }
            foreach (Resident resident in personalAccount.Residents)
            {
                var residentRes = ValidateResident(resident);
                if (!residentRes.IsSuccess)
                {
                    return residentRes;
                }
            }

            return new Result { IsSuccess = true };
        }

        public Result ValidateAddress(Address address)
        {
            if (string.IsNullOrWhiteSpace(address.City))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "City is required."
                };
            }
            if (string.IsNullOrWhiteSpace(address.Street))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Street is required."
                };
            }
            if (string.IsNullOrWhiteSpace(address.HouseNumber))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "House number is required."
                };
            }
            if (address.ApartmentNumber <= 0)
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Apartment number must be greater than zero."
                };
            }
            return new Result { IsSuccess = true };
        }

        public Result ValidateResident(Resident resident)
        {
            if (string.IsNullOrWhiteSpace(resident.FirstName))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "First name is required."
                };
            }
            if (string.IsNullOrWhiteSpace(resident.LastName))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Last name is required."
                };
            }
            if (resident.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Date of birth must be before current date."
                };
            }
            if (!string.IsNullOrWhiteSpace(resident.PhoneNumber))
            {
                var phone = resident.PhoneNumber.Replace("+", "")
                    .Replace(" ", "").Replace("(", "").Replace(")", "")
                    .Replace("-", "");
                Regex regex = new(@"^\d+$");
                if (!regex.IsMatch(phone) || !(phone.Length == 11))
                {
                    return new Result
                    {
                        IsSuccess = false,
                        ErrorDescription = "Phone number's format is incorrect."
                    };
                }
            }
            if (!string.IsNullOrWhiteSpace(resident.Email) && !resident.Email.Contains('@'))
            {
                return new Result
                {
                    IsSuccess = false,
                    ErrorDescription = "Email's format is incorrect."
                };
            }
            return new Result { IsSuccess = true };
        }
    }
}
