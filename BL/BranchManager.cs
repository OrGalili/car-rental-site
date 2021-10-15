using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BL
{
    /// <summary>
    /// Manage the Branch entities that in Db.
    /// </summary>
    public class BranchManager
    {
        private CarsRentalContext context;

        /// <summary>
        /// constructor
        /// </summary>
        public BranchManager()
        {
            context = new CarsRentalContext();
        }

        /// <summary>
        /// get all branch names in Db.
        /// </summary>
        /// <returns>string array with the branch names</returns>
        public string[] BranchNamesInDb()
        {
            return context.Branchs.Select(b => b.Name).ToArray();
        }

        /// <summary>
        /// Get a branch entity.
        /// </summary>
        /// <param name="branchName">the branch name of the branch</param>
        /// <returns>branch entity</returns>
        public Branch FindBranch(string branchName)
        {
            return context.Branchs.Where(b => b.Name == branchName).SingleOrDefault();
        }
    }
}
