using Amazon.Runtime.Internal.Util;
using LiveCharts;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ThumbLedge.API;
using ThumbLedge.Entities;
using ThumbLedge.Model;
using ThumbLedge.Utilities;

namespace ThumbLedge.ViewModel
{
    class DashboardVM: Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;    //Model

        private TreeView _dragSource;
        private Point _startPoint;

        public object ThumbLedgeView
        {
            get { return _pageModel.ThumbLedgeView; }
            set { _pageModel.ThumbLedgeView = value; OnPropertyChanged(); }
        }

        public Uri BackgroundDashURL
        {
            get { return _pageModel.BackgroundDashURL; }
            set { _pageModel.BackgroundDashURL = value; OnPropertyChanged(); }
        }

        public TreeView treeView;
        public TreeView TreeView
        {
            get { return treeView; }
            set { treeView = value; OnPropertyChanged(nameof(treeView)); }
        }

        public ObservableCollection<TreeViewItem> KnowledgeItems { get; set; } = new ObservableCollection<TreeViewItem>();

        public ICommand CloseAppCommand { get; set; }
        public ICommand MinimizeAppCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand BrainCommand { get; set; }
        public ICommand SelectedKnowledge { get; set; }
        public ICommand AddCommand { get; set; }

        public ICommand DropCommand { get; private set; }

        //Event quan s'ha presionat el botó de log out
        public event EventHandler LogoutClicked;

        BrainVM brainVM = new BrainVM();
        ProfileVM profileVM = new ProfileVM();
        ConeixementVM coneixementVM = new ConeixementVM();
        AddKnowledgeVM addKnowledgeVM = new AddKnowledgeVM();
        AddKnowledgeValueVM addKnowledgeValueVM = new AddKnowledgeValueVM();
        EditarDescVM editarDescVM = new EditarDescVM();
        IntelligenceInfoVM intelligenceInfoVM = new IntelligenceInfoVM();

        public DashboardVM()
        {
            _pageModel = PageModel.Instance;
            CloseAppCommand = new RelayCommand(CloseApp);
            MinimizeAppCommand = new RelayCommand(MinimizeApp);
            LoadVideo();

            ThumbLedgeView = brainVM;

            //Commands
            LogoutCommand = new RelayCommand(Logout);
            ProfileCommand = new RelayCommand(Profile);
            BrainCommand = new RelayCommand(Brain);
            SelectedKnowledge = new RelayCommand(selectedKnowledge);
            AddCommand = new RelayCommand(AddKnowledge);
            DropCommand = new RelayCommand(DropExecute);

            //Events
            brainVM.IntelligenceSelected += BrainVM_IntelligenceSelected;
            coneixementVM.AddValueKnowledge += ConeixementVM_AddValueKnowledge;
            addKnowledgeVM.KnowledgeAdded += AddKnowledgeVM_KnowledgeAdded;
            addKnowledgeValueVM.ReturnGraphClicked += returnGraphic;
            editarDescVM.graphClicked += returnGraphic;
            coneixementVM.EditDescription += ConeixementVM_EditDescription;
            editarDescVM.deleteSuccesful += EditarDescVM_deleteSuccesful;

            FillTreeView();
        }

        private async void DropExecute(object obj)
        {
            bool twoLevels = false;

            String draggedConeixementName = PageModel.Instance.SelectedKnowledge.Header.ToString();     //El coneixement que ha estat arrossegat
            String dropConeixementName = PageModel.Instance.DropKnowledge.Header.ToString();    //El coneixement on s'ha deixat

            ConeixementAPI coneixementAPI = new ConeixementAPI();
            Coneixement draggedConeixement = await coneixementAPI.GetConeixementAsync(draggedConeixementName);  //Obtenim la informació del coneixement amb el que estem fent el drag&drop

            Coneixement dropConeixement = await coneixementAPI.GetConeixementAsync(dropConeixementName); ;  //Obtenim la informació del coneixement on s'ha fet el drop

            Coneixement[] oldConeixements = dropConeixement.Coneixements;
            Coneixement[] newConeixements = { };

            if (oldConeixements != null && oldConeixements.Length > 0)
            {
                newConeixements = oldConeixements.Append(draggedConeixement).ToArray();     //Afegim el nou coneixement al final de l'array
            }
            else
            {
                newConeixements = new Coneixement[] { draggedConeixement };     //Si els coneixements estaven buits, creem un nou array
            }

            if (!twoLevels)
            {
                //Eliminem el coneixement
                await coneixementAPI.DeleteAsync(draggedConeixementName);
                await coneixementAPI.UpdateConeixementsAsync(dropConeixement.NomConeixement, newConeixements);  //Afegim els nous valors
            }

            //Actualitzem el tree view
            FillTreeView();
            ThumbLedgeView = brainVM;
        }

