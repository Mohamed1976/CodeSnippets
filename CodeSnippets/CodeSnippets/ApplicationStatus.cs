using System;
using System.ComponentModel;

namespace CodeSnippets
{
    /// <summary>
    /// Class representing whether the application is currently busy.  
    /// Tracks the number of times EnterBusy() and ExitBusy() has been called 
    /// and aggregates this into an effective 'true' or 'false' accordingly.
    /// </summary>
    public sealed class ApplicationStatus : INotifyPropertyChanged, IApplicationStatus
    {
        private int _busyCounter;
        private string _message;
        private bool _isBusy;

        public ApplicationStatus()
        {
            _busyCounter = 0;
            _isBusy = false;
            _message = string.Empty;
        }

        public void EnterBusy()
        {
            _busyCounter++;
            if (_busyCounter == 1)
            {
                IsBusy = true;
            }
        }

        public void ExitBusy()
        {
            if (_busyCounter == 0)
            {
                throw new InvalidOperationException("BusyCounter is already zero.");
            }

            _busyCounter--;
            if (_busyCounter == 0)
            {
                IsBusy = false;
            }
        }

        public void ClearBusy()
        {
            _busyCounter = 0;
            IsBusy = false;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public void SetMessage(SeverityType severityType, string message = null)
        {
            if (severityType == SeverityType.Info ||
                severityType == SeverityType.Warning ||
                severityType == SeverityType.Error)
            {
                Message = string.Format("{0}: {1}", severityType, message);
            }
            else
            {
                Message = string.Empty;
            }
        }

        public string Message
        {
            get { return _message; }
            private set
            {
                _message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        #region [ INotifyPropertyChanged implementation ]

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
