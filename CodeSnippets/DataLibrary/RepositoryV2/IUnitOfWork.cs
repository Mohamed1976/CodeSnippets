using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.RepositoryV2
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        IInstructorRepository Instructors { get; }
        IStudentRepository Students { get; }
        int Complete();
    }
}
