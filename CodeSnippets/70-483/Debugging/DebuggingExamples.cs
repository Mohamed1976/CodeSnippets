//Preprocessor compiler directives, These are commands that are pre-ceded by the # character.
//#define DIAGNOSTICS
#define RELEASEMODE
#define LOGGING
#define CLASS_OBSOLETE

//You can also use the #undef directive to undefine any symbols that might
//have been defined outside your program source.Note that the
//#undef directive can only be used at the very start of the program file. You can use it to remove
//the DEBUG symbol so that elements of code that you are debugging can be made
//to run in release mode, without DEBUG defined.
//#undef DEBUG 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

//You might think that conditional compilation is very powerful.And you
//would be right. However, you should use it with care. It makes code much
//harder to understand.The reader of the program has to be aware which symbols
//are defined when they read the program text.
//Conditional compilation might be useful if you are trying to create a single
//version of a program that is intended to operate on multiple platforms.Platform
//specific behaviors can be selected using symbols that are set for each different configuration.

namespace _70_483.Debugging
{
    public class DebuggingExamples
    {
        public DebuggingExamples()
        {

        }

        public void Run()
        {

            //The most important thing to remember about conditional compilation is that if
            //the condition is not fulfilled the code controlled by conditional compilation is
            //never included in the assembly file output. The Visual Studio editor is aware of
            //this and will show conditionally compiled elements as “grayed out” if they are not active.
            //Use the #if #else or #elif  directive to control code compilation.
            //An #if directive can have an accompanying #else element that identifies
            //statements to be compiled if the symbol is not defined. You can use the #elif
            //directive to create a chain of conditions
#if DIAGNOSTICS
            Console.WriteLine("DIAGNOSTICS is defined.");
#elif RELEASEMODE
            Console.WriteLine("RELEASEMODE is defined.");
#endif

#if DIAGNOSTICS
            Console.WriteLine("DIAGNOSTICS is defined.");
#else
            Console.WriteLine("RELEASEMODE is defined.");
#endif

            //The condition that is tested can be a logical expression that combines tests for multiple symbols.
#if RELEASEMODE && LOGGING
            Console.WriteLine("RELEASEMODE && LOGGING is defined.");
#endif

            //In the application properties->build, TRACE, DEBUG constants can be defined 
            //The DEBUG constant is only defined if your program is built using the debug build type.
            //Additional conditional compilation symbols can also be defined in the properties of an application.
#if TRACE
            Console.WriteLine("TRACE is defined.");
#endif

            display("Program finished OK.");

            OldMethod();

            //Control compilation with warning and error directives
            //The #warning and #error directives allow you to produce a compiler
            //warning or even prevent compilation of a source code file.

#warning This version of the library is no longer maintained. Use Version 2.5

#if DIAGNOSTICS && DEBUG
#error Cannot run with both diagnostics and debug enabled
#endif

            try
            {
                Exploder();
            }
            catch (Exception e)
            {
                //The stacktrace will show #line 1 "kapow.ninja"
                Console.WriteLine(e.StackTrace);
            }

            //Hide code using #line
            //Another use for the
            //#line directive is to hide code statements from the
            //debugger. This can be very useful if your source code contains programmatically
            //generated elements. You don’t want to have to step through all the
            //programmatically generated statements when debugging, because you are only
            //interested in the code that you have written yourself.
#line hidden
            // The debugger will not step through these statements
            Console.WriteLine("You haven't seen me");
#line default

            Update();

            //NOP (no operation) statements are removed from the release version, code is optimised
            //The Debug file also contains nop statements.The word nop is an
            //abbreviation of “no operation”. When the program runs, these statements don’t
            //do anything.They are only present to serve as locations in the program that can
            //be used as the position of breakpoints.We only tend to use breakpoints when
            //debugging, so these are omitted in the release build.

            //Code in release mode is optimised
            //The release build of a program may differ from the debug build in other ways
            //too.The release version may contain “inline” implementations of short methods.
            //If a method body is very short the compiler may decide that the program runs
            //faster if the method body was copied “inline” each time the method is called,
            //rather than generating the instructions that manage a method call.
            //A release version may also be more aggressive about the removal of objects as
            //it runs, meaning that finalizer methods(see Chapter 2, Skill 6) are more likely to
            //be called on objects.

            //When Visual Studio builds a program, it produces more than just the executable assembly,
            //it also produces the Program Debug Database file (PDB)
            //The program debug database file is sometimes called the symbol file.
            //The statement with a breakpoint actually maps to the indicated
            //statement in the compiled output.It is the debug database that contains this
            //mapping.The debug database also contains the names of all the symbols in the
            //program and their addresses in the program memory space when the program runs.
            //A new program debug database is generated each time a program is compiled.
            //A database file contains a Globally Unique Identifier(GUID), which is also held
            //in the executable file with which the debug database is associated.When the
            //debug database is opened this value is checked, and only the database with the
            //matching GUID can be used.In other words, it is not possible to use an “old”
            //debug database file with a newly compiled executable.
            //A program debug file will contain two kinds of symbol information: public and
            //private. The public information contains descriptions of elements that are public
            //in that assembly.The private information also contains descriptions of things
            //like private methods and local variables.Visual Studio creates debug files that
            //contain both private and public components.You might not want other people to
            //be able to see information about private elements in your solution.The tool
            //pdbcopy can be used to make a copy of a pdb file and remove all the private
            //elements.You can also provide pdbcopy with a list of symbols to be removed.
            //Note that the output from pdbcopy has the same identifying GUID as an
            //incoming file, so it can be used with the same executable file.

            //A symbol server provides debug database information to a program in place of a
            //local debug database file.If you really want to step through the Microsoft
            //elements of our program you can connect Visual Studio to a Symbol Server that
            //provides the information that would normally be in the debug database.
            //A symbol server provides debug database that can be cached

           //If you use Team Foundation Server(TFS) to manage your projects, you have
           //the option of saving all debug data files to a central server from which they can
           //be accessed as required.
        }

