using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CodeSnippets.Enums
{
    //Naming convention, StringComparisonOperatorsEnum 
    public enum StringComparisonOperators
    {
        [Description("Select Operator")]
        None = 0,
        [Description("Equals")]
        Equals = 0,
        [Description("Not Equals")]
        NotEquals = 0,
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
    public enum DaysOfWeek
    {
        None = 0,
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Saturday = 64,
        Weekend = Sunday | Saturday
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

}
