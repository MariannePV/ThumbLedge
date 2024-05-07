using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ThumbLedge.Model;
using ThumbLedge.Utilities;

namespace ThumbLedge.ViewModel
{
    class StartupVM: Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;    //Model

        public object CurrentView
        {
            get { return _pageModel.CurrentView; }
            set { _pageModel.CurrentView = value; OnPropertyChanged(); }
        }

        public Uri BackgroundURL
        {
            get { return _pageModel.BackgroundURL; }
            set { _pageModel.BackgroundURL = value; OnPropertyChanged(); }
        }

        public ICommand CloseAppCommand { get; set; }

        public ICommand MinimizeAppCommand { get; set; }

        //Event quan s'ha presionat el botó login des de LoginVM
        public event EventHandler LoginClicked;

        HomeVM homeVM = new HomeVM();
        LoginVM loginVM = new LoginVM();
        SignUpVM signUpVM = new SignUpVM();
        LostPasswordVM lostPasswordVM = new LostPasswordVM();

        public StartupVM()
        {
            _pageModel = new PageModel();
            CloseAppCommand = new RelayCommand(CloseApp);
            MinimizeAppCommand = new RelayCommand(MinimizeApp);
            LoadVideo();

            //Definim com a vista predeterminada la de HomeVM
            CurrentView = homeVM;

            //Creem una instància de homeVM i ens subscribim a l'event
            homeVM.StartButtonClicked += goToLogin;
            loginVM.SignUpClicked += goToSignUp;
            loginVM.ForgotPasswordClicked += goToLostPassword;
            signUpVM.LoginButtonClicked += goToLogin;
            lostPasswordVM.LoginBtnClicked += goToLogin;
            lostPasswordVM.SignUpBtnClicked += goToSignUp;

            loginVM.EnterClicked += enterClicked;
        }

        private void enterClicked(object sender, EventArgs e)
        {
            LoginClicked?.Invoke(this, EventArgs.Empty);
        }

        private void goToLostPassword(object sender, EventArgs e)
        {
            CurrentView = lostPasswordVM;
        }

        private void goToSignUp(object sender, EventArgs e)
        {
            CurrentView = signUpVM;
        }

        private void goToLogin(object sender, EventArgs e)
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
