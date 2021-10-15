using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// this is the data base builder.
    /// </summary>
    public class InitializeDataBase
    {
        public InitializeDataBase()
        {
            var context = new CarsRentalContext();
            context.Database.Initialize(true);
        }
    }
}
