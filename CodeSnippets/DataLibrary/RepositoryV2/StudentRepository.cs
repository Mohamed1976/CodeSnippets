using System;
using System.Collections.Generic;
using DataLibrary.ContosoUniversity.Models;
using DataLibrary.ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataLibrary.RepositoryV2
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolContext context)
            : base(context)
        {
        }

        private SchoolContext SchoolContext
        {
            get { return Context as SchoolContext; }
        }
    }
}