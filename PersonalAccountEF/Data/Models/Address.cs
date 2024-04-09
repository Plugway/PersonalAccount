using PersonalAccountEF.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccountEF.Data.Models
{
    public class Address : IEntity
    {
        public uint Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public int ApartmentNumber { get; set; }
    }
}