        //Use DebuggerStepThrough to prevent debugger stepping
        //You can use the[DebuggerStepThrough] attribute to mark a method or
        //class so that the debugger will not debug each statement in turn when single
        //stepping through the code in that method or class. This is also useful when a
        //program contains programmatically generated elements that you don’t want to step through.
        //Note that if you apply the attribute to a class, it will mean that the debugger
        //will not step through any of the methods in the class. Note also that the program
        //will not hit any breakpoints that are set in elements marked with this attribute.
        [DebuggerStepThrough]
        public void Update()
        {
            Console.WriteLine("Debugger dont step through me.");
        }

        //Note that the use of the Conditional attribute does not control whether or
        //not the method is included in the compiled assembly file output by the compiler.
        //The method is always included in the assembly output. However, the
        //Conditional output will prevent the method from being called.
        [Conditional("LOGGING")]
        private void display(string message)
        {
            Console.WriteLine(message);
        }

        // You can mark classes, interfaces, methods and properties with the Obselete
        // attribute to indicate that they have been superseded by new versions.The
        // Obselete attribute is applied to the superseded element and given a message
        // and a Boolean value that indicates whether a compiler warning(false) or an error
        //(true) should be produced if the element with the attribute is used by a program.
        //Mark code as obsolete using the Obsolete directive
        [Obsolete("This method is obsolete. Call NewMethod instead.", false)]
        public void OldMethod()
        {
            Console.WriteLine("Calling old method.");
        }

        //Manage compiler warning reporting with the pragma directive
        //When writing programs, it is best to see no warnings reported at the end of the
        //build process.However, sometimes you can use code constructions that trigger
        //warnings from the compiler even though the code is correct in the context it is
        //being used.In this situation the #pragma warning directive can be used to
        //disable warning reporting for a region of code.
        //The #pragma warning disable directive turns off any
        //warning reporting.Warning reporting can be turned back on with a
        //#pragma warning restore statement.
        //The problem with disabling all warnings is that this would cause all other
        //warnings to be disabled, as well as the one that I know to ignore.You can
        //improve on a warning filtering by just specifying the warning that you want the
        //compiler to ignore.The pragma next will ignore just the warning CS1998, which
        //is the warning about async methods.If you want to ignore multiple warnings you
        //can supply them as a comma separated list.

#pragma warning disable CS1998
        public async Task<int> Resume()
        {
            return 2020;
        }
#pragma warning restore CS1998

        //Some of the code that you work with may contain auto-generated elements.
        //These code components may be inserted into your program files.This happens
        //with systems such as ASP.NET.These can lead to confusion navigating error
        //reports, where the line number that is reported doesn’t match the one you see in
        //the Visual Studio editor. The #line directive can be used to set the reported line
        //numbers of statements in your code. You can also use the
        //#line directive to specify the file name delivered by error reporting.

        //Note that changing the filename in this way will cause problems when
        //debugging, because the run time system will look for a program database file
        //(more in a moment), which corresponds to the filename of the executable.If this
        //filename has been changed by using the #line directive the system will not be
        //able to display any debugging information.Note also that the
        //#line directive will change the filename and line number reported by the compiler if an error or
        //warning is discovered during compilation.
        private void Exploder()
        {
#line 1 "kapow.ninja"
            throw new Exception("Bang");
#line default
        }
    }
}
