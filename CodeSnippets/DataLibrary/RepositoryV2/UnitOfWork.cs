using DataLibrary.ContosoUniversity.Data;
using System;
using System.Collections.Generic;
using System.Text;

/*
https://www.thereformedprogrammer.net/is-the-repository-pattern-useful-with-entity-framework-core/
https://rob.conery.io/2014/03/04/repositories-and-unitofwork-are-not-a-good-idea/
https://stackoverflow.com/questions/27527757/ef6-code-first-with-generic-repository-and-dependency-injection-and-soc
https://martinfowler.com/eaaCatalog/unitOfWork.html
Whenever we want to swap our repository implementation we have to change the unitofwork class.
For example for testing, we can create a Mockup repo, inheriting from Repository and implementing 
ICourseRepository, so in there we set a context to elsewhere or not setting at all, and 
returning static data from the methods

There is an IUnitOfWork interface because you would create new concrete UnitOfWork classes depending 
on the "unit of work" you want to make transactional. So for example, if you are working with Orders, 
you might need to use the OrderRepository and CatalogRepository. In that case you would create an 
CustomerOrderUnitOfWork class that has OrderRepository and CatalogRepository as it's properties. 
So you would need multiple concrete UnitOfWork implementations to support your various transactional queries.

Lower memory usage and performance impact by using  AsNoTracking() and perhaps also .Attach(), EntityState.Modified and EntityState.Detached.

Alternatively we could just use EF, EF already does it and you could use Specification patterns to 
easily overcome the problem of repeating querying logic.

Another idea if you still want to have a class named Repository: Just make it a simple class 
where you inject the DbContext. It would sort of work as a decorator over the DbContext.

Or use CQRS, IMediator IMapper 

Register IUnitOfWork with DI 
 */
namespace DataLibrary.RepositoryV2
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolContext _context;

        public UnitOfWork(SchoolContext context)
        {
            _context = context;
            Courses = new CourseRepository(_context);
            Instructors = new InstructorRepository(_context);
            Students = new StudentRepository(_context);
        }

        public ICourseRepository Courses { get; private set; }

        public IInstructorRepository Instructors { get; private set; }

        public IStudentRepository Students { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
