using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{

    public enum PublicationType { Misc, Book, Magazine, Article };

    //Because the Publication class is abstract, it cannot be instantiated directly from code
    public abstract class Publication
    {
        private bool published = false;
        private DateTime datePublished;
        private int totalPages;

        public Publication(string title, string publisher, PublicationType type)
        {
            if (publisher == null)
                throw new ArgumentNullException("The publisher cannot be null.");
            else if (String.IsNullOrWhiteSpace(publisher))
                throw new ArgumentException("The publisher cannot consist only of white space.");
            Publisher = publisher;

            if (title == null)
                throw new ArgumentNullException("The title cannot be null.");
            else if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("The title cannot consist only of white space.");
            Title = title;

            Type = type;
        }

        public string Publisher { get; }

        public string Title { get; }

        public PublicationType Type { get; }

        public string CopyrightName { get; private set; }

        public int CopyrightDate { get; private set; }

        public int Pages
        {
            get { return totalPages; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("The number of pages cannot be zero or negative.");
                totalPages = value;
            }
        }

        public string GetPublicationDate()
        {
            if (!published)
                return "NYP";
            else
                return datePublished.ToString("d");
        }

        public void Publish(DateTime datePublished)
        {
            published = true;
            this.datePublished = datePublished;
        }

        public void Copyright(string copyrightName, int copyrightDate)
        {
            if (copyrightName == null)
                throw new ArgumentNullException("The name of the copyright holder cannot be null.");
            else if (String.IsNullOrWhiteSpace(copyrightName))
                throw new ArgumentException("The name of the copyright holder cannot consist only of white space.");
            CopyrightName = copyrightName;

            int currentYear = DateTime.Now.Year;
            if (copyrightDate < currentYear - 10 || copyrightDate > currentYear + 2)
                throw new ArgumentOutOfRangeException($"The copyright year must be between {currentYear - 10} and {currentYear + 1}");
            CopyrightDate = copyrightDate;
        }

        public override string ToString() => Title;

        public virtual void DisplayInfo()
        {
            Console.WriteLine("{0}, {1}, {2}", Publisher, Title, Type.ToString());              
        }
    }
}
