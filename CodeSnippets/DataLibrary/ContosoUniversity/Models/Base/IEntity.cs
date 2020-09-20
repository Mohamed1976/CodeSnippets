using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.ContosoUniversity.Models.Base
{
    //Instead of of inheriting from BaseEntity we can implement IEntity 
    public interface IEntity
    {
        int Id { get; set; }
        byte[] TimeStamp { get; set; }
        bool IsDeleted { get; set; }
    }
}
