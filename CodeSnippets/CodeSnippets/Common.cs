using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets
{
    /// <summary>
    /// Describes the severity of the message sent in <see cref="ApplicationStatus.SetMessage(string, SeverityType)"/>.
    /// </summary>
    public enum SeverityType
    {
        /// <summary>
        /// Clear, the default value.
        /// </summary>
        Clear = 0,

        /// <summary>
        /// Display info message.
        /// </summary>
        Info,

        /// <summary>
        /// Display warning message.
        /// </summary>
        Warning,

        /// <summary>
        /// Display error message.
        /// </summary>
        Error,
    }
}
