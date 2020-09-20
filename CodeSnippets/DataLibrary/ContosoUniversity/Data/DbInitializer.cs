using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataLibrary.ContosoUniversity.Models;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary.ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            try
            {
                //// Look for any students.
                //if (context.Students.Any())
                //{
                //    return;   // DB has been seeded
                //}

                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope())
                {
                    Console.WriteLine("Deleting ContosoUniversity database...");
                    context.Database.EnsureDeleted();
                    Console.WriteLine("Creating ContosoUniversity database...");
                    //EnsureCreated() doesn't run the migrations, so SQL objects will be missing
                    //DON'T USE THIS => context.Database.EnsureCreated();
                    //context.Database.EnsureCreated();
                    context.Database.Migrate();

                    var students = new Student[]
                    {
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Carson",
                                LastName = "Alexander",
                                DateOfBirth = DateTime.Now.AddYears(-20),
                                EmailAddress = "Carson.Alexander@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2010-09-01")
                        },
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Meredith",
                                LastName = "Alonso",
                                DateOfBirth = DateTime.Now.AddYears(-21),
                                EmailAddress = "Meredith.Alonso@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2012-09-01")
                        },
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Arturo",
                                LastName = "Anand",
                                DateOfBirth = DateTime.Now.AddYears(-22),
                                EmailAddress = "Arturo.Anand@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2013-09-01")
                        },
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Gytis",
                                LastName = "Barzdukas",
                                DateOfBirth = DateTime.Now.AddYears(-24),
                                EmailAddress = "Gytis.Barzdukas@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2012-09-01")
                        },
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Yan",
                                LastName = "Li",
                                DateOfBirth = DateTime.Now.AddYears(-27),
                                EmailAddress = "Yan.Li@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2012-09-01")
                        },
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Peggy",
                                LastName = "Justice",
                                DateOfBirth = DateTime.Now.AddYears(-29),
                                EmailAddress = "Peggy.Justice@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2011-09-01")
                        },
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Laura",
                                LastName = "Norman",
                                DateOfBirth = DateTime.Now.AddYears(-31),
                                EmailAddress = "Laura.Norman@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2013-09-01")
                        },
                        new Student
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Nino",
                                LastName = "Olivetto",
                                DateOfBirth = DateTime.Now.AddYears(-31),
                                EmailAddress = "Nino.Olivetto@gmail.com"
                            },
                             EnrollmentDate = DateTime.Parse("2005-09-01")
                        },
                    };

                    Console.WriteLine("Adding Students to ContosoUniversity database...");
                    context.Students.AddRange(students);
                    context.SaveChanges();

                    var instructors = new Instructor[]
                    {
                        new Instructor
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Kim",
                                LastName = "Abercrombie",
                                DateOfBirth = DateTime.Now.AddYears(-32),
                                EmailAddress = "Kim.Abercrombie@gmail.com"
                            },
                            HireDate = DateTime.Parse("1995-03-11")
                        },
                        new Instructor
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Fadi",
                                LastName = "Fakhouri",
                                DateOfBirth = DateTime.Now.AddYears(-32),
                                EmailAddress = "Fadi.Fakhouri@gmail.com"
                            },
                            HireDate =  DateTime.Parse("2002-07-06")//DateTime.Parse("1995-03-11")
                        },
                        new Instructor
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Roger",
                                LastName = "Harui",
                                DateOfBirth = DateTime.Now.AddYears(-32),
                                EmailAddress = "Roger.Harui@gmail.com"
                            },
                            HireDate = DateTime.Parse("1998-07-01")
                        },
                        new Instructor
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Candace",
                                LastName = "Kapoor",
                                DateOfBirth = DateTime.Now.AddYears(-32),
                                EmailAddress = "Candace.Kapoor@gmail.com"
                            },
                            HireDate = DateTime.Parse("2001-01-15")
                        },
                        new Instructor
                        {
                            PersonalInformation = new Person()
                            {
                                FirstMidName = "Roger",
                                LastName = "Zheng",
                                DateOfBirth = DateTime.Now.AddYears(-32),
                                EmailAddress = "Roger.Zheng@gmail.com"
                            },
                            HireDate = DateTime.Parse("2004-02-12")
                        },
                    };

                    Console.WriteLine("Adding Instructors to ContosoUniversity database...");
                    context.Instructors.AddRange(instructors);
                    context.SaveChanges();

                    var departments = new Department[]
                    {
                        new Department 
                        { 
                            Name = "English",     
                            Budget = 350000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = context.Instructors.Single( i => i.PersonalInformation.LastName == "Abercrombie").Id 
                        },
                        new Department 
                        { 
                            Name = "Mathematics", 
                            Budget = 100000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = context.Instructors.Single( i => i.PersonalInformation.LastName == "Fakhouri").Id 
                        },
                        new Department 
                        { 
                            Name = "Engineering", 
                            Budget = 350000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = context.Instructors.Single( i => i.PersonalInformation.LastName == "Harui").Id 
                        },
                        new Department 
                        { 
                            Name = "Economics",   
                            Budget = 100000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = context.Instructors.Single( i => i.PersonalInformation.LastName == "Kapoor").Id 
                        }
                    };

                    Console.WriteLine("Adding Departments to ContosoUniversity database...");
                    context.Departments.AddRange(departments);
                    context.SaveChanges();

                    var courses = new Course[]
                    {
                        new Course 
                        {
                            Title = "Chemistry",      
                            Credits = 3,
                            DepartmentID = context.Departments.Single( s => s.Name == "Engineering").Id
                        },
                        new Course 
                        {
                            Title = "Microeconomics", 
                            Credits = 3,
                            DepartmentID = context.Departments.Single( s => s.Name == "Economics").Id
                        },
                        new Course 
                        {
                            Title = "Macroeconomics", 
                            Credits = 3,
                            DepartmentID = context.Departments.Single( s => s.Name == "Economics").Id
                        },
                        new Course 
                        {
                            Title = "Calculus",       
                            Credits = 4,
                            DepartmentID = context.Departments.Single( s => s.Name == "Mathematics").Id
                        },
                        new Course 
                        {
                            Title = "Trigonometry",   
                            Credits = 4,
                            DepartmentID = context.Departments.Single( s => s.Name == "Mathematics").Id
                        },
                        new Course 
                        {
                            Title = "Composition",    
                            Credits = 3,
                            DepartmentID = context.Departments.Single( s => s.Name == "English").Id
                        },
                        new Course 
                        {
                            Title = "Literature",     
                            Credits = 4,
                            DepartmentID = context.Departments.Single( s => s.Name == "English").Id
                        },
                    };

                    Console.WriteLine("Adding Courses to ContosoUniversity database...");
                    context.Courses.AddRange(courses);
                    context.SaveChanges();

                    var officeAssignments = new OfficeAssignment[]
                    {
                        new OfficeAssignment 
                        {
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Fakhouri").Id,
                            Location = "Smith 17" 
                        },
                        new OfficeAssignment 
                        {
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Harui").Id,
                            Location = "Gowan 27" 
                        },
                        new OfficeAssignment 
                        {
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Kapoor").Id,
                            Location = "Thompson 304" 
                        },
                    };

                    Console.WriteLine("Adding OfficeAssignments to ContosoUniversity database...");
                    context.OfficeAssignments.AddRange(officeAssignments);
                    context.SaveChanges();

                    var courseInstructors = new CourseAssignment[]
                   {
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Chemistry" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Kapoor").Id
                        },
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Chemistry" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Harui").Id
                        },
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Microeconomics" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Zheng").Id
                        },
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Macroeconomics" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Zheng").Id
                        },
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Calculus" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Fakhouri").Id
                        },
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Trigonometry" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Harui").Id
                        },
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Composition" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Abercrombie").Id
                        },
                        new CourseAssignment 
                        {
                            CourseID = context.Courses.Single(c => c.Title == "Literature" ).Id,
                            InstructorID = context.Instructors.Single( i => i.PersonalInformation.LastName == "Abercrombie").Id
                        },
                    };

                    Console.WriteLine("Adding CourseAssignments to ContosoUniversity database...");
                    context.CourseAssignments.AddRange(courseInstructors);
                    context.SaveChanges();

                    var enrollments = new Enrollment[]
                    {
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Alexander").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Chemistry" ).Id,
                            Grade = Grade.A
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Alexander").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Microeconomics" ).Id,
                            Grade = Grade.C
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Alexander").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Macroeconomics" ).Id,
                            Grade = Grade.B
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Alonso").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Calculus" ).Id,
                            Grade = Grade.B
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Alonso").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Trigonometry" ).Id,
                            Grade = Grade.B
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Alonso").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Composition" ).Id,
                            Grade = Grade.B
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Anand").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Chemistry" ).Id,
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Anand").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Microeconomics" ).Id,
                            Grade = Grade.B
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Barzdukas").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Chemistry" ).Id,
                            Grade = Grade.B
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Li").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Composition" ).Id,
                            Grade = Grade.B
                        },
                        new Enrollment
                        {
                            StudentID = context.Students.Single(s => s.PersonalInformation.LastName == "Justice").Id,
                            CourseID = context.Courses.Single(c => c.Title == "Literature" ).Id,
                            Grade = Grade.B
                        }
                   };

                    Console.WriteLine("Adding Enrollments to ContosoUniversity database...");
                    context.Enrollments.AddRange(enrollments);
                    context.SaveChanges();

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                    Console.WriteLine("Finished seeding the ContosoUniversity database...");
                } //Using TrasactionScope
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encountered in DBInitialize, Exception: {ex.Message}");
                //throw;
            }
        }
    }
}
