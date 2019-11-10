using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets
{
    public interface IApplicationStatus
    {
        void EnterBusy();
        void ExitBusy();
        void ClearBusy();
        bool IsBusy { get; }
        void SetMessage(SeverityType severityType, string message = null);
        string Message { get; }
    }
}
