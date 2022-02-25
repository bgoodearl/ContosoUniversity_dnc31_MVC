using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoUniversity.DAL.Interfaces
{
    public interface ISchoolDbContextFactory
    {
        ISchoolDbContext GetSchoolDbContext();
    }
}
