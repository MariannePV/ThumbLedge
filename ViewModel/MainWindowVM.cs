using MongoDB.Driver.Core.Authentication;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using ThumbLedge.Model;
using ThumbLedge.Utilities;
using ThumbLedge.View;

namespace ThumbLedge.ViewModel
{
    class MainWindowVM: Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;    //Model

        public object CurrentView
        {
            get { return _pageModel.CurrentView; }
            set { _pageModel.CurrentView = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get { return _pageModel.Username; }
            set { _pageModel.Username = value; OnPropertyChanged(); }
        }

        public bool LoggedIn
        {
            get { return _pageModel.LoggedIn; }
            set { _pageModel.LoggedIn = value; OnPropertyChanged(); }
        }

        public Uri BackgroundURL
        {
            get { return _pageModel.BackgroundURL; }
            set { _pageModel.BackgroundURL = value; OnPropertyChanged(); }
        }

        public ICommand CloseAppCommand { get; set; }

        public ICommand MinimizeAppCommand { get; set; }

        HomeVM homeVM = new HomeVM();
        LoginVM loginVM = new LoginVM();
        SignUpVM signUpVM = new SignUpVM();

        public MainWindowVM()
        {
            _pageModel = new PageModel();
            CloseAppCommand = new RelayCommand(CloseApp);
            MinimizeAppCommand = new RelayCommand(MinimizeApp);
            LoadVideo();

            //Definim com a vista predeterminada la de HomeVM
            CurrentView = homeVM;

            //Creem una instància de homeVM i ens subscribim a l'event
            homeVM.StartButtonClicked += HomeVM_StartButtonClicked;
            loginVM.SignUpClicked += LoginVM_SignUpClicked;
            signUpVM.LoginButtonClicked += SignUpVM_LoginButtonClicked;
        }

        private void SignUpVM_LoginButtonClicked(object sender, EventArgs e)
        {
            CurrentView = loginVM;
        }

        private void LoginVM_SignUpClicked(object sender, EventArgs e)
        {
            CurrentView = signUpVM;
        }

        private void HomeVM_StartButtonClicked(object sender, EventArgs e)
        {
            //Canviem la vista actual a LoginVM
            CurrentView = loginVM;
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
                string video = "../../Videos/backgroundStart.wmv";

                //Creem l'URI mitjançant una ruta relativa
                Uri obj = new Uri(video, UriKind.Relative);
                BackgroundURL = obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
    }
}
