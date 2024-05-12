using Newtonsoft.Json.Converters;
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
    class AddKnowledgeValueVM: Utilities.ViewModelBase
    {
        public event EventHandler ReturnGraphClicked;

        public string knowledgeName { get; set; }
        public string KnowledgeName
        {
            get { return knowledgeName; }
            set { knowledgeName = value; OnPropertyChanged(); }
        }

        private int knowledgeValue { get; set; }
        public int KnowledgeValue
        {
            get { return knowledgeValue; }
            set { knowledgeValue = value; OnPropertyChanged(); }
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

        public ICommand GraphCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand RadioBtnCommand { get; set; }

        public AddKnowledgeValueVM()
        {
            GraphCommand = new RelayCommand(returnGraph);
            RegisterCommand = new RelayCommand(registerValue);
            RadioBtnCommand = new RelayCommand(selectedKnowledge);

            SuccessMessage = Visibility.Collapsed;
        }

        // Mètode per seleccionar el coneixement
        private void selectedKnowledge(object obj)
        {
            if (int.TryParse(obj.ToString(), out int value))
            {
                KnowledgeValue = value;
            }
        }

        // Mètode per registrar el valor
        private async void registerValue(object obj)
        {
            //Registrem un nou valor de coneixement
            ConeixementAPI coneixementAPI = new ConeixementAPI();
            Coneixement coneixement = await coneixementAPI.GetConeixementAsync(KnowledgeName);

            int[] valorsConeixement = coneixement.Valors;
            DateTime[] dateValors = coneixement.DatesValors;

            DateTime ultimaData = DateTime.Now;

            int[] nouValorsConeixement = { };

            DateTime[] nouDateValors = { };

            if (valorsConeixement != null && valorsConeixement.Length > 0)
            {
                // Obtenim l'últim valor de l'array valorsConeixement
                int ultimValor = valorsConeixement.LastOrDefault();

                int nouValor = ultimValor + KnowledgeValue;

                //Afegim el nou valor al final de l'array
                nouValorsConeixement = valorsConeixement.Append(nouValor).ToArray();

                // Afegim la nova data al final de l'array
                DateTime[] prova = dateValors.Append(ultimaData).ToArray();

                nouDateValors = prova;
            } else
            {
                nouValorsConeixement = new int[] { KnowledgeValue };
                nouDateValors = new DateTime[] { ultimaData };
            }

            await coneixementAPI.UpdateValorsAsync(KnowledgeName, nouValorsConeixement, nouDateValors);

            SuccessMessageText = "The new value was inserted correctly.";
            SuccessMessage = Visibility.Visible;
        }

        private void returnGraph(object obj)
        {
            ReturnGraphClicked?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateData()
        {
            SuccessMessage = Visibility.Collapsed;
            KnowledgeValue = 0;
            KnowledgeName = PageModel.Instance.KnowledgeName;
        }
    }
}
