using System;
using System.Collections.Generic;
using DataLibrary.ContosoUniversity.Models;
using DataLibrary.ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataLibrary.RepositoryV2
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(SchoolContext context)
            : base(context)
        {
        }

        public IEnumerable<Instructor> GetInstructors(int page, int pageSize = 10)
        {
            IEnumerable<Instructor> instructors = SchoolContext.Instructors
                .OrderBy(i => i.PersonalInformation.LastName)
                .ThenBy(i => i.PersonalInformation.FirstMidName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return instructors;
        }

        public IEnumerable<Instructor> GetInstructorsWithCourse(int page, int pageSize = 10)
        {
            IEnumerable<Instructor> instructors = SchoolContext.Instructors
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                .OrderBy(i => i.PersonalInformation.LastName)
                .ThenBy(i => i.PersonalInformation.FirstMidName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return instructors;
        }

        private SchoolContext SchoolContext
        {
            get { return Context as SchoolContext; }
        }
    }
}
