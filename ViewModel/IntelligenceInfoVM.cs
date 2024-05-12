using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThumbLedge.API;
using ThumbLedge.Entities;
using ThumbLedge.Model;

namespace ThumbLedge.ViewModel
{
    class IntelligenceInfoVM: Utilities.ViewModelBase
    {
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

        public string coneixementMesTreballat { get; set; }
        public string ConeixementMesTreballat
        {
            get { return coneixementMesTreballat; }
            set { coneixementMesTreballat = value; OnPropertyChanged(); }
        }

        public string ultimConeixementAfegit { get; set; }
        public string UltimConeixementAfegit
        {
            get { return ultimConeixementAfegit; }
            set { ultimConeixementAfegit = value; OnPropertyChanged(); }
        }

        public string intelligenceName { get; set; }
        public string IntelligenceName
        {
            get { return intelligenceName; }
            set { intelligenceName = value; OnPropertyChanged(); }
        }

        public IntelligenceInfoVM() { }

        public async void GetData()
        {
            ConeixementAPI coneixementApi = new ConeixementAPI();
            List<Coneixement> coneixementsList = await coneixementApi.GetConeixementsAsync();

            //Obtenim tots els coneixements associats a la intel·ligència en qüestió
            List<Coneixement> userConeixements = coneixementsList.Where(c => c.Intelligence == PageModel.Instance.IntelligenceName && c.Username == PageModel.Instance.Username).ToList();
            var coneixementMesRecent = userConeixements.OrderByDescending(c => c.DataCreacio).FirstOrDefault();

            Coneixement coneixementWithMostValues = null;
            int maxTotalValues = 0;

            foreach (var coneixement in userConeixements)
            {
                if (coneixement.Valors != null && coneixement.Valors.Length > 0)
                {
                    // Busquem el que més quantitat de valors té
                    int totalValues = coneixement.Valors.Length;

                    // Mirem si el coneixement actual té més valors que el nombre màxim de valors trobat
                    if (totalValues > maxTotalValues)
                    {
                        maxTotalValues = totalValues;
                        coneixementWithMostValues = coneixement;
                    }
                }
            }

            if (coneixementWithMostValues != null &&
                coneixementMesRecent != null)
            {
                ConeixementMesTreballat = coneixementWithMostValues.NomConeixement;
                UltimConeixementAfegit = coneixementMesRecent.NomConeixement;
            } else
            {
                ConeixementMesTreballat = "";
                UltimConeixementAfegit = "";
            }

            InitalizeChart(userConeixements);
        }

        public void InitalizeChart(List<Coneixement> coneixements)
        {
            //Agafem els 5 coneixements que tenen més valors
            var sortedConeixements = coneixements.OrderByDescending(c => c.Valors?.Length ?? 0).Take(5);


            // Inicialitzem SeriesCollection i Labels
            SeriesCollection = new SeriesCollection();
            Labels = new ObservableCollection<string>();

            foreach (var coneixement in sortedConeixements)
            {
                if (coneixement.Valors != null)
                {
                    // Oer cada coneixement afegim un LineSeries
                    SeriesCollection.Add(new LineSeries
                    {
                        Title = coneixement.NomConeixement,
                        Values = new ChartValues<int>(coneixement.Valors),
                        PointGeometry = null
                    });

                    Labels.AddRange(coneixement.DatesValors.Select(date => date.ToString("MMM dd")));
                }
            }

            if (SeriesCollection.Count == 0)
            {
                SeriesCollection = null;
            }
        }

        public void UpdateData()
        {
            GetData();
            intelligenceName = PageModel.Instance.IntelligenceName;
        }
    }
}
