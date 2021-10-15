using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BL.SearchModels
{
    public class CarSearch
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public Gear GearType { get; set; }
        public string FreeText { get; set; }
        public DateTime LeaseTimeFrom { get; set; }
        public DateTime LeaseTimeTo { get; set; }
    }
}
