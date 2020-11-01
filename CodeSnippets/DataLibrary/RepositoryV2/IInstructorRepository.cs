using System;
using System.Collections.Generic;
using System.Text;
using DataLibrary.ContosoUniversity.Models;

namespace DataLibrary.RepositoryV2
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        IEnumerable<Instructor> GetInstructors(int page, int pageSize = 10);
        IEnumerable<Instructor> GetInstructorsWithCourse(int page, int pageSize = 10);
    }
}
