using System;
using System.Collections.Generic;
using DataLibrary.ContosoUniversity.Models;
using DataLibrary.ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataLibrary.RepositoryV2
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(SchoolContext context)
            : base(context)
        {
        }

        public IEnumerable<Course> GetTopEnrollmentCourses(int count)
        {
            IEnumerable<Course> courses = SchoolContext.Courses
                .Include(c => c.Enrollments)
                .OrderByDescending(c => c.Enrollments.Count)
                .Take(count)
                .ToList();
            
            return courses;
        }

        private SchoolContext SchoolContext
        {
            get { return Context as SchoolContext; }
        }
    }
}
