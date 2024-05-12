using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThumbLedge.API;
using ThumbLedge.Model;
using ThumbLedge.Utilities;
using ThumbLedge.Entities;
using System.Windows;

namespace ThumbLedge.ViewModel
{
    class EditarDescVM: Utilities.ViewModelBase
    {
        public event EventHandler graphClicked;
        public event EventHandler deleteSuccesful;

        public string knowledgeName { get; set; }
        public string KnowledgeName
        {
            get { return knowledgeName; }
            set { knowledgeName = value; OnPropertyChanged(); }
        }

        public string description { get; set; }
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        private Coneixement coneixementData { get; set; }
        public Coneixement ConeixementData
        {
            get { return coneixementData; }
            set { coneixementData = value; OnPropertyChanged(); }
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

        public ICommand GraphCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public EditarDescVM()
        {
            GraphCommand = new RelayCommand(clickGraph);
            SaveCommand = new RelayCommand(updateDescription);
            RemoveCommand = new RelayCommand(removeKnowledge);

            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;
        }

        private async void removeKnowledge(object obj)
        {
            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;

            ConeixementAPI coneixementAPI = new ConeixementAPI();

            if (ConeixementData.Coneixements != null)
            {
                ErrorMessageText = "You can't remove a knowledge if it has attached knowledges.";
                ErrorMessage = Visibility.Visible;
            } else
            {
                await coneixementAPI.DeleteAsync(KnowledgeName);

                SuccessMessageText = "The knowledge was succesfully removed.";
                SuccessMessage = Visibility.Visible;

                //Ens esperem dos segons i activem l'event
                await Task.Delay(2000);

                deleteSuccesful?.Invoke(this, EventArgs.Empty);
            }
        }

        //Actualitzem la descripció
        private async void updateDescription(object obj)
        {
            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;

            Coneixement updatedConeixement = new Coneixement
            {
                NomConeixement = KnowledgeName,
                DataCreacio = ConeixementData.DataCreacio,
                Descripcio = Description,
                Valors = ConeixementData.Valors,
                DatesValors = coneixementData.DatesValors,
                Coneixements = coneixementData.Coneixements                
            };

            ConeixementAPI coneixementAPI = new ConeixementAPI();
            await coneixementAPI.UpdateAsync(updatedConeixement, KnowledgeName);

            SuccessMessageText = "The data was succesfully updated.";
            SuccessMessage = Visibility.Visible;
        }

        private void clickGraph(object obj)
        {
            graphClicked?.Invoke(this, EventArgs.Empty);
        }

        public async void getInfo()
        {
            ConeixementAPI coneixementAPI = new ConeixementAPI();
            ConeixementData = await coneixementAPI.GetConeixementAsync(KnowledgeName);

            Description = ConeixementData.Descripcio;
        }

        public void UpdateData()
        {
            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;
            KnowledgeName = PageModel.Instance.KnowledgeName;
        }
    }
}
