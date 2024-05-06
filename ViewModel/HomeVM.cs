using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ThumbLedge.Model;
using ThumbLedge.Utilities;
using ThumbLedge.View;

namespace ThumbLedge.ViewModel
{
    class HomeVM: Utilities.ViewModelBase
    {
        //Declarem un event que controlarà quan es polsa el botó
        public event EventHandler StartButtonClicked;

        public ICommand StartButton { get; set; }

        public HomeVM()
        {
            StartButton = new RelayCommand(StartButtonClick);
        }

        private void StartButtonClick(object obj)
        {
            //Disparem l'event indicant que s'ha fet click al botó START
            StartButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