        private void BrainVM_IntelligenceSelected(object sender, EventArgs e)
        {
            intelligenceInfoVM.UpdateData();
            ThumbLedgeView = intelligenceInfoVM;
        }

        private void EditarDescVM_deleteSuccesful(object sender, EventArgs e)
        {
            FillTreeView();
            ThumbLedgeView = brainVM;
        }

        private void ConeixementVM_EditDescription(object sender, EventArgs e)
        {
            editarDescVM.UpdateData();
            editarDescVM.getInfo();
            ThumbLedgeView = editarDescVM;
        }

        private void returnGraphic(object sender, EventArgs e)
        {
            coneixementVM.UpdateData();
            coneixementVM.getInfo();
            ThumbLedgeView = coneixementVM;
        }

        private void ConeixementVM_AddValueKnowledge(object sender, EventArgs e)
        {
            addKnowledgeValueVM.UpdateData();
            ThumbLedgeView = addKnowledgeValueVM;
        }

        private void AddKnowledgeVM_KnowledgeAdded(object sender, EventArgs e)
        {
            //Quan s'afegeix un nou coneixement, actualitzem el TreeView
            FillTreeView();
        }

        private void AddKnowledge(object obj)
        {
            addKnowledgeVM.UpdateData();
            ThumbLedgeView = addKnowledgeVM;
        }

        private void selectedKnowledge(object obj)
        {
            if (obj is TreeViewItem selectedItem)
            {
                PageModel.Instance.KnowledgeName = selectedItem.Header.ToString();
                PageModel.Instance.SelectedKnowledge = selectedItem;
                coneixementVM.UpdateData();
                coneixementVM.getInfo();
                ThumbLedgeView = coneixementVM;
            }
        }

        public async void FillTreeView()
        {
            KnowledgeItems.Clear();

            ConeixementAPI coneixementApi = new ConeixementAPI();
            List<Coneixement> coneixementsList = await coneixementApi.GetConeixementsAsync();

            List<Coneixement> userConeixements = coneixementsList.Where(c => c.Username == PageModel.Instance.Username).ToList();

            // Afegim els coneixements al treeview
            foreach (var coneixement in userConeixements)
            {
                // Afegim el coneixement com a tree item
                TreeViewItem mainTreeItem = new TreeViewItem { Header = coneixement.NomConeixement };

                // Mirem si el coneixement te sub-coneixements
                if (coneixement.Coneixements != null && coneixement.Coneixements.Length > 0)
                {
                    // Afegim els sub-coneixements
                    foreach (var subConeixement in coneixement.Coneixements)
                    {
                        mainTreeItem.Items.Add(new TreeViewItem { Header = subConeixement.NomConeixement });
                    }
                }

                // Afegim el valor al tree view
                KnowledgeItems.Add(mainTreeItem);
            }
        }

        private void Brain(object obj)
        {
            ThumbLedgeView = brainVM;
        }

        private void Profile(object obj)
        {
            profileVM.UpdateData();
            ThumbLedgeView = profileVM;
        }

        private void Logout(object obj)
        {
            PageModel.Instance.Username = null;
            PageModel.Instance.Password = null;
            PageModel.Instance.FullName = null;
            PageModel.Instance.Email = null;

            LogoutClicked?.Invoke(this, EventArgs.Empty);
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
                string video = "../../Videos/backgroundDashboard.wmv";

                //Creem l'URI mitjançant una ruta relativa
                Uri obj = new Uri(video, UriKind.Relative);
                BackgroundDashURL = obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        public void SetBrain()
        {
            ThumbLedgeView = brainVM;
        }
    }
}
