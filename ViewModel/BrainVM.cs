using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThumbLedge.Model;

namespace ThumbLedge.ViewModel
{
    class BrainVM: Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;    //Model

        public string ImageSource
        {
            get { return _pageModel.ImageSource; }
            set { _pageModel.ImageSource = value; OnPropertyChanged(); }
        }

        public BrainVM()
        {
            _pageModel = PageModel.Instance;

            ImageSource = "../../Images/thumbLedge_brainOff.png";
        }
    }
}
