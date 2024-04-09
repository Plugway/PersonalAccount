using PersonalAccountEF.Data.Models;
using PersonalAccountEF.Utility;

namespace PersonalAccountWebAPI.Interfaces
{
    public interface IValidationService
    {
        public Result ValidatePersonalAccount(PersonalAccount personalAccount);
        public Result ValidateAddress(Address address);
        public Result ValidateResident(Resident resident);

    }
}
