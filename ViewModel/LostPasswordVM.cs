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
    class LostPasswordVM: Utilities.ViewModelBase
    {
        public event EventHandler LoginBtnClicked;
        public event EventHandler SignUpBtnClicked;

        public ICommand LoginCommand { get; set; }

        public ICommand SignUpCommand { get; set; }

        public LostPasswordVM()
        {
            LoginCommand = new RelayCommand(LoginBtnClick);
            SignUpCommand = new RelayCommand(SignUpBtnClick);
        }

        private void SignUpBtnClick(object obj)
        {
            SignUpBtnClicked?.Invoke(this, EventArgs.Empty);
        }

        private void LoginBtnClick(object obj)
        {
            LoginBtnClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
