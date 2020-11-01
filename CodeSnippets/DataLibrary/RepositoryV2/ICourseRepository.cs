using System;
using System.Collections.Generic;
using System.Text;
using DataLibrary.ContosoUniversity.Models;

namespace DataLibrary.RepositoryV2
{
    public interface ICourseRepository : IRepository<Course>
    {
        IEnumerable<Course> GetTopEnrollmentCourses(int count);
    }
}
