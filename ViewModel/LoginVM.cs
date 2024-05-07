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
        public event EventHandler ForgotPasswordClicked;
        public event EventHandler EnterClicked;

        public ICommand SignUpCommand { get; set; }

        public ICommand ForgotPasswordCommand { get; set; }

        public ICommand EnterCommand { get; set; }

        public LoginVM()
        {
            SignUpCommand = new RelayCommand(SignUpClick);
            ForgotPasswordCommand = new RelayCommand(ForgotPasswordClick);
            EnterCommand = new RelayCommand(EnterClick);
        }

        private void EnterClick(object obj)
        {
            EnterClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ForgotPasswordClick(object obj)
        {
            ForgotPasswordClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SignUpClick(object obj)
        {
            SignUpClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
