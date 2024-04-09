using PersonalAccountEF.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccountEF.Data.Models
{
    public class PersonalAccount : IEntity
    {
        public uint Id { get; set; }
        public string PersonalAccountNumber { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Address Address { get; set; } = null!;
        public double ApartmentArea { get; set; }
        public List<Resident> Residents { get; set; } = null!;
    }
}
