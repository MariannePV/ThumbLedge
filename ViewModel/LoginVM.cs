using MongoDB.Driver.Core.WireProtocol.Messages.Encoders.BinaryEncoders;
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
    class LoginVM: Utilities.ViewModelBase
    {
        public event EventHandler SignUpClicked;
        public event EventHandler ForgotPasswordClicked;
        public event EventHandler EnterClicked;

        public string usernameView { get; set; }
        public string Username
        {
            get { return usernameView; }
            set { usernameView = value; OnPropertyChanged(); }
        }

        public Visibility errorMssg { get; set; }
        public Visibility ErrorMessage
        {
            get { return errorMssg; }
            set { errorMssg = value; OnPropertyChanged(nameof(errorMssg)); }
        }

        public ICommand SignUpCommand { get; set; }

        public ICommand ForgotPasswordCommand { get; set; }

        public ICommand EnterCommand { get; set; }

        public LoginVM()
        {
            SignUpCommand = new RelayCommand(SignUpClick);
            ForgotPasswordCommand = new RelayCommand(ForgotPasswordClick);
            EnterCommand = new RelayCommand(EnterClick);

            ErrorMessage = Visibility.Hidden;
        }

        private async void EnterClick(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;

            try
            {
                //Canviem el cursor per notificar a l'usuari que s'està duent a terme un procés
                Mouse.OverrideCursor = Cursors.Wait;

                //Si la contrasenya no es null o està buida
                if (!(string.IsNullOrEmpty(password)) && !(string.IsNullOrEmpty(Username)))
                {
                    UsuariAPI usuariAPI = new UsuariAPI();
                    List<Usuari> usuarisList = await usuariAPI.GetUsuarisAsync();

                    Usuari usuariAutentificat = usuarisList.FirstOrDefault(u => u.Username == Username && u.Password == password);

                    if (usuariAutentificat != null)
                    {
                        PageModel.Instance.Username = Username;
                        PageModel.Instance.Password = password;
                        PageModel.Instance.FullName = usuariAutentificat.FullName;
                        PageModel.Instance.Email = usuariAutentificat.Email;

                        ErrorMessage = Visibility.Hidden;
                        
                        //Si la sessió s'inicia correctament, llavors invoquem l'event
                        EnterClicked?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        //Missatge d'error
                        ErrorMessage = Visibility.Visible;
                    }
                } else
                {
                    //Missatge d'error
                    ErrorMessage = Visibility.Visible;
                }
            } finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ForgotPasswordClick(object obj)
        {
            ErrorMessage = Visibility.Hidden;

            ForgotPasswordClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SignUpClick(object obj)
        {
            ErrorMessage = Visibility.Hidden;

            SignUpClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
