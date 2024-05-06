using MongoDB.Driver.Core.WireProtocol.Messages.Encoders.BinaryEncoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ThumbLedge.Utilities;

namespace ThumbLedge.ViewModel
{
    class LoginVM: Utilities.ViewModelBase
    {
        public event EventHandler SignUpClicked;

        public ICommand SignUpCommand { get; set; }

        public LoginVM()
        {
            SignUpCommand = new RelayCommand(SignUpClick);
        }

        private void SignUpClick(object obj)
        {
            SignUpClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
