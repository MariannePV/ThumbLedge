using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    class AddKnowledgeVM: Utilities.ViewModelBase
    {
        //Event per indicar que s'ha afegit coneixement
        public event EventHandler KnowledgeAdded;

        // Llista d'elements del ComboBox
        private List<string> comboBoxItems;
        public List<string> ComboBoxItems
        {
            get { return comboBoxItems; }
            set { comboBoxItems = value; OnPropertyChanged(nameof(ComboBoxItems)); }
        }

        // Intel·ligència seleccionada
        public string selectedIntel;
        public string SelectedIntel
        {
            get { return selectedIntel; }
            set { selectedIntel = value; OnPropertyChanged(nameof(selectedIntel)); }
        }

        public string knowledgeName { get; set; }
        public string KnowledgeName
        {
            get { return knowledgeName; }
            set { knowledgeName = value; OnPropertyChanged(nameof(KnowledgeName)); }
        }

        public string creationDate { get; set; }
        public string CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; OnPropertyChanged(nameof(creationDate)); }
        }

        public string description { get; set; }
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(nameof(description)); }
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

        // Command per afegir coneixement
        public ICommand AddCommand { get; set; }

        public AddKnowledgeVM()
        {
            ComboBoxItems = new List<string>();
            FillIntelligences();

            AddCommand = new RelayCommand(AddKnowledge);

            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;

            CreationDate = DateTime.Now.ToString("dd/MM/yyyy");
        }

        // Omple la llista d'intel·ligències
        private async void FillIntelligences()
        {
            InteligenciaAPI inteligenciaAPI = new InteligenciaAPI();
            List<Inteligencia> inteligencies = await inteligenciaAPI.GetInteligenciesAsync();

            List<string> nomInteligencies = new List<string>();

            foreach (var intelligence in inteligencies)
            {
                nomInteligencies.Add(intelligence.Name);
            }

            ComboBoxItems = nomInteligencies;
        }

        // Crear coneixement
        private async void AddKnowledge(object obj)
        {
            ErrorMessage = Visibility.Hidden;
            SuccessMessage = Visibility.Collapsed;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                if (!string.IsNullOrEmpty(KnowledgeName) &&
                    !string.IsNullOrEmpty(selectedIntel))
                {
                    ConeixementAPI coneixementAPI = new ConeixementAPI();
                    
                    if(await coneixementAPI.GetConeixementAsync(KnowledgeName) != null)
                    {
                        //Ja existeix un coneixement amb el mateix nom
                        ErrorMessageText = "This knowledge name is already registered. Try another one.";
                        ErrorMessage = Visibility.Visible;
                    } else
                    {
                        Coneixement coneixement = new Coneixement
                        {
                            NomConeixement = KnowledgeName,
                            DataCreacio = DateTime.Now,
                            Descripcio = Description,
                            Valors = { },
                            DatesValors = { },
                            Username = PageModel.Instance.Username,
                            Intelligence = selectedIntel
                        };

                        await coneixementAPI.AddAsync(coneixement);

                        KnowledgeAdded?.Invoke(this, EventArgs.Empty);

                        ErrorMessage = Visibility.Collapsed;
                        SuccessMessageText = "The knowledge was succesfully created.";
                        SuccessMessage = Visibility.Visible;

                        KnowledgeName = "";
                        Description =  "";
                    }

                }
                else
                {
                    //Si hi ha camps buits
                    ErrorMessageText = "You have to assign a name and an intelligence to the knowledge.";
                    ErrorMessage = Visibility.Visible;
                }
            } 
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        //Actualitzem les dades
        public void UpdateData()
        {
            FillIntelligences();
        }
    }
}
