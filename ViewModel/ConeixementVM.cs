using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ThumbLedge.API;
using ThumbLedge.Entities;
using ThumbLedge.Model;
using ThumbLedge.Utilities;

namespace ThumbLedge.ViewModel
{
    class ConeixementVM: Utilities.ViewModelBase
    {
        public event EventHandler AddValueKnowledge;
        public event EventHandler EditDescription;

        public SeriesCollection seriesCollection { get; set; }
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { seriesCollection = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> labels { get; set; }
        public ObservableCollection<string> Labels
        {
            get { return labels; }
            set { labels = value; OnPropertyChanged(); }
        }

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

        public ICommand AddValueCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ConeixementVM()
        {
            AddValueCommand = new RelayCommand(AddValueToKnowledge);
            EditCommand = new RelayCommand(editDescription);
        }

        private void editDescription(object obj)
        {
            EditDescription?.Invoke(this, EventArgs.Empty);
        }

        private void AddValueToKnowledge(object obj)
        {
            AddValueKnowledge?.Invoke(this, EventArgs.Empty);
        }

        public async void getInfo()
        {
            ConeixementAPI coneixementAPI = new ConeixementAPI();
            ConeixementData = await coneixementAPI.GetConeixementAsync(KnowledgeName);

            Description = ConeixementData.Descripcio;

            InitalizeChart();
        }

        //Inicialitzem la gràfica
        public void InitalizeChart()
        {
            if (ConeixementData.Valors != null)
            {
                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = ConeixementData.NomConeixement,
                        Values = new ChartValues<int> ( ConeixementData.Valors ),
                        PointGeometry = null
                    }
                };

                Labels = new ObservableCollection<string>(ConeixementData.DatesValors.Select(date => date.ToString("MMM dd")));
            }
            else
            {
                SeriesCollection = null;
                Labels = new ObservableCollection<string>();
            }
        }

        public void UpdateData()
        {
            KnowledgeName = PageModel.Instance.KnowledgeName;
        }
    }
}
