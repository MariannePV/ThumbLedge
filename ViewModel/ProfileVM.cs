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
    internal class ProfileVM: Utilities.ViewModelBase
    {
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

        public ICommand UpdateCommand { get; set; }

        public ProfileVM()
        {
            UpdateCommand = new RelayCommand(Update);

            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;
        }

        public void UpdateData()
        {
            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;

            Email = PageModel.Instance.Email;
            Username = PageModel.Instance.Username;
            FullName = PageModel.Instance.FullName;
        }

        private async void Update(object obj)
        {
            bool error = false;
            SuccessMessage = Visibility.Collapsed;

            var passwords = (object[])obj;

            string oldPsswd = "";
            string newPsswd = "";

            if (passwords.Length >= 2)
            {
                var oldPsswdBox = passwords[0] as PasswordBox;
                var newPsswdBox = passwords[1] as PasswordBox;

                if (!(string.IsNullOrEmpty(oldPsswdBox.Password)) && !(string.IsNullOrEmpty(newPsswdBox.Password)))
                {
                    oldPsswd = oldPsswdBox.Password;
                    newPsswd = newPsswdBox.Password;
                } else if (!(string.IsNullOrEmpty(oldPsswdBox.Password)) || !(string.IsNullOrEmpty(newPsswdBox.Password)))
                {
                    //Si un dels dos sí te valors
                    ErrorMessageText = "To change the password you have to insert both, the current one and the new one.";
                    ErrorMessage = Visibility.Visible;
                    error = true;
                }
            }

            //La contrasenya que tindrà l'usuari
            string newPsswdOK = "";

            //Comprovem que no hi ha cap camp obligatori buit
            if (!(string.IsNullOrEmpty(Username)) &&
                !(string.IsNullOrEmpty(FullName)) &&
                !(string.IsNullOrEmpty(Email)))
            {
                //Si es preten canviar la contrasenya
                if (!string.IsNullOrEmpty(newPsswd))
                { 
                    //Comprovem si la contrasenya antiga coicideix realment amb la de l'usuari
                    if (oldPsswd == PageModel.Instance.Password)
                    {
                        newPsswdOK = newPsswd;
                    }
                    else
                    {
                        //Si hi ha camps buits
                        ErrorMessageText = "The current password entered does not match the user's existing password.";
                        ErrorMessage = Visibility.Visible;
                        error = true;
                    }
                }
                else
                {
                    newPsswdOK = PageModel.Instance.Password;
                }

                //Validem el username
                if (!error && UsernameValidation(Username))
                {
                    //Creem l'usuari amb els nous canvis
                    Usuari usuari = new Usuari
                    {
                        Username = Username,
                        Password = newPsswdOK,
                        FullName = FullName,
                        Email = Email
                    };

                    UsuariAPI usuariAPI = new UsuariAPI();

                    //Comprovem si existeix un usuari amb el mateix username
                    List<Usuari> usuarisList = await usuariAPI.GetUsuarisAsync();

                    //Comprovem si el nom d'usuari existeix
                    Usuari usernameExists = usuarisList.FirstOrDefault(u => u.Username == Username);

                    if (usernameExists == null || usernameExists.Username == PageModel.Instance.Username)
                    {
                        PageModel.Instance.Username = Username;
                        PageModel.Instance.Password = newPsswdOK;
                        PageModel.Instance.FullName = FullName;
                        PageModel.Instance.Email = Email;

                        UpdateData();

                        await usuariAPI.UpdateAsync(usuari, PageModel.Instance.Email);
                        ErrorMessage = Visibility.Collapsed;
                        SuccessMessageText = "The data was succesfully updated.";
                        SuccessMessage = Visibility.Visible;
                    }
                    else
                    {
                        //Si hi ha un usuari amb el mateix username
                        ErrorMessageText = "The username is not available. Try anothe one.";
                        ErrorMessage = Visibility.Visible;
                    }
                } else if (!error)
                {
                    //Si hi ha camps buits
                    ErrorMessageText = "The username only allows lowercase, numbers and the following characters: . - _.";
                    ErrorMessage = Visibility.Visible;
                }
            }
            else
            {
                //Si hi ha camps buits
                ErrorMessageText = "The fields for username and full name are mandatory and must be filled out.";
                ErrorMessage = Visibility.Visible;
            }
        }

        private bool UsernameValidation(string text)
        {
            //Només donarem com vàlids els username que tinguin 
            string pattern = @"^[a-z0-9\.\-_]+$";

            //Regular expression
            return Regex.IsMatch(text, pattern);
        }
    }
}
