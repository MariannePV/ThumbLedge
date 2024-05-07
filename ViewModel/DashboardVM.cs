using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ThumbLedge.Model;
using ThumbLedge.Utilities;

namespace ThumbLedge.ViewModel
{
    class DashboardVM: Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;    //Model

        public Uri BackgroundDashURL
        {
            get { return _pageModel.BackgroundDashURL; }
            set { _pageModel.BackgroundDashURL = value; OnPropertyChanged(); }
        }

        public ICommand CloseAppCommand { get; set; }

        public ICommand MinimizeAppCommand { get; set; }

        public DashboardVM()
        {
            _pageModel = new PageModel();
            CloseAppCommand = new RelayCommand(CloseApp);
            MinimizeAppCommand = new RelayCommand(MinimizeApp);
            LoadVideo();
        }

        private void MinimizeApp(object parameter)
        {
            if (parameter is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void CloseApp(object parameter)
        {
            //Passem per paràmetre la finestra
            var window = parameter as Window;
            window?.Close();
        }

        private void LoadVideo()
        {
            try
            {
                string video = "../../Videos/backgroundDashboard.wmv";

                //Creem l'URI mitjançant una ruta relativa
                Uri obj = new Uri(video, UriKind.Relative);
                BackgroundDashURL = obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
    }
}
