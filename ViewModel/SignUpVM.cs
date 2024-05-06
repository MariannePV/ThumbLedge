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
    class SignUpVM: Utilities.ViewModelBase
    {
        public event EventHandler LoginButtonClicked;

        public ICommand LoginCommand { get; set; }

        public SignUpVM()
        {
            LoginCommand = new RelayCommand(LoginButtonClick);
        }

        private void LoginButtonClick(object obj)
        {
            LoginButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
