using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ThumbLedge.API;
using ThumbLedge.Entities;
using ThumbLedge.Model;
using ThumbLedge.Utilities;

namespace ThumbLedge.ViewModel
{
    class BrainVM: Utilities.ViewModelBase
    {
        public event EventHandler IntelligenceSelected;

        private readonly PageModel _pageModel;    //Model

        public string ImageSource
        {
            get { return _pageModel.ImageSource; }
            set { _pageModel.ImageSource = value; OnPropertyChanged(); }
        }

        public ICommand IntelligenceCommand { get; set; }


        public BrainVM()
        {
            _pageModel = PageModel.Instance;
            IntelligenceCommand = new RelayCommand(IntelligenceClicked);

            ImageSource = "../../Images/thumbLedge_brainOff.png";
        }

        // Mètode per gestionar el clic a la intel·ligència
        private void IntelligenceClicked(object obj)
        {
            PageModel.Instance.IntelligenceName = obj.ToString();

            IntelligenceSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}
