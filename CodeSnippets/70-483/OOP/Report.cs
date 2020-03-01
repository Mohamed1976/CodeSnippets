using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    public interface IPrintable
    {
        string GetPrintableText(int pageWidth, int pageHeight);
        string GetTitle();
    }
    public interface IDisplay
    {
        string GetTitle();
    }

    //The Report class contains two methods: GetPrintableText and
    //GetTitle, which are declared in the IPrintable interface. These methods
    //have been made explicit implementations of the interface by preceding the
    //method name with the name of the interface they are implementing.
    public class Report : IPrintable, IDisplay
    {
        string IPrintable.GetPrintableText(int pageWidth, int pageHeight)
        {
            return "Report text to be printed";
        }

        string IPrintable.GetTitle()
        {
            return "Report title to be printed";
        }

        string IDisplay.GetTitle()
        {
            return "Report title to be displayed";
        }

        public string GetTitle()
        {
            return "General Report title to be displayed";
        }

    }
}
