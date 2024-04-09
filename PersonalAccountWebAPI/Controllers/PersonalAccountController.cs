using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAccountEF.Data;
using PersonalAccountEF.Data.Models;
using PersonalAccountEF.DBAccess.Controllers;
using PersonalAccountEF.DBAccess.Interfaces;
using PersonalAccountWebAPI.Interfaces;
using PersonalAccountWebAPI.Utility;

namespace PersonalAccountWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalAccountController : ControllerBase
    {
        private readonly ILogger<PersonalAccountController> logger;
        private readonly IDatabaseController service;
        private readonly IValidationService validationService;

        public PersonalAccountController(IDatabaseController service, IValidationService validationService, ILogger<PersonalAccountController> logger)
        {
            this.logger = logger;
            this.service = service;
            this.validationService = validationService;
        }

        // GET: api/PersonalAccount
        [HttpGet]
        public ActionResult<IEnumerable<PersonalAccount>> GetAll()
        {
            var result = service.TryReadAllEntities(out List<PersonalAccount>? personalAccounts);
            if (!result.IsSuccess)
            {
                return NotFound();
            }
            return personalAccounts;
        }

        // GET: api/PersonalAccount/[id]
        [HttpGet("{id}")]
        public ActionResult<PersonalAccount> GetById(uint id) 
        {
            var result = service.TryReadEntityById(id, out PersonalAccount personalAccount);
            if (!result.IsSuccess)
            {
                return NotFound();
            }
            return personalAccount;
        }

        // POST: api/PersonalAccount
        [HttpPost]
        public ActionResult<PersonalAccount> PostPersonalAccount(PersonalAccount personalAccount)
        {
            var validationResult = validationService.ValidatePersonalAccount(personalAccount);
            if (!validationResult.IsSuccess)
            {
                return BadRequest(validationResult.ErrorDescription);
            }
            var result = service.IsUniquePANumber(out bool isUniquePANumber, personalAccount.PersonalAccountNumber);
            if (!result.IsSuccess)
            {
                return Problem("Unable to perform unique check.");
            }
            if (!isUniquePANumber)
            {
                return Conflict("Personal account number must be unique.");
            }
            result = service.TryCreateEntity(personalAccount);
            if (!result.IsSuccess)
            {
                BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = personalAccount.Id }, personalAccount);
        }

        // PUT: api/PersonalAccount/[id]
        [HttpPut("{id}")]
        public IActionResult PutPersonalAccount(uint id, PersonalAccount personalAccount)
        {
            if (id != personalAccount.Id)
            {
                return BadRequest();
            }
            var validationResult = validationService.ValidatePersonalAccount(personalAccount);
            if (!validationResult.IsSuccess)
            {
                return BadRequest(validationResult.ErrorDescription);
            }

            var result = service.TryUpdateEntity(personalAccount);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorDescription);
            }

            return NoContent();
        }

        // DELETE: api/PersonalAccount/[id]
        [HttpDelete("{id}")]
        public IActionResult DeletePersonalAccount(uint id)
        {
            var result = service.TryDeleteEntityById<PersonalAccount>(id);
            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/PersonalAccount/WithResidents
        [HttpGet("WithResidents")]
        public ActionResult<IEnumerable<PersonalAccount>> GetPersonalAccountsWithResidents()
        {
            var result = service.GetPersonalAccountsWithResidents(out IEnumerable<PersonalAccount> personalAccounts);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorDescription);
            }
            return Ok(personalAccounts);
        }

        // GET: api/PersonalAccount/ByDate?Date=2023-04-01
        [HttpGet("ByDate")]
        public ActionResult<IEnumerable<PersonalAccount>> GetPersonalAccountsByDate(DateOnly date)
        {
            var result = service.GetPersonalAccountsByDate(out IEnumerable<PersonalAccount> personalAccounts, date);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorDescription);
            }
            return Ok(personalAccounts);
        }

        // GET: api/PersonalAccount/SearchByResidentFullName?firstName=John&lastName=Doe&middleName=Coby
        [HttpGet("SearchByResidentFullName")]
        public ActionResult<IEnumerable<PersonalAccount>> SearchPersonalAccountsByResidentFullName(string? firstName = null, string? lastName = null, string? middleName = null)
        {
            var result = service.SearchPersonalAccountsByResidentFullName(out IEnumerable<PersonalAccount> personalAccounts, firstName, lastName, middleName);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorDescription);
            }
            return Ok(personalAccounts);
        }

        // GET: api/PersonalAccount/SearchByAddress?city=New York&street=Main St&houseNumber=123&apartmentNumber=101
        [HttpGet("SearchByAddress")]
        public ActionResult<IEnumerable<PersonalAccount>> SearchPersonalAccountsByAddress(string? city = null, string? street = null, string? houseNumber = null, int? apartmentNumber = null)
        {
            var result = service.SearchPersonalAccountsByAddress(out IEnumerable<PersonalAccount> personalAccounts, city, street, houseNumber, apartmentNumber);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorDescription);
            }
            return Ok(personalAccounts);
        }

        private bool PersonalAccountExists(uint id)
        {
            service.IsEntityExists<PersonalAccount>(out bool exists, id);
            return exists;
        }
    }
}
