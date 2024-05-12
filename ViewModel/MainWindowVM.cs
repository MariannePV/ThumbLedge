using MongoDB.Driver.Core.Authentication;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using ThumbLedge.Model;
using ThumbLedge.Utilities;
using ThumbLedge.View;

namespace ThumbLedge.ViewModel
{
    class MainWindowVM: Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;    //Model

        public object MainView
        {
            get { return _pageModel.MainView; }
            set { _pageModel.MainView = value; OnPropertyChanged(); }
        }

        StartupVM startupVM = new StartupVM();
        DashboardVM dashboardVM = new DashboardVM();

        public MainWindowVM()
        {
            _pageModel = PageModel.Instance;

            MainView = startupVM;

            //Ens subscribim a l'event quan des de startup presionen el botó de Log in
            startupVM.LoginClicked += StartupVM_LoginClicked;
            dashboardVM.LogoutClicked += DashboardVM_LogoutClicked;
        }

        private void DashboardVM_LogoutClicked(object sender, EventArgs e)
        {
            MainView = startupVM;
        }

        private void StartupVM_LoginClicked(object sender, EventArgs e)
        {
            dashboardVM.FillTreeView();
            dashboardVM.SetBrain();
            MainView = dashboardVM;
        }
    }
}
