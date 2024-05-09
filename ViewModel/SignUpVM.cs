using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    class SignUpVM: Utilities.ViewModelBase
    {
        public event EventHandler LoginButtonClicked;

        public string usernameTxt { get; set; }
        public string Username
        {
            get { return usernameTxt; }
            set { usernameTxt = value; OnPropertyChanged(); }
        }

        public string fullNameTxt { get; set; }
        public string FullName
        {
            get { return fullNameTxt; }
            set { fullNameTxt = value; OnPropertyChanged(); }
        }

        public string emailTxt { get; set; }
        public string Email
        {
            get { return emailTxt; }
            set { emailTxt = value; OnPropertyChanged(); }
        }

        //Error message
        public string errorMssgText { get; set; }
        public string ErrorMessageText
        {
            get { return errorMssgText; }
            set { errorMssgText = value; OnPropertyChanged(nameof(errorMssgText)); }
        }

        public Visibility errorMssg { get; set; }
        public Visibility ErrorMessage
        {
            get { return errorMssg; }
            set { errorMssg = value; OnPropertyChanged(nameof(errorMssg)); }
        }

        //Success message
        public string successMssgText { get; set; }
        public string SuccessMessageText
        {
            get { return successMssgText; }
            set { successMssgText = value; OnPropertyChanged(nameof(successMssgText)); }
        }

        public Visibility successMssg { get; set; }
        public Visibility SuccessMessage
        {
            get { return successMssg; }
            set { successMssg = value; OnPropertyChanged(nameof(successMssg)); }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        public SignUpVM()
        {
            LoginCommand = new RelayCommand(LoginButtonClick);
            SignUpCommand = new RelayCommand(SignUpClick);

            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;
        }

        private bool UsernameValidation(string text)
        {
            //Només donarem com vàlids els username que tinguin 
            string pattern = @"^[a-z0-9\.\-_]+$";

            //Regular expression
            return Regex.IsMatch(text, pattern);
        }

        private bool EmailValidation(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            return Regex.IsMatch(email, pattern);
        }

        private async void SignUpClick(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;

            try
            {
                //Canviem el cursor per notificar a l'usuari que s'està duent a terme un procés
                Mouse.OverrideCursor = Cursors.Wait;

                //Si cap dels camps està buit
                if (!(string.IsNullOrEmpty(Username)) &&
                    !(string.IsNullOrEmpty(password)) &&
                    !(string.IsNullOrEmpty(FullName)) &&
                    !(string.IsNullOrEmpty(Email)))
                {
                    //Comprovem que el username tingui només minúscules . _ -
                    if (UsernameValidation(Username))
                    {
                        UsuariAPI usuariAPI = new UsuariAPI();
                        List<Usuari> usuarisList = await usuariAPI.GetUsuarisAsync();

                        //Comprovem si el nom d'usuari existeix
                        Usuari usernameExists = usuarisList.FirstOrDefault(u => u.Username == Username);

                        if (usernameExists == null)
                        {
                            if (EmailValidation(Email))
                            {
                                Usuari usuari = new Usuari
                                {
                                    Username = Username,
                                    Password = password,
                                    FullName = FullName,
                                    Email = Email
                                };

                                //Comprovar si existeix
                                if (await usuariAPI.GetUsuariAsync(usuari.Email) != null)
                                {
                                    //Si el compte de correu ja està registrat
                                    ErrorMessageText = "This email account is already registered. Try to log in.";
                                    ErrorMessage = Visibility.Visible;
                                }
                                else
                                {
                                    await usuariAPI.AddAsync(usuari);

                                    ErrorMessage = Visibility.Collapsed;
                                    SuccessMessageText = "The account was succesfully created. Redirecting to log in.";
                                    SuccessMessage = Visibility.Visible;

                                    await Task.Delay(2000);

                                    Username = "";
                                    FullName = "";
                                    Email = "";

                                    //Si el compte es crea correctament, llavors invoquem l'event
                                    LoginButtonClicked?.Invoke(this, EventArgs.Empty);
                                }                            
                            } else
                            {
                                //Si el compte de correu no és vàlid
                                ErrorMessageText = "The email account doesn't have a valid format. Try again.";
                                ErrorMessage = Visibility.Visible;
                            }
                        }
                        else
                        {
                            //Si hi ha un usuari amb el mateix username
                            ErrorMessageText = "The username is not available. Try anothe one.";
                            ErrorMessage = Visibility.Visible;
                        }

                    } else
                    {
                        //Si el username no és vàlid (estructura)
                        ErrorMessageText = "The username only allows lowercase, numbers and the following characters: . - _.";
                        ErrorMessage = Visibility.Visible;
                    }
                }
                else
                {
                    //Si hi ha camps buits
                    ErrorMessageText = "There are empty fields. Check your data please.";
                    ErrorMessage = Visibility.Visible;
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void LoginButtonClick(object obj)
        {
            Username = "";
            FullName = "";
            Email = "";

            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;

            LoginButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
