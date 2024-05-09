using Amazon.Runtime.Internal.Util;
using Prism.Commands;
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

        public object ThumbLedgeView
        {
            get { return _pageModel.ThumbLedgeView; }
            set { _pageModel.ThumbLedgeView = value; OnPropertyChanged(); }
        }

        public Uri BackgroundDashURL
        {
            get { return _pageModel.BackgroundDashURL; }
            set { _pageModel.BackgroundDashURL = value; OnPropertyChanged(); }
        }

        public ICommand CloseAppCommand { get; set; }

        public ICommand MinimizeAppCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

        public ICommand ProfileCommand { get; set; }

        public ICommand BrainCommand { get; set; }

        //Event quan s'ha presionat el botó de log out
        public event EventHandler LogoutClicked;

        BrainVM brainVM = new BrainVM();
        ProfileVM profileVM = new ProfileVM();

        public DashboardVM()
        {
            _pageModel = PageModel.Instance;
            CloseAppCommand = new RelayCommand(CloseApp);
            MinimizeAppCommand = new RelayCommand(MinimizeApp);
            LoadVideo();

            ThumbLedgeView = brainVM;

            LogoutCommand = new RelayCommand(Logout);
            ProfileCommand = new RelayCommand(Profile);
            BrainCommand = new RelayCommand(Brain);
        }

        private void Brain(object obj)
        {
            ThumbLedgeView = brainVM;
        }

        private void Profile(object obj)
        {
            profileVM.UpdateData();
            ThumbLedgeView = profileVM;
        }

        private void Logout(object obj)
        {
            PageModel.Instance.Username = null;
            PageModel.Instance.Password = null;
            PageModel.Instance.FullName = null;
            PageModel.Instance.Email = null;

            LogoutClicked?.Invoke(this, EventArgs.Empty);
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
