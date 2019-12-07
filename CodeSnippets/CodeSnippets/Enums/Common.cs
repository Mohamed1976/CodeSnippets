using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CodeSnippets.Enums
{
    //Naming convention, StringComparisonOperatorsEnum 
    public enum StringComparisonOperators
    {
        //By default, the first member of an enum has the value 0 and the value of each successive enum member is increased by 1. 
        //For example, in the following enumeration, None is 0, Equals is 1, NotEquals is 2 and so forth.
        [Description("Select Operator")]
        None,
        [Description("Equals")]
        Equals,
        [Description("Not Equals")]
        NotEquals,
        [Description("Contains")]
        Contains,
        [Description("Not Contains")]
        NotContains,
        [Description("Starts With")]
        StartsWith,
        [Description("Ends With")]
        EndsWith
    };

    public enum NumericComparisonOperators
    {
        [Description("Select Operator")]
        None = 0,
        [Description("Is Equal To")]
        IsEqualTo,
        [Description("Is Not Equal To")]
        IsNotEqualTo,
        [Description("Is Between")]
        IsBetween,
        [Description("Is Greater Than Or Equal To")]
        IsGreaterThanOrEqualTo,
        [Description("Is Greater Than")]
        IsGreaterThan,
        [Description("Is Less Than Or Equal To")]
        IsLessThanOrEqualTo,
        [Description("Is Less Than")]
        IsLessThan
    }

    public enum ListComparisonOperators
    {
        [Description("Select Operator")]
        None = 0,
        [Description("Contains")]
        Contains,
        [Description("Not Contains")]
        NotContains
    }

    public enum OrderStatus
    {
        [Description("In process")]
        InProcess = 1,
        [Description("Approved")]
        Approved,
        [Description("Backordered")]
        Backordered,
        [Description("Rejected")]
        Rejected,
        [Description("Shipped")]
        Shipped,
        [Description("Cancelled")]
        Cancelled,
    }

    [Flags]
    public enum WeekDays
    {
        None = 0, //can be used but not combined in bitwise operations
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Saturday = 64,
        Weekend = Sunday | Saturday,
        //you must use powers of two or combinations of powers of two
        //for bitwise operations to work
    }

    public enum Moods
    {
        NoMood = 0,
        Ambivalent = 1,
        Crabby = 10,
        Grouchy = Crabby - 1,
        Happy = 42,
        SuperHappy = 2 * Happy
    }

    //An enum can derive from any of the following types: byte, sbyte, short, ushort, int, uint, long, ulong. The default is
    //int, and can be changed by specifying the type in the enum definition :
    //If you use a type other than int, you must specify the type using a colon after the enum name :
    public enum Volume : byte
    {
        Low = 1,
        Medium,
        High
    }

}
